using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class UtilityCarController : Controller
    {
        UtilityCar utilityCar = new UtilityCar();
        SendEmail sendEmail = new SendEmail();
        EmailNotification emailNotification = new EmailNotification();

        // GET: YMMHRSystem/UtilityCar
        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("UTILITYCAR.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveUtilityCar"] = Permission.QueryPermission("UTILITYCAR.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanDeleteUtilityCar"] = Permission.QueryPermission("UTILITYCAR.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View("~/Areas/YMMHRSystem/Views/UtilityCar/Index.cshtml", utilityCar.LoadMultiple());
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
        /// Load view with new/exisiting UtilityCar.
        /// </summary>
        /// <param name="nMR4z"></param>
        /// <returns></returns>
        public ActionResult LoadUtilityCar(long nMR4z)
        {
            try
            {
                Session["LoadedUtilityCarId"] = nMR4z;

                DtoUtilityCar dtoUtilityCar = utilityCar.Load(nMR4z);

                return View("~/Areas/YMMHRSystem/Views/UtilityCar/UtilityCarDetail.cshtml", dtoUtilityCar);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a UtilityCar.
        /// </summary>
        /// <param name="dtoUtilityCar"></param>
        /// <returns></returns>
        public ActionResult SaveUtilityCar(DtoUtilityCar dtoUtilityCar)
        {

            try
            {
                dtoUtilityCar.UtilityCarId = Convert.ToInt64(Session["LoadedUtilityCarId"]);
                utilityCar.Save(dtoUtilityCar);
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a UtilityCar. 
        /// </summary>
        /// <param name="utilityCarId"></param>
        /// <returns></returns>
        public string DeleteUtilityCar(long utilityCarId)
        {
            try
            {
                return utilityCar.DeleteUtilityCar(utilityCarId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Starts a utilityCar
        /// </summary>
        /// <returns></returns>
        public ActionResult StartUtilityCar(DtoUtilityCar dtoUtilityCar)
        {
            try
            {
                dtoUtilityCar.Status = 2;
                utilityCar.Save(dtoUtilityCar);
                sendEmail.SendEmailTemplate("YMM HR System: Utility Car Approval", "TemplateUtilityCarStart", new[,]
                    {
                        {"$APPLICANT$", dtoUtilityCar.Associate},
                        {"$PASSENGERS$", dtoUtilityCar.Passengers.ToString()},
                        {"$DESTINATION$", dtoUtilityCar.Destination },
                        {"$DATE$", dtoUtilityCar.DepartureDate?.ToString("dd/MM/yyyy")}
                    }, sendEmail.GetAdminEmail(), emailNotification.GetUtilityCarContacts(1).Split(',').ToList());

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Ends a UtilityCar.
        /// </summary>
        /// <returns></returns>
        public ActionResult EndUtilityCar(DtoUtilityCar dtoUtilityCar)
        {
            try
            {
                dtoUtilityCar.Status = 3;
                utilityCar.Save(dtoUtilityCar);
                sendEmail.SendEmailTemplate("YMM HR System: Utility Car Complete", "TemplateUtilityCarFinish", new[,]
     {
                        {"$APPLICANT$", dtoUtilityCar.Associate},
                        {"$PASSENGERS$", dtoUtilityCar.Passengers.ToString()},
                        {"$DESTINATION$", dtoUtilityCar.Destination },
                        {"$DATE$", dtoUtilityCar.DepartureDate?.ToString("dd/MM/yyyy")}
                    }, sendEmail.GetAdminEmail(), emailNotification.GetUtilityCarContacts(1).Split(',').ToList());
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Loads car list
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns></returns>
        public ActionResult LoadVehicles(DateTime date)
        {
            try
            {
                var carList = utilityCar.LoadMultipleAvailableCars(date);

                return Json(carList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
