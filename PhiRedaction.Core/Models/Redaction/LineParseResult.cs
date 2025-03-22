namespace PhiRedaction.Core.Models.Redaction
{
    public class LineParseResult
    {
        public bool IsField { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public string Delimiter { get; set; }
        public bool IsMultilineField { get; set; }
    }
}
