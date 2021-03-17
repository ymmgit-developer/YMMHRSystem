using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoTranslationAttachment
    {
        public long TranslationAttachmentId { get; set; }
        public long TranslationId { get; set; }
        public string FileName { get; set; }
        public bool IsTranslated { get; set; }
    }
}
