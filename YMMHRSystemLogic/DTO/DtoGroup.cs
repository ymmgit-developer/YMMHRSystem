using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic.DTO
{
    public class DtoGroup
    {
        public long GroupId { get; set; }
        public string GroupName { get; set; }
        public Boolean IsActive { get; set; } = true;
    }
}
