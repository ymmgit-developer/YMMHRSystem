using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoImmigrationProcedure
    {
        public long ImmigrationProcedureId { get; set; }
        public string Associate { get; set; }
        public string Process { get; set; }
        public string DocumentType { get; set; }
        public string Motive { get; set; }
        public string ProcedureType { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Justification { get; set; }
        public string RejectionMotive { get; set; }
        public DateTime? Date { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
    }
}
