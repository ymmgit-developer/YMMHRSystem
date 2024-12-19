using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoVacationDayUsage
    {
        public int VacationDayUsageId { get; set; }
        public long VacationId { get; set; }
        public int HistoryAnniversaryId { get; set; }
        public int DaysUsed { get; set; }
    }
}
