using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoTransport
    {
        public long TransportId { get; set; }
        public string Route { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public decimal Cost { get; set; }
        public bool Type { get; set; }
        public string UserCreated { get; set; }
    }
}
