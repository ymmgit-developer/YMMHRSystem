using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class TransportController : Controller
    {
        // GET: YMMHRSystem/Transport
        Transport transport = new Transport();
        User user = new User();

        // GET: YMMHRSystem/Transport
        public ActionResult Transport()
        {
            try
            {
                if (Permission.QueryPermission("TRANSPORT.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveTransport"] = Permission.QueryPermission("TRANSPORT.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanDeleteTransport"] = Permission.QueryPermission("TRANSPORT.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View("~/Areas/YMMHRSystem/Views/Transport/Transport.cshtml", transport.LoadMultiple());
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
        /// Load view with new/existing Transport.
        /// </summary>
        /// <param name="transportId"></param>
        /// <returns></returns>
        public ActionResult LoadTransport(long transportId)
        {
            try
            {
                Session["LoadedTransportId"] = transportId;

                DtoTransport dtoTransport = transport.Load(transportId);

                return View("~/Areas/YMMHRSystem/Views/Transport/TransportDetailDialog.cshtml", dtoTransport);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Transport.
        /// </summary>
        /// <param name="dtoTransport"></param>
        /// <returns></returns>
        public ActionResult SaveTransport(DtoTransport dtoTransport)
        {
            try
            {

                dtoTransport.TransportId = Convert.ToInt64(Session["LoadedTransportId"]);
                if (dtoTransport.TransportId == 0)
                {
                    if (transport.GetTransportId(dtoTransport.Route, dtoTransport.StartDate, dtoTransport.FinishDate) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }

                int totalDays = (dtoTransport.FinishDate.AddDays(1) - dtoTransport.StartDate).Days;

                if (!dtoTransport.Type)
                {
                    dtoTransport.Cost = (dtoTransport.Cost * totalDays) * 2;
                }
                else
                {
                    dtoTransport.Cost = dtoTransport.Cost * totalDays;
                }
                dtoTransport.UserCreated = Session["UserName"].ToString();
                transport.Save(dtoTransport);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a Transport. 
        /// </summary>
        /// <param name="TransportId"></param>
        /// <returns></returns>
        public string DeleteTransport(long TransportId)
        {
            try
            {
                return transport.DeleteTransport(TransportId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // GET: YMMHRSystem/ExtraordinaryTransport
        public ActionResult ExtraordinaryTransport()
        {
            try
            {
                if (Permission.QueryPermission("EXTRATRANSPORT.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveExtraordinaryTransport"] = Permission.QueryPermission("EXTRATRANSPORT.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanCancelExtraordinaryTransport"] = Permission.QueryPermission("EXTRATRANSPORT.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    if (Role.QueryRole("HR", Convert.ToInt64(Session["UserId"])) || Role.QueryRole("Admin", Convert.ToInt64(Session["UserId"])))
                    {
                        return View("~/Areas/YMMHRSystem/Views/Transport/ExtraordinaryTransport.cshtml", transport.LoadMultipleExtra());
                    }
                    else
                    {
                        return View("~/Areas/YMMHRSystem/Views/Transport/ExtraordinaryTransport.cshtml", transport.LoadMultipleExtra(Session["UserName"].ToString()));
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
        /// Load view with new/existing Transport.
        /// </summary>
        /// <param name="extraordinaryTransportId"></param>
        /// <returns></returns>
        public ActionResult LoadExtraordinaryTransport(long extraordinaryTransportId)
        {
            try
            {
                Session["LoadedExtraTransportId"] = extraordinaryTransportId;

                DtoExtraordinaryTransport dtoExtraordinaryTransport = transport.LoadExtra(extraordinaryTransportId);

                return View("~/Areas/YMMHRSystem/Views/Transport/ExtraordinaryTransportDetailDialog.cshtml", dtoExtraordinaryTransport);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Add view Transport.
        /// </summary>
        /// <param name="extraordinaryTransportId"></param>
        /// <returns></returns>
        public ActionResult AddExtraordinaryTransport()
        {
            try
            {
                DtoExtraordinaryTransport dtoExtraordinaryTransport = new DtoExtraordinaryTransport();

                // Obtener todas las sugerencias de correo electrónico
                List<DtoUser> dtoUserListSuggestions = user.GetUserEmailSuggestions();
                dtoExtraordinaryTransport.UserEmailSuggestions = dtoUserListSuggestions;              

                return View("~/Areas/YMMHRSystem/Views/Transport/ExtraordinaryTransportAddDialog.cshtml", dtoExtraordinaryTransport);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Transport.
        /// </summary>
        /// <param name="dtoExtraTransport"></param>
        /// <returns></returns>
        public ActionResult SaveExtraordinaryTransport(DtoExtraordinaryTransport dtoExtraTransport, int TypeTransport, string ListMultiday)
        {
            try
            {
                dtoExtraTransport.ExtraordinaryTransportId = Convert.ToInt64(Session["LoadedExtraTransportId"]);
                if (dtoExtraTransport.ExtraordinaryTransportId == 0)
                {
                    if (dtoExtraTransport.WorkerList.Count <= 0)
                    {
                        return Json(new { success = false, message = "Please select an associate" }, JsonRequestBehavior.AllowGet);
                    }
                }
                if (dtoExtraTransport.FinishDate != null)
                {
                    if (dtoExtraTransport.StartDate != null)
                    {
                        if (dtoExtraTransport.StartDate > dtoExtraTransport.FinishDate)
                        {
                            return Json(new { success = false, message = "Start date cannot be later than finish date" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                if (!Role.QueryRole("HR", Convert.ToInt64(Session["UserId"])) && !Role.QueryRole("Admin", Convert.ToInt64(Session["UserId"])))
                {
                    if (dtoExtraTransport.FinishDate != null)
                    {
                        DateTime finishDateTime = Convert.ToDateTime(dtoExtraTransport.FinishDate + dtoExtraTransport.FinishTime);
                        if (finishDateTime.Subtract(DateTime.Now).TotalHours < 4 || finishDateTime < DateTime.Now)
                        {
                            return Json(new { success = false, message = "Please request transport within 4 hour window" }, JsonRequestBehavior.AllowGet);
                        }

                        if (finishDateTime.Subtract(DateTime.Now).Days == 1)
                        {
                            TimeSpan deadline = TimeSpan.Parse("16:56", CultureInfo.InvariantCulture);
                            if (TimeSpan.Compare(DateTime.Now.TimeOfDay, deadline) == 1)
                            {
                                return Json(new { success = false, message = "Please request next day transport earlier than 4:56pm" }, JsonRequestBehavior.AllowGet);
                            }
                        }

                        if ((finishDateTime.DayOfWeek == DayOfWeek.Saturday || finishDateTime.DayOfWeek == DayOfWeek.Sunday) && DateTime.Now.DayOfWeek == DayOfWeek.Friday)
                        {
                            TimeSpan deadline = TimeSpan.Parse("10:30", CultureInfo.InvariantCulture);
                            if (TimeSpan.Compare(DateTime.Now.TimeOfDay, deadline) == 1)
                            {
                                return Json(new { success = false, message = "Please request Saturday transport earlier than 10:30am Friday" }, JsonRequestBehavior.AllowGet);
                            }
                        }

                        if (finishDateTime.Day == DateTime.Now.Day)
                        {
                            TimeSpan deadline = TimeSpan.Parse("15:30", CultureInfo.InvariantCulture);
                            if (TimeSpan.Compare(DateTime.Now.TimeOfDay, deadline) == 1)
                            {
                                return Json(new { success = false, message = "Please request same day transport earlier than 3:30pm" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }

                    if (dtoExtraTransport.StartDate != null)
                    {
                        DateTime startDateTime = Convert.ToDateTime(dtoExtraTransport.StartDate + dtoExtraTransport.StartTime);
                        if (startDateTime.Subtract(DateTime.Now).TotalHours < 4 || startDateTime < DateTime.Now)
                        {
                            return Json(new { success = false, message = "Please request transport within 4 hour window" }, JsonRequestBehavior.AllowGet);
                        }

                        if (startDateTime.Subtract(DateTime.Now).Days == 1)
                        {
                            TimeSpan deadline = TimeSpan.Parse("16:56", CultureInfo.InvariantCulture);
                            if (TimeSpan.Compare(DateTime.Now.TimeOfDay, deadline) == 1)
                            {
                                return Json(new { success = false, message = "Please request next day transport earlier than 4:56pm" }, JsonRequestBehavior.AllowGet);
                            }
                        }

                        if ((startDateTime.DayOfWeek == DayOfWeek.Saturday || startDateTime.DayOfWeek == DayOfWeek.Sunday) && DateTime.Now.DayOfWeek == DayOfWeek.Friday)
                        {
                            TimeSpan deadline = TimeSpan.Parse("10:30", CultureInfo.InvariantCulture);
                            if (TimeSpan.Compare(DateTime.Now.TimeOfDay, deadline) == 1)
                            {
                                return Json(new { success = false, message = "Please request Saturday transport earlier than 10:30am Friday" }, JsonRequestBehavior.AllowGet);
                            }
                        }

                        if (startDateTime.Day == DateTime.Now.Day)
                        {
                            TimeSpan deadline = TimeSpan.Parse("15:30", CultureInfo.InvariantCulture);
                            if (TimeSpan.Compare(DateTime.Now.TimeOfDay, deadline) == 1)
                            {
                                return Json(new { success = false, message = "Please request same day transport earlier than 3:30pm" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                }

                List<DtoExtraordinaryTransport> transportList = new List<DtoExtraordinaryTransport>();
                WorkerFile workerFile = new WorkerFile();

                if (dtoExtraTransport.WorkerList.Count > 0)
                {
                    foreach (var item in dtoExtraTransport.WorkerList)
                    {
                        if (TypeTransport == 1)
                        {
                            transportList.Add(new DtoExtraordinaryTransport()
                            {
                                AssociateName = item.Names,
                                Process = item.Process,
                                StartDate = dtoExtraTransport.StartDate,
                                StartTime = dtoExtraTransport.StartTime,
                                FinishDate = dtoExtraTransport.FinishDate,
                                FinishTime = dtoExtraTransport.FinishTime,
                                Route = workerFile.GetWorkerFileRoute(item.Names),
                                Stop = workerFile.GetWorkerFileStop(item.Names),
                                UserCreated = Session["UserName"].ToString(),
                                Motive = dtoExtraTransport.Motive,
                                Cost = dtoExtraTransport.Cost,
                                Contacts = dtoExtraTransport.Contacts,
                                ShiftChange = dtoExtraTransport.ShiftChange
                            });
                        }
                        else if (TypeTransport == 2)
                        {
                            DateTime currentDate = dtoExtraTransport.StartDate.Value;
                            while (currentDate <= dtoExtraTransport.FinishDate.Value)
                            {
                                transportList.Add(new DtoExtraordinaryTransport()
                                {
                                    AssociateName = item.Names,
                                    Process = item.Process,
                                    StartDate = currentDate,
                                    StartTime = dtoExtraTransport.StartTime,
                                    FinishDate = currentDate,
                                    FinishTime = dtoExtraTransport.FinishTime,
                                    Route = workerFile.GetWorkerFileRoute(item.Names),
                                    Stop = workerFile.GetWorkerFileStop(item.Names),
                                    UserCreated = Session["UserName"].ToString(),
                                    Motive = dtoExtraTransport.Motive,
                                    Cost = dtoExtraTransport.Cost,
                                    Contacts = dtoExtraTransport.Contacts,
                                    ShiftChange = dtoExtraTransport.ShiftChange
                                });
                                currentDate = currentDate.AddDays(1);
                            }
                        }
                        else if (TypeTransport == 3)
                        {
                            var dates = ListMultiday.Split(',').Select(date => DateTime.Parse(date.Trim()));
                            foreach (var date in dates)
                            {
                                transportList.Add(new DtoExtraordinaryTransport()
                                {
                                    AssociateName = item.Names,
                                    Process = item.Process,
                                    StartDate = date,
                                    StartTime = dtoExtraTransport.StartTime,
                                    FinishDate = date,
                                    FinishTime = dtoExtraTransport.FinishTime,
                                    Route = workerFile.GetWorkerFileRoute(item.Names),
                                    Stop = workerFile.GetWorkerFileStop(item.Names),
                                    UserCreated = Session["UserName"].ToString(),
                                    Motive = dtoExtraTransport.Motive,
                                    Cost = dtoExtraTransport.Cost,
                                    Contacts = dtoExtraTransport.Contacts,
                                    ShiftChange = dtoExtraTransport.ShiftChange
                                });
                            }
                        }
                    }

                    transport.SaveMultipleExtra(transportList, Convert.ToInt64(Session["UserId"]));
                }
                else
                {
                    if (dtoExtraTransport.ExtraordinaryTransportId == 0)
                    {
                        if (transport.GetExtraTransportId(dtoExtraTransport.AssociateName, dtoExtraTransport.StartDate, dtoExtraTransport.FinishDate) != 0)
                        {
                            return Json(new { success = false, message = "Transport already exists" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (dtoExtraTransport.ExtraordinaryTransportId == 0)
                    {
                        dtoExtraTransport.UserCreated = Session["UserName"].ToString();
                    }
                    else
                    {
                        dtoExtraTransport.UserModified = Session["UserName"].ToString();
                    }

                    transport.SaveExtra(dtoExtraTransport);
                }

                return Json(new { success = true, message = "Transport saved successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="extraordinaryTransportId"></param>
        /// <returns></returns>
        public ActionResult ConfirmExtraordinaryTransport(long extraordinaryTransportId)
        {
            Session["LoadedExtraTransportId"] = extraordinaryTransportId;

            transport.ConfirmExtraordinaryTransport(extraordinaryTransportId, Convert.ToInt64(Session["UserId"]));

            return RedirectToAction("ExtraordinaryTransport");
        }
        /// <summary>
        /// Removes a Transport. 
        /// </summary>
        /// <param name="extraordinaryTransportId"></param>
        /// <returns></returns>
        public string CancelExtraordinaryTransport(long extraordinaryTransportId)
        {
            try
            {
                return transport.CancelExtraTransport(extraordinaryTransportId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}