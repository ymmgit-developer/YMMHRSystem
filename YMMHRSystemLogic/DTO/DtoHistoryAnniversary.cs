using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoHistoryAnniversary
    {
        public int HistoryAnniversaryId { get; set; }
        public long WorkerFileId { get; set; }
        public DateTime AdmissionDate { get; set; }
        public int AnniversaryNumber { get; set; }
        public int CorrespondingDays { get; set; }
        public DateTime DateAwarded { get; set; }
        public int DaysAvailable { get; set; }
        public bool IsExpired { get; set; }
    }
}
