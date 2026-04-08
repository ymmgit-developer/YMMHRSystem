using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic.DTO
{
    public class DtoLowStockRow
    {
        public long ItemId { get; set; }
        public string ItemName { get; set; } = "";
        public int Stock { get; set; }
        public int StockMin { get; set; }
        public int RedeemedQty { get; set; }
    }
}
