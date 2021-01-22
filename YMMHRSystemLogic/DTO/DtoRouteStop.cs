using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoRouteStop
    {
        public long RouteStopId { get; set; }
        public long RouteId { get; set; }
        public long StopId { get; set; }
        public string StopName { get; set; }
        public string Reference { get; set; }
        public TimeSpan Time { get; set; }
    }
}
