using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoPrizeTeamPointLog
    {
        public long PrizeTeamPointLogId { get; set; }
        public long PrizeTeamId { get; set; }
        public string MovementType { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
    }
}
