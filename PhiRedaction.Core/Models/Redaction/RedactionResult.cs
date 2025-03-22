namespace PhiRedaction.Core.Models.Redaction
{
    public class RedactionResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string OutputFilePath { get; set; }
        public string InputFilePath { get; set; }
    }
}
