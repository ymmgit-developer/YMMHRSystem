using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoFlightReservation
    {
        public long FlightReservationId { get; set; }
        public long BusinessTripId { get; set; }
        public string PassengerName { get; set; }
        public string PassportNumber { get; set; }
        public string FlightType { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public DateTime? Departure { get; set; }
        public DateTime? Arrival { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Airline { get; set; }
        public DateTime? FlightDate { get; set; }
        public TimeSpan FlightTime { get; set; }
        public string CompanyPaying { get; set; }
        public string Class { get; set; }
        public string FlightNumber { get; set; }
        public string CostCenter { get; set; }
        public decimal Cost { get; set; }
        public string FlyerMilesCard { get; set; }
    }
}
