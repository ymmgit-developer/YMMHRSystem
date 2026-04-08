using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class EmailNotificationController : Controller
    {
        EmailNotification emailNotification = new EmailNotification();

        // GET: YMMHRSystem/EmailNotification
        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("EMAIL.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveEmailNotification"] = Permission.QueryPermission("EMAIL.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View("~/Areas/YMMHRSystem/Views/EmailNotification/Index.cshtml", emailNotification.Load(1));
                }
                else
                {
                    return View("~/Views/Shared/AccessDenied.cshtml");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contacts"></param>
        /// <returns></returns>
        public string SaveExtraDinerContacts(string contacts)
        {
            try
            {
                return emailNotification.SaveExtraDinerContacts(contacts, 1) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contacts"></param>
        /// <returns></returns>
        public string SaveExtraTransportContacts(string contacts)
        {
            try
            {
                return emailNotification.SaveExtraTransportContacts(contacts, 1) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contacts"></param>
        /// <returns></returns>
        public string SaveLegalRequirementContacts(string contacts)
        {
            try
            {
                return emailNotification.SaveLegalRequirementsContacts(contacts, 1) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contacts"></param>
        /// <returns></returns>
        public string SaveVehicleContacts(string contacts)
        {
            try
            {
                return emailNotification.SaveVehicleContacts(contacts, 1) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contacts"></param>
        /// <returns></returns>
        public string SaveUtilityCarContacts(string contacts)
        {
            try
            {
                return emailNotification.SaveUtilityCarContacts(contacts, 1) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contacts"></param>
        /// <returns></returns>
        public string SaveTranslationContacts(string contacts)
        {
            try
            {
                return emailNotification.SaveTranslationContacts(contacts, 1) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contacts"></param>
        /// <returns></returns>
        public string SaveCoffeeBreakContacts(string contacts)
        {
            try
            {
                return emailNotification.SaveCoffeeBreakContacts(contacts, 1) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contacts"></param>
        /// <returns></returns>
        public string SaveBusinessTripContacts(string contacts)
        {
            try
            {
                return emailNotification.SaveBusinessTripContacts(contacts, 1) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contacts"></param>
        /// <returns></returns>
        public string SaveLegalAffairContacts(string contacts)
        {
            try
            {
                return emailNotification.SaveLegalAffairContacts(contacts, 1) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contacts"></param>
        /// <returns></returns>
        public string SaveGiftContacts(string contacts)
        {
            try
            {
                return emailNotification.SaveGiftContacts(contacts, 1) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Send Anniversarty Gift Email
        /// </summary>
        /// 
        /// <summary>
        /// Send Anniversarty Gift Email
        /// </summary>
        public string SaveRewardsInvenContacts(string contacts)
        {
            try
            {
                return emailNotification.SaveRewardInventoryContacts(contacts) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
