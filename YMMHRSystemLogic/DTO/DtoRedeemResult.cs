using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoRedeemResult
    {
        public int Success { get; set; }          // 1 ok, 0 error
        public string Message { get; set; } = "";
        public long RedeemId { get; set; }
        public int TotalPoints { get; set; }
        public int NewBalance { get; set; }
        
    }
}
