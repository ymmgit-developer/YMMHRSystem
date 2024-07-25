using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoExtraordinaryTransport
    {
        public long ExtraordinaryTransportId { get; set; }
        public string AssociateName { get; set; }
        public string Process { get; set; }
        public string Route { get; set; }
        public string Stop { get; set; }
        public TimeSpan StartTime { get; set; }
        public DateTime? StartDate { get; set; }
        public TimeSpan FinishTime { get; set; }
        public DateTime? FinishDate { get; set; }
        public string Motive { get; set; }
        public decimal Cost { get; set; }
        public string Contacts { get; set; }
        public string UserCreated { get; set; }
        public DateTime Date { get; set; }
        public string UserModified { get; set; }
        public int Status { get; set; }
        public long CreatedBy{ get; set; }
        public List<DtoWorkerFile> WorkerList { get; set; } = new List<DtoWorkerFile>();
        public bool ShiftChange { get; set; }
        public List<DtoUser> UserEmailSuggestions { get; set; } = new List<DtoUser> { };
    }
}
