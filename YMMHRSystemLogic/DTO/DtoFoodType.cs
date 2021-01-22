using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoFoodType
    {
        public long FoodTypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Cost { get; set; }
        public bool Extraordinary { get; set; }
        public string UserCreated { get; set; }
        public DateTime? DateAdded { get; set; }
    }
}
