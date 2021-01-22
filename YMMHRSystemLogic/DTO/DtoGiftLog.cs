using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoGiftLog
    {
        public long GiftLogId { get; set; }
        public long GiftId { get; set; }
        public string Gift { get; set; }
        public string MovementType { get; set; }
        public string Associate { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
    }
}
