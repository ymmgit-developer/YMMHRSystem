using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoPrizeAssociatePrize
    {
        public long PrizeAssociatePrizeId { get; set; }
        public long PrizeAssociateId { get; set; }
        public long PrizeLogId { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public long PrizeId { get; set; }
        public string Prize { get; set; }
        public int Points { get; set; }
        public DateTime Date { get; set; }
    }
}
