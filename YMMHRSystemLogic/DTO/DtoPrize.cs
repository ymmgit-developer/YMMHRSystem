using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoPrize
    {
        public long PrizeId { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public int Minimum { get; set; }
        public bool Status { get; set; }
        public long CreatedBy { get; set; }

        public List<DtoPrizeLog> PrizeLogList { get; set; } = new List<DtoPrizeLog>();
    }
}
