using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoLegalProcedure
    {
        public long LegalProcedureId { get; set; }
        public string Name { get; set; }
        public string Process { get; set; }
        public string UserCreated { get; set; }
        public DateTime? DateAdded { get; set; }
    }
}
