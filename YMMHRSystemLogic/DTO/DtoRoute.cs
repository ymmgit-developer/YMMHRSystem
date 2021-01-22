using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoRoute
    {
        public long RouteId { get; set; }
        public string Name { get; set; }
        public string Shift { get; set; }
        public decimal Cost { get; set; }
        public bool Extraordinary { get; set; }
        public string UserCreated { get; set; }
        public DateTime? DateAdded { get; set; }
        public List<DtoRouteStop> StopList { get; set; } = new List<DtoRouteStop>();
    }
}
