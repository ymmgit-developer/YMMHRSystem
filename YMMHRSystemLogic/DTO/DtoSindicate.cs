using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoSindicate
    {
        public long SindicateId { get; set; }
        public string Issue { get; set; }
        public decimal AdministrativeSupport { get; set; }
        public decimal UnionFee { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Date { get; set; }
        public List<DtoSindicateAttachment> AttachmentList { get; set; } = new List<DtoSindicateAttachment>();
    }
}
