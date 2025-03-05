using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoDaysAvaible
    {
        [Key] public long WorkerFileId { get; set; }
        public int DaysAvaible { get; set; }
    }
}