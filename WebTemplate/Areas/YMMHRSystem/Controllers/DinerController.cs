using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class DinerController : Controller
    {
        // GET: YMMHRSystem/Diner
        Diner diner = new Diner();
        User user = new User();

        public ActionResult Diner()
        {
            try
            {
                if (Permission.QueryPermission("DINER.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveDiner"] = Permission.QueryPermission("DINER.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanDeleteDiner"] = Permission.QueryPermission("DINER.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View("~/Areas/YMMHRSystem/Views/Diner/Diner.cshtml", diner.LoadMultiple());
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
        /// Load view with new/existing Diner.
        /// </summary>
        /// <param name="dinerId"></param>
        /// <returns></returns>
        public ActionResult LoadDiner(long dinerId)
        {
            try
            {
                Session["LoadedDinerId"] = dinerId;

                DtoDiner dtoDiner = diner.Load(dinerId);

                return View("~/Areas/YMMHRSystem/Views/Diner/DinerDetailDialog.cshtml", dtoDiner);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Diner.
        /// </summary>
        /// <param name="dtoDiner"></param>
        /// <returns></returns>
        public ActionResult SaveDiner(DtoDiner dtoDiner)
        {
            try
            {

                dtoDiner.DinerId = Convert.ToInt64(Session["LoadedDinerId"]);
                if (dtoDiner.DinerId == 0)
                {
                    if (diner.GetDinerId(dtoDiner.Type, dtoDiner.StartDate, dtoDiner.FinishDate) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }
                dtoDiner.UserCreated = Session["UserName"].ToString();
                diner.Save(dtoDiner);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a Diner. 
        /// </summary>
        /// <param name="DinerId"></param>
        /// <returns></returns>
        public string DeleteDiner(long DinerId)
        {
            try
            {
                return diner.DeleteDiner(DinerId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: YMMHRSystem/ExtraordinaryDiner
        public ActionResult ExtraordinaryDiner()
        {
            try
            {
                if (Permission.QueryPermission("EXTRADINER.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveExtraordinaryDiner"] = Permission.QueryPermission("EXTRADINER.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanCancelExtraordinaryDiner"] = Permission.QueryPermission("EXTRADINER.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    if (Role.QueryRole("HR", Convert.ToInt64(Session["UserId"])) || Role.QueryRole("Admin", Convert.ToInt64(Session["UserId"])))
                    {
                        return View("~/Areas/YMMHRSystem/Views/Diner/ExtraordinaryDiner.cshtml", diner.LoadMultipleExtra());
                    }
                    else
                    {
                        return View("~/Areas/YMMHRSystem/Views/Diner/ExtraordinaryDiner.cshtml", diner.LoadMultipleExtra(Session["UserName"].ToString()));
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
        /// Add view Diner.
        /// </summary>
        /// <param name="extraordinaryDinerId"></param>
        /// <returns></returns>
        public ActionResult AddExtraordinaryDiner()
        {
            try
            {
                DtoExtraordinaryDiner dtoExtraordinaryDiner = new DtoExtraordinaryDiner();

                // Obtener todas las sugerencias de correo electrónico
                List<DtoUser> dtoUserListSuggestions = user.GetUserEmailSuggestions();
                dtoExtraordinaryDiner.UserEmailSuggestions = dtoUserListSuggestions;

                return View("~/Areas/YMMHRSystem/Views/Diner/ExtraordinaryDinerAddDialog.cshtml", dtoExtraordinaryDiner);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddGuestExtraordinaryDiner()
        {
            try
            {
                DtoExtraordinaryDiner dtoExtraordinaryDiner = new DtoExtraordinaryDiner();

                // Obtener todas las sugerencias de correo electrónico
                List<DtoUser> dtoUserListSuggestions = user.GetUserEmailSuggestions();
                dtoExtraordinaryDiner.UserEmailSuggestions = dtoUserListSuggestions;

                return View("~/Areas/YMMHRSystem/Views/Diner/GuestExtraordinaryDinerAddDialog.cshtml", dtoExtraordinaryDiner);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Load view with new/existing Diner.
        /// </summary>
        /// <param name="extraordinaryDinerId"></param>
        /// <returns></returns>
        public ActionResult LoadExtraordinaryDiner(long extraordinaryDinerId)
        {
            try
            {
                Session["LoadedExtraDinerId"] = extraordinaryDinerId;

                DtoExtraordinaryDiner dtoExtraordinaryDiner = diner.LoadExtra(extraordinaryDinerId);

                return View("~/Areas/YMMHRSystem/Views/Diner/ExtraordinaryDinerDetailDialog.cshtml", dtoExtraordinaryDiner);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Confirms Extra Diner Request
        /// </summary>
        /// <param name="extraordinaryDinerId"></param>
        /// <returns></returns>
        public ActionResult ConfirmExtraordinaryDiner(long extraordinaryDinerId)
        {
            try
            {
                Session["LoadedExtraDinerId"] = extraordinaryDinerId;

                diner.ConfirmExtraordinaryDiner(extraordinaryDinerId, Convert.ToInt64(Session["UserId"]));

                return RedirectToAction("ExtraordinaryDiner");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Diner.
        /// </summary>
        /// <param name="dtoExtraDiner"></param>
        /// <returns></returns>
        public ActionResult SaveExtraordinaryDiner(DtoExtraordinaryDiner dtoExtraDiner, string MultipleDates)
        {
            try
            {
                dtoExtraDiner.ExtraordinaryDinerId = Convert.ToInt64(Session["LoadedExtraDinerId"]);
                if (dtoExtraDiner.ExtraordinaryDinerId == 0)
                {
                    if (dtoExtraDiner.WorkerList.Count <= 0)
                    {
                        return Json(new { success = false, message = "Please select an associate." }, JsonRequestBehavior.AllowGet);
                    }
                }

                if (!Role.QueryRole("HR", Convert.ToInt64(Session["UserId"])) && !Role.QueryRole("Admin", Convert.ToInt64(Session["UserId"])))
                {
                    // Si hay fechas múltiples, se validan una por una
                    if (!string.IsNullOrEmpty(MultipleDates))
                    {
                        var dates = MultipleDates.Split(',');
                        foreach (var date in dates)
                        {
                            DateTime parsedDate;
                            if (DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                            {
                                // Aquí validamos cada fecha como se hace para dtoExtraDiner.Date
                                var validationResult = diner.ValidateDinerDate(parsedDate, dtoExtraDiner.Time);
                                if (!validationResult.success)
                                {
                                    return Json(new { success = false, message = validationResult.message }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                return Json(new { success = false, message = "Invalid date format in MultipleDates." }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        // Validar la fecha única de dtoExtraDiner
                        var validationResult = diner.ValidateDinerDate(dtoExtraDiner.Date.Value, dtoExtraDiner.Time);
                        if (!validationResult.success)
                        {
                            return Json(new { success = false, message = validationResult.message }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                if (dtoExtraDiner.WorkerList.Count > 0)
                {
                    List<DtoExtraordinaryDiner> dinerList = new List<DtoExtraordinaryDiner>();

                    // Si se recibieron múltiples fechas
                    if (!string.IsNullOrEmpty(MultipleDates))
                    {
                        // Dividir las fechas recibidas por comas
                        var dates = MultipleDates.Split(',');

                        foreach (var date in dates)
                        {
                            DateTime parsedDate;
                            if (DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                            {
                                // Crear un registro para cada trabajador con cada fecha
                                foreach (var worker in dtoExtraDiner.WorkerList)
                                {
                                    if (diner.GetExtraDinerId(worker.Names, dtoExtraDiner.Type, parsedDate, dtoExtraDiner.Time) == 0)
                                    {
                                        dinerList.Add(new DtoExtraordinaryDiner()
                                        {
                                            AssociateName = worker.Names,
                                            Process = worker.Process,
                                            Date = parsedDate,
                                            Time = dtoExtraDiner.Time,
                                            Lading = dtoExtraDiner.Lading,
                                            LadingCost = dtoExtraDiner.LadingCost,
                                            Type = dtoExtraDiner.Type,
                                            UserCreated = Session["UserName"].ToString(),
                                            Motive = dtoExtraDiner.Motive,
                                            TypeCost = dtoExtraDiner.TypeCost,
                                            Contacts = dtoExtraDiner.Contacts
                                        });
                                    }
                                }
                            }
                        }
                    }
                    else if (dtoExtraDiner.FinishDate != null)
                    {
                        if (dtoExtraDiner.FinishDate < dtoExtraDiner.Date)
                        {
                            return Json(new { success = false, message = "Finish Date cannot be less than Start Date" }, JsonRequestBehavior.AllowGet);
                        }

                        List<DateTime?> selectedDates = new List<DateTime?>();
                        for (var date = dtoExtraDiner.Date; date <= dtoExtraDiner.FinishDate; date = date?.AddDays(1))
                        {
                            selectedDates.Add(date);
                        }

                        List<DtoWorkerFile> workerDatelist = dtoExtraDiner.WorkerList.SelectMany(g => selectedDates.Select(c => new DtoWorkerFile { Names = g.Names, Process = g.Process, AdmissionDate = c.Value })).ToList();

                        foreach (var item in workerDatelist)
                        {
                            if (diner.GetExtraDinerId(item.Names, dtoExtraDiner.Type, item.AdmissionDate, dtoExtraDiner.Time) == 0)
                            {
                                dinerList.Add(new DtoExtraordinaryDiner()
                                {
                                    AssociateName = item.Names,
                                    Process = item.Process,
                                    Date = item.AdmissionDate,
                                    Time = dtoExtraDiner.Time,
                                    Lading = dtoExtraDiner.Lading,
                                    LadingCost = dtoExtraDiner.LadingCost,
                                    Type = dtoExtraDiner.Type,
                                    UserCreated = Session["UserName"].ToString(),
                                    Motive = dtoExtraDiner.Motive,
                                    TypeCost = dtoExtraDiner.TypeCost,
                                    Contacts = dtoExtraDiner.Contacts
                                });
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in dtoExtraDiner.WorkerList)
                        {
                            if (diner.GetExtraDinerId(item.Names, dtoExtraDiner.Type, dtoExtraDiner.Date, dtoExtraDiner.Time) == 0)
                            {
                                dinerList.Add(new DtoExtraordinaryDiner()
                                {
                                    AssociateName = item.Names,
                                    Process = item.Process,
                                    Date = dtoExtraDiner.Date,
                                    Time = dtoExtraDiner.Time,
                                    Lading = dtoExtraDiner.Lading,
                                    LadingCost = dtoExtraDiner.LadingCost,
                                    Type = dtoExtraDiner.Type,
                                    UserCreated = Session["UserName"].ToString(),
                                    Motive = dtoExtraDiner.Motive,
                                    TypeCost = dtoExtraDiner.TypeCost,
                                    Contacts = dtoExtraDiner.Contacts
                                });
                            }
                        }
                    }

                    // Guardar todos los registros
                    diner.SaveMultipleExtra(dinerList, Convert.ToInt64(Session["UserId"]));
                }
                else
                {
                    if (dtoExtraDiner.ExtraordinaryDinerId == 0)
                    {
                        if (diner.GetExtraDinerId(dtoExtraDiner.AssociateName, dtoExtraDiner.Type, dtoExtraDiner.Date, dtoExtraDiner.Time) != 0)
                        {
                            return Json(new { success = false, message = "This ExtraDiner already exists" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    dtoExtraDiner.Lading = dtoExtraDiner.LadingCost > 0.0m ? true : false;
                    if (dtoExtraDiner.ExtraordinaryDinerId == 0)
                    {
                        dtoExtraDiner.UserCreated = Session["UserName"].ToString();
                    }
                    else
                    {
                        dtoExtraDiner.UserModified = Session["UserName"].ToString();
                    }
                    dtoExtraDiner.FinishDate = dtoExtraDiner.Date;
                    diner.SaveExtra(dtoExtraDiner, Convert.ToInt64(Session["UserId"]));
                }

                return Json(new { success = true, message = "Diner saved" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Saves a Diner.
        /// </summary>
        /// <param name="dtoExtraDiner"></param>
        /// <returns></returns>
        public ActionResult SaveGuestExtraordinaryDiner(DtoExtraordinaryDiner dtoExtraDiner, string MultipleDates)
        {
            try
            {
                
                if (dtoExtraDiner.GuestQuantity <= 0)
                {
                    return Json(new { success = false, message = "Guest quantity must be greater than zero." }, JsonRequestBehavior.AllowGet);
                }

                // Si no hay fechas múltiples ni FinishDate, asignar FinishDate = Date
                if (dtoExtraDiner.FinishDate == null && string.IsNullOrEmpty(MultipleDates) && dtoExtraDiner.Date != null)
                {
                    dtoExtraDiner.FinishDate = dtoExtraDiner.Date;
                }

                if (!Role.QueryRole("HR", Convert.ToInt64(Session["UserId"])) && !Role.QueryRole("Admin", Convert.ToInt64(Session["UserId"])))
                {
                    // Si hay fechas múltiples, se validan una por una
                    if (!string.IsNullOrEmpty(MultipleDates))
                    {
                        var dates = MultipleDates.Split(',');
                        foreach (var date in dates)
                        {
                            DateTime parsedDate;
                            if (DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                            {
                                // Aquí validamos cada fecha como se hace para dtoExtraDiner.Date
                                var validationResult = diner.ValidateDinerDate(parsedDate, dtoExtraDiner.Time);
                                if (!validationResult.success)
                                {
                                    return Json(new { success = false, message = validationResult.message }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                return Json(new { success = false, message = "Invalid date format in MultipleDates." }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        // Validar la fecha única de dtoExtraDiner
                        DateTime dateOrdered = Convert.ToDateTime(dtoExtraDiner.Date + dtoExtraDiner.Time);
                        var validationResult = diner.ValidateDinerDate(dtoExtraDiner.Date.Value, dtoExtraDiner.Time);

                        if (!validationResult.success)
                        {
                            return Json(new { success = false, message = validationResult.message }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                List<DtoExtraordinaryDiner> dinerList = new List<DtoExtraordinaryDiner>();
                WorkerFile workerFile = new WorkerFile();

                for (int i = 0; i < dtoExtraDiner.GuestQuantity; i++)
                {
                    dtoExtraDiner.WorkerList.Add(new DtoWorkerFile { Names = "Guest", Process = dtoExtraDiner.Process });
                }

                if (dtoExtraDiner.FinishDate != null)
                {
                    if (dtoExtraDiner.FinishDate < dtoExtraDiner.Date)
                    {
                        return Json(new {success = false, message = "The end date is less than the current date" });
                    }

                    List<DateTime?> selectedDates = new List<DateTime?>();
                    for (var date = dtoExtraDiner.Date; date <= dtoExtraDiner.FinishDate; date = date?.AddDays(1))
                    {
                        selectedDates.Add(date);
                    }
                   
                    List<DtoWorkerFile> workerDatelist = dtoExtraDiner.WorkerList.SelectMany(g => selectedDates.Select(c => new DtoWorkerFile { Names = g.Names, Process = g.Process, AdmissionDate = c.Value })).ToList();

                    foreach (var item in workerDatelist)
                    {
                        if (diner.GetExtraDinerId(item.Names, dtoExtraDiner.Type, item.AdmissionDate, dtoExtraDiner.Time) == 0)
                        {
                            dinerList.Add(new DtoExtraordinaryDiner()
                            {
                                AssociateName = item.Names,
                                Process = item.Process,
                                Date = item.AdmissionDate,
                                Time = dtoExtraDiner.Time,
                                Lading = dtoExtraDiner.Lading,
                                LadingCost = dtoExtraDiner.LadingCost,
                                Type = dtoExtraDiner.Type,
                                UserCreated = Session["UserName"].ToString(),
                                Motive = dtoExtraDiner.Motive,
                                TypeCost = dtoExtraDiner.TypeCost,
                                Contacts = dtoExtraDiner.Contacts
                            });
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(MultipleDates)) 
                    {
                        var dates = MultipleDates.Split(',');
                        foreach (var date in dates) 
                        {
                            DateTime parsedDate;
                            DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);
                            foreach (var item in dtoExtraDiner.WorkerList)
                            {
                                if (diner.GetExtraDinerId(item.Names, dtoExtraDiner.Type, parsedDate, dtoExtraDiner.Time) == 0)
                                {
                                    dinerList.Add(new DtoExtraordinaryDiner()
                                    {
                                        AssociateName = item.Names,
                                        Process = item.Process,
                                        Date = parsedDate,
                                        Time = dtoExtraDiner.Time,
                                        Lading = dtoExtraDiner.Lading,
                                        LadingCost = dtoExtraDiner.LadingCost,
                                        Type = dtoExtraDiner.Type,
                                        UserCreated = Session["UserName"].ToString(),
                                        Motive = dtoExtraDiner.Motive,
                                        TypeCost = dtoExtraDiner.TypeCost,
                                        Contacts = dtoExtraDiner.Contacts
                                    });
                                }
                            }
                        }
                    }
                }

                diner.SaveMultipleExtra(dinerList, Convert.ToInt64(Session["UserId"]));

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a Diner. 
        /// </summary>
        /// <param name="extraordinaryDinerId"></param>
        /// <returns></returns>
        public string CancelExtraordinaryDiner(long extraordinaryDinerId)
        {
            try
            {
                return diner.CancelExtraDiner(extraordinaryDinerId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Loads Route's stop list
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns></returns>
        public ActionResult LoadStops(long routeId)
        {
            try
            {
                Route route = new Route();
                var stopList = route.GetRouteStopList(routeId);

                return Json(stopList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


    }
}