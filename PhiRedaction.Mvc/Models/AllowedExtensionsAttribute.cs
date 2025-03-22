using System.ComponentModel.DataAnnotations;

namespace PhiRedaction.Mvc.Models
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] extensions;

        public AllowedExtensionsAttribute(string[] extensions)
        {
            this.extensions = extensions;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            var extension = "";
            if (file != null)
            {
                extension = Path.GetExtension(file.FileName).Replace(".", "");
            }
            if (file != null && !this.extensions.Contains(extension.ToLower()))
            {
                return new ValidationResult($"Lab order file must end with one of the following extensions [{string.Join(", ", this.extensions)}]");
            }
            return ValidationResult.Success;
        }
    }
}
