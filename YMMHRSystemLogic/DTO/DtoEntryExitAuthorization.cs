using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoEntryExitAuthorization
    {
        public long IdRecordsInOut { get; set; }
        public List<DtoWorkerFile> WorkerList { get; set; } = new List<DtoWorkerFile>();
        public string Associate {  get; set; }
        public string WorkerId { get; set; }
        public string Process { get; set; }
        public DateTime DateFor { get; set; }
        public TimeSpan TimeFor { get; set; }
        public long? IdType { get; set; }
        public long? IdSalary { get; set; }
        public long? IdMotive { get; set; }
        public bool? FirtsAuthorization { get; set; }
        public string FirtsAuthorizedby { get; set; }
        public bool? SecondAuthorization { get; set; }
        public string SecondAuthorizedby { get; set; }
        public int? CurrentState { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}

