using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoLegalRequirementAttachment
    {
        public long LegalRequirementAttachmentId { get; set; }
        public long LegalRequirementId { get; set; }
        public string FileName { get; set; }
    }
}
