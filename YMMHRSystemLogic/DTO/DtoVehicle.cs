using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace YMMHRSystemLogic
{
    public class DtoVehicle
    {
        public long VehicleId { get; set; }
        public string Model { get; set; }
        public string EngineNumber { get; set; }
        public string VIN { get; set; }
        public string Color { get; set; }
        public string Type { get; set; }
        public string Associate { get; set; }
        public string Year { get; set; }
        public string LicensePlate { get; set; }
        public DateTime? PolicyStart { get; set; }
        public DateTime? PolicyEnd { get; set; }
        public DateTime? NextService { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? AnnualFee { get; set; }
        public DateTime? NextVerification { get; set; }
        public string Status { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
