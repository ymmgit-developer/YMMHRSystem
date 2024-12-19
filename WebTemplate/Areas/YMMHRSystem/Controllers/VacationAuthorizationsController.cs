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
    public class VacationAuthorizationsController : Controller
    {
        VacationAuthorizations vacationAuthorization = new VacationAuthorizations();
        WorkerFile workerFile = new WorkerFile();
        User users = new User();
        
        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("VACATIONAUTHORIZATION.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanCreateVacationAuthorizationRecord"] = Permission.QueryPermission("VACATIONAUTHORIZATION.CREATE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    HttpContext.Session["CanDeleteVacationAuthorizationRecord"] = Permission.QueryPermission("VACATIONAUTHORIZATION.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    ViewBag.Users = users.LoadMultiple();
                    ViewBag.Workers = workerFile.LoadMultiple();
                    return View("~/Areas/YMMHRSystem/Views/VacationAuthorizations/Index.cshtml", vacationAuthorization.Load());
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

        public ActionResult Add()
        {
            try
            {
                ViewBag.Users = users.LoadMultiple();
                ViewBag.Workers = workerFile.LoadMultiple();
                return PartialView("~/Areas/YMMHRSystem/Views/VacationAuthorizations/AuthorizationAddDialog.cshtml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Save(List<DtoWorkerList> WorkerList, List<DtoUserList> UserList)
        {
            try
            {
                foreach (var User in UserList)
                {
                    foreach (var Worker in WorkerList)
                    {
                        DtoVacationAuthorizations dtoVacationAuthorizations = new DtoVacationAuthorizations();
                        dtoVacationAuthorizations.WorkerFileId = long.Parse(Worker.WorkerFileId);
                        dtoVacationAuthorizations.UserId = users.GetUserId(User.WorkerId);
                        dtoVacationAuthorizations.DateAdded = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        vacationAuthorization.Save(dtoVacationAuthorizations);
                    }
                }
                return Json(new { success = true, message = "Vacation Authorization Record Saved Successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Failed to Save Vacation Authorization Record" });
            }
        }

        public ActionResult Delete(int vacationAuthorizationsId)
        {
            try
            {
                vacationAuthorization.Delete(vacationAuthorizationsId);
                return Json(new { success = true, message = "Vacation Authorization Record Deleted Successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Failed to Delete Vacation Authorization Record" });
            }
        }
    }
}