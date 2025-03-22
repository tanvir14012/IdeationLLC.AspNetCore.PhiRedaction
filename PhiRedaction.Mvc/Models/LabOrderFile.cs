namespace PhiRedaction.Mvc.Models
{
    public partial class LabOrderFile
    {
        [AllowedExtensions(new string[] { "txt","log","rtf" })]
        public IFormFile File { get; set; }
    }
}
