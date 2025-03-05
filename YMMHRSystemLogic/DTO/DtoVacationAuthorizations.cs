using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoVacationAuthorizations
    {
        [Key]  public long VacationAuthorizationsId {  get; set; }
        public long WorkerFileId { get; set; }
        public long UserId { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateModification { get; set; }
    }
}
