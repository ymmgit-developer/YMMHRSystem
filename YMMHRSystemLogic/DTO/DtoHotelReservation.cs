using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoHotelReservation
    {
        public long HotelReservationId { get; set; }
        public long BusinessTripId { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string Place { get; set; }
        public string CostCenter { get; set; }
        public string Notes { get; set; }
        public string HotelName { get; set; }
        public string ReservationNumber { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string CompanyPaying { get; set; }
        public decimal Cost { get; set; }
    }
}
