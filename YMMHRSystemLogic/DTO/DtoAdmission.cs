using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoAdmission
    {
        public long AdmissionId { get; set; }
        public long WorkerFileId { get; set; }
        public DateTime? Admission { get; set; }
    }
}
