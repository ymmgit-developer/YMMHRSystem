using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoGift
    {
        public long GiftId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }

        public List<DtoGiftLog>GiftLogList { get; set; } = new List<DtoGiftLog>();
    }
}
