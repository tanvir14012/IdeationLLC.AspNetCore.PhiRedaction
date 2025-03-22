namespace PhiRedaction.Core.Models.Settings
{
    public class LabOrderParseSettings
    {
        public LabOrderParseSettings()
        {
        }
        public string[] FieldDelimiters { get; set; }
        public string ProcessDirectoryRelative { get; set; }
        public string ProcessedIndicatorSuffix { get; set; }
        public int MaxLineSizeInBytes { get; set; }
        public RedactionPattern[] RedactionPatterns { get; set; }
    }

    public class RedactionPattern
    {
        public string Pattern { get; set; }
        public string Replacement { get; set; }
    }
}
