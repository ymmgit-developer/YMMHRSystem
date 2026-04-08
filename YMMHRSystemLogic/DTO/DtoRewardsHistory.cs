using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoRewardsHistory
    {
        [Key] public long RewardsHistoryId { get; set; }
        public string WorkerId { get; set; } = "";
        public int TransactionType { get; set; }
        public long? ImprovementId { get; set; }
        public long? RedeemId { get; set; }
        [Required] public int ScoreBefore { get; set; }
        [Required] public int ScoreDelta { get; set; }
        [Required] public int ScoreAfter { get; set; }
        [Required] public DateTime TransactionDate { get; set; }
        [Required] public long UserCreated { get; set; }
    }
}
