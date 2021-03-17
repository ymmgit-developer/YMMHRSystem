using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoRoom
    {
        public long RoomId { get; set; }
        public string Name { get; set; }
        public string UserCreated { get; set; }
        public DateTime? DateAdded { get; set; }
    }
}
