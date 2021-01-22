using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoPrizeAssociateMonth
    {
        public long PrizeAssociateMonthId { get; set; }
        public long PrizeAssociateId { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public int Points { get; set; }
    }
}
