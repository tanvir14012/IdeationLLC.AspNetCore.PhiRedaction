using Microsoft.Extensions.Configuration;
using PhiRedaction.Core.Models.Redaction;
using PhiRedaction.Core.Models.Settings;
using PhiRedaction.Core.Services.Settings;
using System.Text;

namespace PhiRedaction.Core.Services.Redaction.LabOrder
{
    public class LabOrderRedactionService : IRedactionService
    {
        private readonly LabOrderParseSettings _parseSettings;

        public LabOrderRedactionService()
        {
            SettingsManager.Initialize();
            _parseSettings = SettingsManager.Configuration.GetSection("LabOrderFileParse")
                .Get<LabOrderParseSettings>();
        }
        public async Task<RedactionResult> RedactAsync(string inputFilePath)
        {
            var result = new RedactionResult();

            if (string.IsNullOrEmpty(inputFilePath))
            {
                result.Message = "The lab order file was not uploaded successfully.";
                return await Task.FromResult(result);
            }

            if (!File.Exists(inputFilePath))
            {
                result.Message = "The lab order file was not uploaded successfully or was deleted by another program.";
                return await Task.FromResult(result);
            }
            
            try
            {
                var fileInfo = new FileInfo(inputFilePath);
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.Name);
                var outputFilePath = Path.Combine(fileInfo.DirectoryName,
                    _parseSettings.ProcessDirectoryRelative, $"{fileNameWithoutExtension}{_parseSettings.ProcessedIndicatorSuffix}{fileInfo.Extension}");
                var outputDir = Path.GetDirectoryName(outputFilePath);
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
                if (File.Exists(outputFilePath))
                {
                    outputFilePath = outputFilePath.Replace(fileInfo.Extension, $"{Guid.NewGuid().ToString()}_{fileInfo.Extension}");
                }

                using (var fs = File.Open(inputFilePath, FileMode.Open, FileAccess.Read))
                using (var writeStream = File.Open(outputFilePath, FileMode.Create, FileAccess.Write))
                {
                    string line;
                    var prevLineParseResult = new LineParseResult();
                    while ((line = FileStreamHelper.ReadLineWithBufferLimit(fs, _parseSettings.MaxLineSizeInBytes)).Length > 0)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        var currentLineParseResult = RedactionHelper.ParseLine(line, _parseSettings.FieldDelimiters);
                        if (!currentLineParseResult.IsMultilineField)
                        {
                            if (prevLineParseResult.IsMultilineField)
                            {
                                prevLineParseResult = RedactionHelper.RedactField(prevLineParseResult, _parseSettings.RedactionPatterns);
                                await writeStream.WriteAsync(Encoding.UTF8.GetBytes(prevLineParseResult.FieldName + prevLineParseResult.Delimiter));
                                prevLineParseResult.FieldValue = prevLineParseResult.FieldValue;
                                await writeStream.WriteAsync(Encoding.UTF8.GetBytes(prevLineParseResult.FieldValue));
                            }
                            prevLineParseResult = currentLineParseResult;

                            currentLineParseResult = RedactionHelper.RedactField(currentLineParseResult, _parseSettings.RedactionPatterns);
                            await writeStream.WriteAsync(Encoding.UTF8.GetBytes(currentLineParseResult.FieldName + currentLineParseResult.Delimiter));
                            await writeStream.WriteAsync(Encoding.UTF8.GetBytes(currentLineParseResult.FieldValue));
                        }
                        else
                        {
                            prevLineParseResult.FieldName = !string.IsNullOrWhiteSpace(currentLineParseResult.FieldName)
                                ? currentLineParseResult.FieldName : prevLineParseResult.FieldName;
                            if(prevLineParseResult.IsMultilineField)
                                prevLineParseResult.FieldValue += currentLineParseResult.FieldValue;
                            else
                                prevLineParseResult.FieldValue = currentLineParseResult.FieldValue;
                            prevLineParseResult.IsMultilineField = true;
                        }

                    }
                    if (prevLineParseResult.IsMultilineField)
                    {
                        prevLineParseResult = RedactionHelper.RedactField(prevLineParseResult, _parseSettings.RedactionPatterns);
                        await writeStream.WriteAsync(Encoding.UTF8.GetBytes(prevLineParseResult.FieldName + prevLineParseResult.Delimiter));
                        prevLineParseResult.FieldValue = prevLineParseResult.FieldValue;
                        await writeStream.WriteAsync(Encoding.UTF8.GetBytes(prevLineParseResult.FieldValue));
                    }

                    result.IsSuccess = true;
                    result.Message = "The lab order file was redacted successfully.";
                    result.OutputFilePath = outputFilePath;
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                result.Message = "The uploaded file access is denied by the operating system. Please update the directory access permission.";
            }
            catch (IOException ex)
            {
                result.Message = "Something went wrong. Please try again later.";
            }
            catch (Exception ex)
            {
                result.Message = "Something went wrong. Please contact support.";
            }

            return result;
        }
    }
}
