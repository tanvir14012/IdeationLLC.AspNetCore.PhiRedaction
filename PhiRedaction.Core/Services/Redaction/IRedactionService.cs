using PhiRedaction.Core.Models.Redaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhiRedaction.Core.Services.Redaction
{
    public interface IRedactionService
    {
        public Task<RedactionResult> RedactAsync(string inputFilePath);
    }
}
