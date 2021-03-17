using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoCarRentalReservation
    {
        public long CarRentalReservationId { get; set; }
        public long BusinessTripId { get; set; }
        public string DriverName { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime? PickupDate { get; set; }
        public TimeSpan PickupTime { get; set; }
        public string PickupPlace { get; set; }
        public string ReturnPlace { get; set; }
        public DateTime? ReturnDate { get; set; }
        public TimeSpan ReturnTime { get; set; }
        public string ReservationNumber { get; set; }
        public string CarType { get; set; }
        public string CompanyPaying { get; set; }
        public string CostCenter { get; set; }
        public decimal Cost { get; set; }

    }
}
