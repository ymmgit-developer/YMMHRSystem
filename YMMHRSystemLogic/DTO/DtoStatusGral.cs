using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoStatusGral
    {
        [Key] public long VacationId { get; set; }
        public long WorkerFileId { get; set; }
        public int GralStatus { get; set; }
    }
}
