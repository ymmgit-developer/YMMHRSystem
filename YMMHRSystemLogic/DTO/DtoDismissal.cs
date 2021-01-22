using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace YMMHRSystemLogic
{
    public class DtoDismissal
    {
        public long DismissalId { get; set; }
        public long WorkerFileId { get; set; }
        public string FileName { get; set; }
        public string Motive { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public bool Rehirable { get; set; }
    }
}
