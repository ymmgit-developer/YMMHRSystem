using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class ProcessController : Controller
    {
        Process process = new Process();

        // GET: YMMHRSystem/Process
        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("PROCESS.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveProcess"] = Permission.QueryPermission("PROCESS.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanDeleteProcess"] = Permission.QueryPermission("PROCESS.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View("~/Areas/YMMHRSystem/Views/Process/Index.cshtml", process.LoadMultiple());
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
        /// Load view with new/exisiting Process.
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public ActionResult LoadProcess(long processId)
        {
            try
            {
                Session["LoadedProcessId"] = processId;

                DtoProcess dtoProcess = process.Load(processId);

                return View("~/Areas/YMMHRSystem/Views/Process/ProcessDetailDialog.cshtml", dtoProcess);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Process.
        /// </summary>
        /// <param name="dtoProcess"></param>
        /// <returns></returns>
        public ActionResult SaveProcess(DtoProcess dtoProcess)
        {
            DtoProcess dtoProcesses = new DtoProcess();

            try
            {

                dtoProcesses.ProcessId = Convert.ToInt64(Session["LoadedProcessId"]);
                if (dtoProcesses.ProcessId == 0)
                {
                    if (process.GetProcessId(dtoProcess.Name) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }

                dtoProcesses.Name = dtoProcess.Name;
                dtoProcesses.Code = dtoProcess.Code;
                dtoProcesses.UserCreated = dtoProcess.UserCreated;
                dtoProcesses.DateAdded = dtoProcess.DateAdded;

                process.Save(dtoProcesses);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a Process. 
        /// </summary>
        /// <param name="ProcessId"></param>
        /// <returns></returns>
        public string DeleteProcess(long ProcessId)
        {
            try
            {
                return process.DeleteProcess(ProcessId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}