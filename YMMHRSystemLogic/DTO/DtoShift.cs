using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoShift
    {
        public long ShiftId { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string UserCreated { get; set; }
        public DateTime? DateAdded { get; set; }
    }
}
