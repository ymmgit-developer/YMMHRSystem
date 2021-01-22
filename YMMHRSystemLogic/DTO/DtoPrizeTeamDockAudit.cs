using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoPrizeTeamDockAudit
    {
        public long PrizeTeamDockAuditId { get; set; }
        public long PrizeTeamId { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public int Points { get; set; }
    }
}
