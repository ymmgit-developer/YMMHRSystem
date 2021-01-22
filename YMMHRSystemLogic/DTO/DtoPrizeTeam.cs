using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoPrizeTeam
    {
        public long PrizeTeamId { get; set; }
        public string Name { get; set; }
        public DateTime DateAdded { get; set; }
        public string CreatedBy { get; set; }
        public int TeamPoints { get; set; }
        public List<DtoPrizeTeamAssociate> AssociateList { get; set; } = new List<DtoPrizeTeamAssociate>();
        public List<DtoPrizeTeamAttendance> AttendanceList { get; set; } = new List<DtoPrizeTeamAttendance>();
        public List<DtoPrizeTeamProductivity> ProductivityList { get; set; } = new List<DtoPrizeTeamProductivity>();
        public List<DtoPrizeTeamDockAudit> DockAuditList { get; set; } = new List<DtoPrizeTeamDockAudit>();
        public List<DtoPrizeTeamScrap> ScrapList { get; set; } = new List<DtoPrizeTeamScrap>();
        public List<DtoPrizeTeamSecurityCross> SecurityCrossList { get; set; } = new List<DtoPrizeTeamSecurityCross>();
    }
}
