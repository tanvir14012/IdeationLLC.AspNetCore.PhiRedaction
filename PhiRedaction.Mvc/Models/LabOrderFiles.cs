using Microsoft.AspNetCore.Mvc;
using PhiRedaction.Core.Models.Redaction;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhiRedaction.Mvc.Models
{
    public partial class LabOrderFiles
    {
        [BindProperty]
        public IList<LabOrderFile> Files { get; set; }

        [NotMapped]
        public IList<RedactionResult> Results { get; set; }
    }
}
