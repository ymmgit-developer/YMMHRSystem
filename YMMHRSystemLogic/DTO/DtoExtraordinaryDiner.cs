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
        public string Process { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Type { get; set; }
        public string Motive { get; set; }
        public decimal Cost { get; set; }
        public bool Lading { get; set; }
        public decimal LadingCost { get; set; }
        public string UserCreated { get; set; }
    }
}
