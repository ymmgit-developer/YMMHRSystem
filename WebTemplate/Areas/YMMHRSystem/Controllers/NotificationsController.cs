using Newtonsoft.Json.Converters;
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
    public class NotificationsController : Controller
    {
        FirstNotificationEntryExit firstNotificationEntryExit = new FirstNotificationEntryExit();
        SecondNotificationEntryExit secondNotificationEntryExit = new SecondNotificationEntryExit();
        User users = new User();
        public ActionResult Notifications()
        {
            try
            {
                if (Permission.QueryPermission("NOTIFICATIONSINOUT.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveNotificationsInOut"] = Permission.QueryPermission("NOTIFICATIONSINOUT.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanDeleteNotificationsInOut"] = Permission.QueryPermission("NOTIFICATIONSINOUT.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    ViewBag.Users = users.LoadMultiple();
                    ViewBag.FirstNotifications = firstNotificationEntryExit.GetRecords();
                    ViewBag.SecondNotifications = secondNotificationEntryExit.GetRecords();
                    return View("~/Areas/YMMHRSystem/Views/Notifications/Notifications.cshtml");
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

        public ActionResult AddFirstApprover()
        {
            try
            {
                return PartialView("~/Areas/YMMHRSystem/Views/Notifications/FirstApprover.cshtml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult AddSecondApprover()
        {
            {
                try
                {
                    return PartialView("~/Areas/YMMHRSystem/Views/Notifications/SecondApprover.cshtml");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public ActionResult SaveFirstApprover(List<string>checkUserId, List<DtoWorkerFile>WorkerList) 
        {
            try
            {
                User users = new User();
                WorkerFile workerFile = new WorkerFile();

                if (checkUserId == null || !checkUserId.Any())
                    return Json(new { success = false, message = "No users selected for notification." });

                if (WorkerList == null || !WorkerList.Any())
                    return Json(new { success = false, message = "No workers provided for notification." });

                FirstNotificationEntryExit notifications = new FirstNotificationEntryExit();
                string currentUser = HttpContext.Session["UserName"].ToString();
                DateTime currentDate = DateTime.Now;

                foreach (var worker in WorkerList)
                {
                    foreach (var userId in checkUserId)
                    {
                        // Obtener el email del usuario a notificar (simulación de la función)
                        string emailToNotify = users.GetUserEmail(long.Parse(userId));
                        string workerFileId = workerFile.GetWorkerFileIdByName(worker.Names);
                        if (string.IsNullOrEmpty(emailToNotify))
                            continue; // Si no hay email, omitir este registro

                        // Crear una nueva instancia de la tabla
                        var notification = new DtoFirstNotificationEntryExit
                        {
                            IdApplicant = workerFileId,
                            NamesApplicant = worker.Names,
                            IdUserToNotify = userId,
                            EmailToNotify = emailToNotify,
                            UserCreated = currentUser,
                            DateAdded = currentDate
                        };

                        notifications.Save(notification);
                    }
                }
                return Json(new { success = true, message = "Notifications successfully saved." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        public ActionResult DeleteFirstApprover(string IdNotification)
        {
            try
            {
                FirstNotificationEntryExit firstNotificationEntryExit = new FirstNotificationEntryExit();
                if (firstNotificationEntryExit.Delete(long.Parse(IdNotification)))
                {
                    return Json(new { success = true, message = "Notifications successfully delete" });
                }
                else
                {
                    return Json(new { success = false, message = "An error occurred"});
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        public ActionResult SaveSecondApprover(List<string> checkUserId)
        {
            try
            {
                User user = new User();
                SecondNotificationEntryExit secondNotificationEntryExit = new SecondNotificationEntryExit();
                foreach (string userId in checkUserId)
                {
                    var email = user.GetUserEmail(long.Parse(userId));
                    var name = user.GetUserName(userId);
                    string currentUser = HttpContext.Session["UserName"].ToString();
                    DateTime currentDate = DateTime.Now;
                    var secondnotification = new DtoSecondNotificationEntryExit()
                    {
                        IdApplicant = "IdApplicant",
                        NamesApplicant = "1",
                        IdUserToNotify = name,
                        EmailToNotify = email,
                        UserCreated = currentUser,
                        DateAdded = currentDate,
                    };
                    secondNotificationEntryExit.Save(secondnotification);
                }
                return Json(new { success = true, message = "Notifications successfully saved." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        public ActionResult DeleteSecondApprover(string IdNotification)
        {
            try
            {
                SecondNotificationEntryExit secondNotificationEntryExit = new SecondNotificationEntryExit();
                if (secondNotificationEntryExit.Delete(long.Parse(IdNotification)))
                {
                    return Json(new { success = true, message = "Notifications successfully delete" });
                }
                else
                {
                    return Json(new { success = false, message = "An error occurred"});
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }
    }
}