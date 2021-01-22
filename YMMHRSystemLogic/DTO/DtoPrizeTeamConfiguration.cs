using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoPrizeTeamConfiguration
    {
        public long PrizeTeamConfigurationId { get; set; }
        public int MonthlyAttendance { get; set; }
        public int Productivity { get; set; }
        public int SecurityCross { get; set; }
        public int Scrap { get; set; }
        public int DockAudit { get; set; }
    }
}
