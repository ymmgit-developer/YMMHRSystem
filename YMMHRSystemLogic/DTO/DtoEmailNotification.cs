using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class DtoEmailNotification
    {
        public long EmailNotificationId { get; set; }
        public string ExtraDinerContacts { get; set; }
        public string ExtraTransportContacts { get; set; }
        public string LegalRequirementContacts { get; set; }
        public string VehicleContacts { get; set; }
        public string UtilityCarContacts { get; set; }
        public string TranslationContacts { get; set; }
        public string CoffeeBreakContacts { get; set; }
        public string BusinessTripContacts { get; set; }
        public string LegalAffairContacts { get; set; }
        public string GiftContacts { get; set; }
    }
}
