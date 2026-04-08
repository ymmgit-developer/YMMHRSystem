using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoRewardsItem
    {
        public long ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int ValuePoints { get; set; }
        public int Stock { get; set; }
        public decimal Cost { get; set; }
        public int StockMin { get; set; }
        public int StockMax { get; set; }
        public bool IsActive { get; set; }
    }
}
