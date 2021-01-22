using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoPrizeAssociateAttendance
    {
        public long PrizeAssociateAttendanceId { get; set; }
        public long PrizeAssociateId { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public int Points { get; set; }
    }
}
