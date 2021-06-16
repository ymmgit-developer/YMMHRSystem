using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class StopController : Controller
    {
        Stop stop = new Stop();

        // GET: YMMHRSystem/Stop
        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("STOP.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveStop"] = Permission.QueryPermission("STOP.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanDeleteStop"] = Permission.QueryPermission("STOP.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View("~/Areas/YMMHRSystem/Views/Stop/Index.cshtml", stop.LoadMultiple());
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
        /// Load view with new/exisiting Stop.
        /// </summary>
        /// <param name="stopId"></param>
        /// <returns></returns>
        public ActionResult LoadStop(long stopId)
        {
            try
            {
                Session["LoadedStopId"] = stopId;

                DtoStop dtoStop = stop.Load(stopId);

                return View("~/Areas/YMMHRSystem/Views/Stop/StopDetailDialog.cshtml", dtoStop);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Stop.
        /// </summary>
        /// <param name="dtoStop"></param>
        /// <returns></returns>
        public ActionResult SaveStop(DtoStop dtoStop)
        {
            try
            {

                dtoStop.StopId = Convert.ToInt64(Session["LoadedStopId"]);
                if (dtoStop.StopId == 0)
                {
                    if (stop.GetStopId(dtoStop.Name) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }
                dtoStop.UserCreated = Session["UserName"].ToString();


                stop.Save(dtoStop);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a Stop. 
        /// </summary>
        /// <param name="StopId"></param>
        /// <returns></returns>
        public string DeleteStop(long StopId)
        {
            try
            {
                return stop.DeleteStop(StopId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}