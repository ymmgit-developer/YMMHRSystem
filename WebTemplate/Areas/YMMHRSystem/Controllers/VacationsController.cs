using Newtonsoft.Json.Converters;
using OfficeOpenXml.Drawing.Slicer.Style;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using YMMHRSystemLogic;
using YMMHRSystemLogic.Classes;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class VacationsController: Controller
    {
        Vacations vacation = new Vacations();
        DatesForVacations dates = new DatesForVacations();
        WorkerFile workerFile = new WorkerFile();
        Process process = new Process();
        User users = new User();
        VacationAuthorizations vacationAuthorizations = new VacationAuthorizations();

        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("VACATION.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanCreateVacationRequest"] = Permission.QueryPermission("VACATION.CREATE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    HttpContext.Session["CanApproveVacationRequest"] = Permission.QueryPermission("VACATION.APPROVE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    HttpContext.Session["CanDeleteVacationRequest"] = Permission.QueryPermission("VACATION.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    ViewBag.UserId = users.GetUserId(HttpContext.Session["UserId"].ToString());
                    ViewBag.WorkerFileIdAllowedToPass = users.GetWorkerId(long.Parse(HttpContext.Session["UserId"].ToString()));
                    ViewBag.Process = process.LoadMultiple();
                    ViewBag.Workers = workerFile.LoadMultiple();
                    return View();
                }
                else
                {
                    return View("~/Views/Shared/AccessDenied.cshtml");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult AddRegisterVacation(long WorkerFileId)
        {
            try
            {
                ViewBag.Workerfile = workerFile.Load(WorkerFileId);
                ViewBag.MonthsOld = vacation.MonthsOldbyUser(WorkerFileId);
                return PartialView("~/Areas/YMMHRSystem/Views/Vacations/VacationAddDialog.cshtml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetRecordsVacationbyWorkerFileId(long WorkerFileId)
        {
            try
            {
                var dataVacations =  new List<DtoVacationRecordView>();
                List<DtoVacations> vacations = vacation.GetVacationRequestsbyUser(WorkerFileId);
                foreach (var vacation in vacations)
                {
                    List<DtoDatesForVacations> datesForVacations = dates.GetDayesVacationRequestsbyVacationId(vacation.VacationId);
                    var datesList = datesForVacations.Select(d => d.Date.ToString("yyyy-MM-dd")).ToList();
                    dataVacations.Add(new DtoVacationRecordView
                    {
                        VacationId = vacation.VacationId,
                        DateRequest = vacation.DateRequest,
                        WorkerFileId = vacation.WorkerFileId,
                        DaysRequest = vacation.DaysRequest,
                        Comments = vacation.Comments,
                        BossAuthorization = vacation.BossAuthorization,
                        HrAuthorization = vacation.HrAuthorization,
                        GralStatus = vacation.GralStatus,
                        Blocked = vacation.Blocked,
                        DatesList = datesList
                    });
                }
                return PartialView("~/Areas/YMMHRSystem/Views/Vacations/VacationViewRecordsDialog.cshtml", dataVacations);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult SaveRegisterVacation(string WorkerFileId, string DatesVacation, string Comments)
        {
            try
            {
                // Dividir las fechas en un arreglo basado en comas
                var datesArray = DatesVacation.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var daysRequested = datesArray.Length; // Contar el número de fechas
                long VacationId;

                // Obtener antigüedad del trabajador en meses
                var monthsOld = vacation.MonthsOldbyUser(long.Parse(WorkerFileId));

                // Crear el registro de vacaciones
                var vacations = new DtoVacations
                {
                    DateRequest = DateTime.Now,
                    WorkerFileId = long.Parse(WorkerFileId),
                    DaysRequest = daysRequested,
                    Comments = Comments,
                    BossAuthorization = false,
                    HrAuthorization = false,
                    GralStatus = 1,
                    Blocked = false
                };

                if (vacation.Save(vacations)) // Si el registro en Vacations es correcto
                {
                    VacationId = vacation.GetIdentityVacations();
                    // Aplicar reglas de recorte basadas en la antigüedad
                    if (monthsOld >= 0 && monthsOld <= 3) //De 0 meses a 3 meses
                    {
                        // Dejar solo la primera fecha
                        foreach (var date in datesArray.Take(1)) //puede tomar 1 dia
                        {
                            var datesForVacations = new DtoDatesForVacations
                            {
                                VacationId = VacationId,
                                Date = DateTime.Parse(date),
                            };
                            dates.Save(datesForVacations);
                        }
                    }
                    else if (monthsOld >= 4 && monthsOld < 12) //De 4 meses y antes de 12 meses
                    {
                        // Dejar solo las primeras 3 fechas
                        foreach (var date in datesArray.Take(3)) //puede tomar 3 dias
                        {
                            var datesForVacations = new DtoDatesForVacations
                            {
                                VacationId = VacationId,
                                Date = DateTime.Parse(date),
                            };
                            dates.Save(datesForVacations);
                        }
                    }
                    else if (monthsOld >= 12 && vacation.TotalAvailableDays(long.Parse(WorkerFileId)) > 0) //De 12 meses y si tiene dias disponibles puede tomar vacaciones
                    {
                        foreach (var date in datesArray)
                        {
                            var datesForVacations = new DtoDatesForVacations
                            {
                                VacationId = VacationId,
                                Date = DateTime.Parse(date),
                            };
                            dates.Save(datesForVacations);
                        }
                        vacation.AnniversaryVacationSharing(long.Parse(WorkerFileId), datesArray.Length, VacationId); //Distribuye los dias solicitados entre las dias disponibles mas antiguos
                    }
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditRegisterVacation(long VacationId)
        {
            try
            {
                DtoVacations record = new DtoVacations();
                List<DtoDatesForVacations> recordDates = new List<DtoDatesForVacations>();
                recordDates = dates.GetDayesVacationRequestsbyVacationId(VacationId);
                string datesString = string.Join(", ", recordDates.Select(d => d.Date.ToString("yyyy-MM-dd")));
                record = vacation.GetVacationRequestByVacationId(VacationId);
                ViewBag.Dates = datesString;
                return PartialView("~/Areas/YMMHRSystem/Views/Vacations/VacationEditDialog.cshtml", record);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult UpdateRecordVacations(long VacationId, string DatesVacation, string Comments)
        {
            try
            {
                var datesArray = DatesVacation.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (vacation.Update(Comments, VacationId, datesArray.Length) && dates.Delete(VacationId) )
                {
                    foreach (var date in datesArray)
                    {
                        var datesForVacations = new DtoDatesForVacations
                        {
                            VacationId = VacationId,
                            Date = DateTime.Parse(date),
                        };
                        dates.Save(datesForVacations);
                    }
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ApproveBossRegisterVacation(long VacationId)
        {
            try
            {
                if (vacation.ApproveBossVacation(VacationId) && vacation.StatusUpdate(VacationId, 2))
                {
                    vacation.Blocked(VacationId);
                    return Json(new { success = true, message = "Vacation approved!" });
                }
                else { return Json(new { success = false, message = "Error" }); }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}