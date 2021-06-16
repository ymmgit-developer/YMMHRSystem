using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoBusinessTrip
    {
        public long BusinessTripId { get; set; }
        public string Associate { get; set; }
        public string Process { get; set; }
        public DateTime DateAdded { get; set; }
        public string UserCreated { get; set; }
        public long CreatedBy { get; set; }
        public int Status { get; set; }
        public List<DtoPassengerDetail> PassengerList { get; set; } = new List<DtoPassengerDetail>();
        public List<DtoTaxiReservation> TaxiList { get; set; } = new List<DtoTaxiReservation>();
        public List<DtoFlightReservation> FlightList { get; set; } = new List<DtoFlightReservation>();
        public List<DtoCarRentalReservation> CarRentalList { get; set; } = new List<DtoCarRentalReservation>();
        public List<DtoHotelReservation> HotelList { get; set; } = new List<DtoHotelReservation>();
        public List<DtoBusinessTripAttachment> AttachmentList { get; set; } = new List<DtoBusinessTripAttachment>();
    }
}
