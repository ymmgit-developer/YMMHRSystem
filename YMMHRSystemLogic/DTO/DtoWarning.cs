using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoWarning
    {
        public long WarningId { get; set; }
        public long WorkerFileId { get; set; }
        public string FileName { get; set; }
        public string Motive { get; set; }
        public DateTime Date { get; set; }
    }
}
