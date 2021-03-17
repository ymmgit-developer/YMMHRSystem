using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoTaxiReservation
    {
        public long TaxiReservationId { get; set; }
        public long BusinessTripId { get; set; }
        public string QuoteNumber { get; set; }
        public DateTime? PickupDate { get; set; }
        public TimeSpan PickupTime { get; set; }
        public string PickupPlace { get; set; }
        public string Destination { get; set; }
        public string FlightNumber { get; set; }
        public string CompanyPaying { get; set; }
        public string CostCenter { get; set; }
        public string LicensePlate { get; set; }
        public string DriverName { get; set; }
        public decimal Cost { get; set; }
    }
}
