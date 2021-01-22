using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoPrizeLog
    {
        public long PrizeLogId { get; set; }
        public long PrizeId { get; set; }
        public string MovementType { get; set; }
        public string Associate { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
    }
}
