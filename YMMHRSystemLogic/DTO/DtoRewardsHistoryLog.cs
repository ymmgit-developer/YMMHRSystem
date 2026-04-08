using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoRewardsHistoryLog
    {
        public int TransactionType { get; set; }
        public string ImprovemenName { get; set; } = string.Empty;
        public string RedeemName { get; set; } = string.Empty;
        public int ScoreBefore { get; set; }
        public int ScoreDelta { get; set; }
        public int ScoreAfter { get; set; }
        public string TransactionDate { get; set; } = string.Empty;
        public string TransactionName { get;set; } = string.Empty;
    }
}
