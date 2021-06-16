using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoUser
    {
        #region Properties

        public long UserId { get; set; }
        public List<DtoPermission> PermissionList { get; set; } = new List<DtoPermission>();
        public List<DtoRole> RoleList { get; set; } = new List<DtoRole>();
        public string Name { get; set; }
        public string FirstSurname { get; set; }
        public string LastSurname { get; set; }
        public string Email { get; set; }
        public string WorkerId { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool FirstLogIn { get; set; }
        public string Process { get; set; }

        #endregion
    }
}
