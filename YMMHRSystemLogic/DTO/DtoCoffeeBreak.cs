using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace YMMHRSystemLogic
{
    public class DtoCoffeeBreak
    {
        public long CoffeeBreakId { get; set; }
        public int Status { get; set; }
        public int AttendeeQuantity { get; set; }
        public int InstructorQuantity { get; set; }
        public string Responsable { get; set; }
        public string Room { get; set; }
        public string Type { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan FinishTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime DateAdded { get; set; }
        public long CreatedBy { get; set; }
    }
}
