using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoWorkerFile
    {
        public long WorkerFileId { get; set; }
        public string Names { get; set; }
        public string WorkerId { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public DateTime? DismissalDate { get; set; }
        public string Process { get; set; }
        public DateTime? DoB { get; set; }
        public string CivilStatus { get; set; }
        public string Education { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string WorkerPhoto { get; set; }
        public string Type { get; set; }
        public bool Gender { get; set; }
        public bool Rehirable { get; set; }
        public string UserCreated { get; set; }
        public DateTime? DateAdded { get; set; }
        public bool Status { get; set; }
        public string CURP { get; set; }
        public string RFC { get; set; }
        public string NSS { get; set; }
        public string Job { get; set; }
        public string Route { get; set; }
        public string Shift { get; set; }
        public string Stop { get; set; }
        public int AdmissionNumber { get; set; }
        public int DismissalNumber { get; set; }
        public List<DtoDismissal> DismissalList { get; set; } = new List<DtoDismissal>();
        public List<DtoAdmission> AdmissionList { get; set; } = new List<DtoAdmission>();
        public List<DtoWorkerAttachment> AttachmentList { get; set; } = new List<DtoWorkerAttachment>();
        public List<DtoTrajectory> TrajectoryList { get; set; } = new List<DtoTrajectory>();
        public List<DtoWarning> WarningList { get; set; } = new List<DtoWarning>();
    }
}
