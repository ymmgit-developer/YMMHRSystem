using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoPrizeAssociatePointLog
    {
        public long PrizeAssociatePointLogId { get; set; }
        public long PrizeAssociateId { get; set; }
        public string MovementType { get; set; }
        public string Associate { get; set; }
        public string Motive { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
    }
}
