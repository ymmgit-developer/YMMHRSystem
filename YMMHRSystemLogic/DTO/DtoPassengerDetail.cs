using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoPassengerDetail
    {
        public long PassengerDetailId { get; set; }
        public long BusinessTripId { get; set; }
        public string Passenger { get; set; }
        public string CompanyName { get; set; }
        public string Purpose { get; set; }
        public string Type { get; set; }
        public string Notes { get; set; }       
    }
}
