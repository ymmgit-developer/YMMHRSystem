using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoLegalRequirement
    {
        public long LegalRequirementId { get; set; }
        public string LegalProcedure { get; set; }
        public string Institution { get; set; }
        public DateTime? EmissionDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? RenewalDate { get; set; }
        public string UserCreated { get; set; }
        public DateTime? DateAdded { get; set; }
        public List<DtoLegalRequirementAttachment> AttachmentList { get; set; } = new List<DtoLegalRequirementAttachment>();
    }
}
