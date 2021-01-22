using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
     public class DtoRole
    {
        #region Propiedades

        public long RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> UserList { get; set; } = new List<string>();
         #endregion
    }
}
