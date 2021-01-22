using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoGiftAssociate
    {
        public long GiftAssociateId { get; set; }
        public long WorkerFileId { get; set; }
        public string Associate { get; set; }
        public string Process { get; set; }
        public string Month { get; set; }
        public int Day { get; set; }
    }
}
