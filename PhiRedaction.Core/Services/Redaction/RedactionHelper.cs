using PhiRedaction.Core.Models.Redaction;
using PhiRedaction.Core.Models.Settings;
using System.Text.RegularExpressions;

namespace PhiRedaction.Core.Services.Redaction
{
    public static class RedactionHelper
    {
        /// <summary>
        /// Assumes each field starts on a new line and is separated by a delimiter, and multi-line fields contain no value in the first line.
        /// </summary>
        public static LineParseResult ParseLine(string line, string[] fieldDelimiters)
        {
            var result = new LineParseResult();
            var splitSuccess = false;

            foreach(var delimiter in fieldDelimiters)
            {
                if (line.Contains(delimiter))
                {
                    var fragments = line.Split(delimiter);
                    if(fragments.Length >= 2)
                    {
                        result.IsField = true;
                        result.FieldName = fragments[0].NormalizeSpaces();
                        result.FieldValue = fragments[1].NormalizeSpaces();
                        result.IsMultilineField = string.IsNullOrWhiteSpace(result.FieldValue) || result.FieldValue.Trim().Equals(Environment.NewLine);
                        result.Delimiter = delimiter;
                        splitSuccess = true;
                        break;
                    }
                }
            }

            if(!splitSuccess)
            {
                result.FieldValue = line.NormalizeSpaces();
                result.IsMultilineField = true;
            }

            return result;
        }

        public static LineParseResult RedactField(LineParseResult parseResult, RedactionPattern[] patterns)
        {
            if(string.IsNullOrWhiteSpace(parseResult.FieldName) && !parseResult.IsMultilineField)
                return parseResult;

            foreach (var pattern in patterns)
            {
                if(!parseResult.IsMultilineField)
                {
                    if(Regex.IsMatch(parseResult.FieldName, pattern.Pattern, RegexOptions.IgnoreCase))
                    {
                        parseResult.FieldValue = pattern.Replacement + Environment.NewLine;
                        break;
                    }   
                }
                else
                {
                    if (Regex.IsMatch(parseResult.FieldValue, pattern.Pattern))
                    {
                        parseResult.FieldValue = Regex.Replace(parseResult.FieldValue, pattern.Pattern, pattern.Replacement + Environment.NewLine, RegexOptions.IgnoreCase);
                    }
                }
            }

            return parseResult;
        }
    }
}
