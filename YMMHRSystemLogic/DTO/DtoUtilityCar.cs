using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace YMMHRSystemLogic
{
    public class DtoUtilityCar
    {
        public long UtilityCarId { get; set; }
        public string UtilityCar { get; set; }
        public string LicensePlate { get; set; }
        public string Associate { get; set; }
        public string Destination { get; set; }
        public string Motive { get; set; }
        public string Observations { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public DateTime? LicenseExpiration { get; set; }
        public bool Card { get; set; }
        public int KMDeparture { get; set; }
        public int KMArrival { get; set; }
        public string TankLevelDeparture { get; set; }
        public string TankLevelArrival { get; set; }
        public int Status { get; set; }
        public int Passengers { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
