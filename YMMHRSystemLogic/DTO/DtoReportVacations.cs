using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoReportVacations
    {
        [Key] public long VacationId { get; set; }
        public DateTime DateRequest { get; set; }
        public string Names { get; set; }
        public int DaysRequest { get; set; }
        public string Comments { get; set; }
        public int GralStatus { get; set; }
        public DateTime Date { get; set; }
    }
}
