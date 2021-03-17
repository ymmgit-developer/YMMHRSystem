using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoBusinessTripAttachment
    {
        public long BusinessTripAttachmentId { get; set; }
        public long BusinessTripId { get; set; }
        public string FileName { get; set; }
    }
}
