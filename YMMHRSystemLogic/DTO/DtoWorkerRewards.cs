using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoWorkerRewards
    {
        [Key]
        public long WorkerFileId { get; set; }
        public string Names { get; set; } = string.Empty;
        public string WorkerId { get; set; } = string.Empty;
        public string Process { get; set; } = string.Empty;
        public string Job { get; set; } = string.Empty;
        public long? GroupId { get; set; }
        public string GroupName { get; set; } = "";
        public int Score { get; set; }
    }
}
