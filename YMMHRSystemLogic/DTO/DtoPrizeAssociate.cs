using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoPrizeAssociate
    {
        public long PrizeAssociateId { get; set; }
        public long WorkerFileId { get; set; }
        public string Associate { get; set; }
        public string Department { get; set; }
        public string Team { get; set; }
        public int AssociatePoints { get; set; }
        public int TeamPoints { get; set; }
        public List<DtoPrizeAssociateAttendance> AttendanceList { get; set; } = new List<DtoPrizeAssociateAttendance>();
        public List<DtoPrizeAssociateFinding> FindingList { get; set; } = new List<DtoPrizeAssociateFinding>();
        public List<DtoPrizeAssociateMonth> AssociateMonthList { get; set; } = new List<DtoPrizeAssociateMonth>();
        public List<DtoPrizeAssociateYear> AssociateYearList { get; set; } = new List<DtoPrizeAssociateYear>();
        public List<DtoPrizeAssociatePrize> PrizeList { get; set; } = new List<DtoPrizeAssociatePrize>();
    }
}
