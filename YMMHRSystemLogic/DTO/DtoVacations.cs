using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoVacations
    {
        public long VacationId {  get; set; }
        public DateTime DateRequest { get; set; }
        public long WorkerFileId { get; set; }
        public int DaysRequest { get; set; }
        public string Comments { get; set; }
        public bool BossAuthorization { get; set; }
        public bool HrAuthorization { get; set; }
        public int GralStatus { get; set; }
        public bool Blocked { get; set; }

        public static implicit operator List<object>(DtoVacations v)
        {
            throw new NotImplementedException();
        }
    }
}
