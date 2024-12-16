using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using YMMHRSystemLogic;

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

        public ActionResult Index()
        {
            try
            {
                ViewBag.UserId = users.GetUserId(HttpContext.Session["UserId"].ToString());
                ViewBag.Process = process.LoadMultiple();
                ViewBag.Workers = workerFile.LoadMultiple();
                return View("");
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
                
                if (vacation.Save(vacations))
                {
                    VacationId = vacation.GetIdentityVacations();
                    foreach (var date in datesArray)
                    {
                        var datesforvacations = new DtoDatesForVacations
                        {
                            VacationId = VacationId,
                            Date = DateTime.Parse(date),
                        };
                        dates.Save(datesforvacations);
                    }
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else { return Json(false, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception ex)
            {
                throw ex;
                
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
                if (vacation.Update(Comments, VacationId) && dates.Delete(VacationId))
                {
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
    }
}