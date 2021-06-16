using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoExtraordinaryDiner
    {
        public long ExtraordinaryDinerId { get; set; }
        public string AssociateName { get; set; }
        public string Contacts { get; set; }
        public string Process { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? FinishDate { get; set; }
        public TimeSpan Time { get; set; }
        public string Type { get; set; }
        public string Motive { get; set; }
        public decimal TypeCost { get; set; }
        public bool Lading { get; set; }
        public decimal LadingCost { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }
        public int Status { get; set; }
        public long CreatedBy { get; set; }
        public List<DtoWorkerFile> WorkerList { get; set; } = new List<DtoWorkerFile>();
        public int GuestQuantity { get; set; }
    }
}
