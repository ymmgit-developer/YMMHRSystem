using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoImprovements
    {
        public long ImprovementId {  get; set; }
        public string ImprovemenName { get; set; }
        public int ScoreValue { get; set; }
        public string WorkerType { get; set; }
        public long UserCreated {  get; set; }
        public DateTime DateAdded { get; set; }
    }
}
