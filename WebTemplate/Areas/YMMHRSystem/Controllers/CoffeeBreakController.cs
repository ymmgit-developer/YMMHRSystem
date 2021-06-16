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
    public class CoffeeBreakController : Controller
    {
        CoffeeBreak coffeeBreak = new CoffeeBreak();
        SendEmail sendEmail = new SendEmail();
        EmailNotification emailNotification = new EmailNotification();

        // GET: YMMHRSystem/CoffeeBreak
        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("COFFEEBREAK.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveCoffeeBreak"] = Permission.QueryPermission("COFFEEBREAK.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanDeleteCoffeeBreak"] = Permission.QueryPermission("COFFEEBREAK.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    if (Role.QueryRole("GA", Convert.ToInt64(Session["UserId"])) || Role.QueryRole("Admin", Convert.ToInt64(Session["UserId"])))
                    {
                        return View("~/Areas/YMMHRSystem/Views/CoffeeBreak/Index.cshtml", coffeeBreak.LoadMultiple());
                    }
                    else
                    {
                        return View("~/Areas/YMMHRSystem/Views/CoffeeBreak/Index.cshtml", coffeeBreak.LoadMultiple(Session["UserName"].ToString()));
                    }
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
        /// Load view with new/exisiting CoffeeBreak.
        /// </summary>
        /// <param name="coffeeBreakId"></param>
        /// <returns></returns>
        public ActionResult LoadCoffeeBreak(long coffeeBreakId)
        {
            try
            {
                Session["LoadedCoffeeBreakId"] = coffeeBreakId;

                DtoCoffeeBreak dtoCoffeeBreak = coffeeBreak.Load(coffeeBreakId);

                return View("~/Areas/YMMHRSystem/Views/CoffeeBreak/CoffeeBreakDetailDialog.cshtml", dtoCoffeeBreak);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a CoffeeBreak.
        /// </summary>
        /// <param name="dtoCoffeeBreak"></param>
        /// <returns></returns>
        public ActionResult SaveCoffeeBreak(DtoCoffeeBreak dtoCoffeeBreak)
        {

            try
            {
                if (coffeeBreak.LoadMultipleAvailableRooms(dtoCoffeeBreak.StartDate).Count == 0)
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }
                if (dtoCoffeeBreak.FinishDate < dtoCoffeeBreak.StartDate)
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }
                dtoCoffeeBreak.CoffeeBreakId = Convert.ToInt64(Session["LoadedCoffeeBreakId"]);
                dtoCoffeeBreak.Responsable = Session["UserName"].ToString();
                coffeeBreak.Save(dtoCoffeeBreak, Convert.ToInt64(Session["UserId"]));
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a CoffeeBreak. 
        /// </summary>
        /// <param name="coffeeBreakId"></param>
        /// <returns></returns>
        public string DeleteCoffeeBreak(long coffeeBreakId)
        {
            try
            {
                return coffeeBreak.DeleteCoffeeBreak(coffeeBreakId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Starts a coffeeBreak
        /// </summary>
        /// <returns></returns>
        public ActionResult StartCoffeeBreak(DtoCoffeeBreak dtoCoffeeBreak)
        {
            try
            {
                dtoCoffeeBreak.Status = 2;
                coffeeBreak.Save(dtoCoffeeBreak);

                User user = new User();
                List<string> contacts = emailNotification.GetCoffeeBreakContacts(1).Split(',').ToList();
                contacts.Add(user.GetUserEmail(dtoCoffeeBreak.CreatedBy));
                sendEmail.SendEmailTemplate("YMM HR System: Coffee Break Approval", "TemplateCoffeeBreakStart", new[,]
                    {
                        {"$APPLICANT$", dtoCoffeeBreak.Responsable},
                        {"$TYPE$", dtoCoffeeBreak.Type},
                        {"$ROOM$", dtoCoffeeBreak.Room},
                        {"$DATE$", dtoCoffeeBreak.StartDate?.ToString("dd/MM/yyyy")}
                    }, sendEmail.GetAdminEmail(), contacts);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Ends a CoffeeBreak.
        /// </summary>
        /// <returns></returns>
        public ActionResult EndCoffeeBreak(DtoCoffeeBreak dtoCoffeeBreak)
        {
            try
            {
                User user = new User();
                dtoCoffeeBreak.Status = 3;
                coffeeBreak.Save(dtoCoffeeBreak);

                List<string> contacts = emailNotification.GetCoffeeBreakContacts(1).Split(',').ToList();
                contacts.Add(user.GetUserEmail(dtoCoffeeBreak.CreatedBy));

                sendEmail.SendEmailTemplate("YMM HR System: Coffee Break Complete", "TemplateCoffeeBreakFinish", new[,]
{
                        {"$APPLICANT$", dtoCoffeeBreak.Responsable},
                        {"$TYPE$", dtoCoffeeBreak.Type},
                        {"$ROOM$", dtoCoffeeBreak.Room},
                        {"$DATE$", dtoCoffeeBreak.StartDate?.ToString("dd/MM/yyyy")}
                    }, sendEmail.GetAdminEmail(), contacts);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public ActionResult GetAvailableRooms(DateTime date)
        {
            try
            {
                var roomList = coffeeBreak.LoadMultipleAvailableRooms(date);

                return Json(roomList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
