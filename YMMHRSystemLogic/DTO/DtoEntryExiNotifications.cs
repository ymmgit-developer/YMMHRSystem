using EmailComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoEntryExiNotifications
    {
        public long IdNotification { get; set; }
        public string IdApplicant { get; set; }
        public string NamesApplicant { get; set; }
        public string IdUserToNotify { get; set; }
        public string EmailToNotify { get; set; }
        public string UserCreated { get; set; }
        public DateTime? DateAdded { get; set; }
    }

}
