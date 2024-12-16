using DBFramework;
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
    public class MotiveController : Controller
    {
        // GET: YMMHRSystem/Motive
        Motive motive = new Motive();
        Log log = new Log();

        public ActionResult Index() 
        {
            try
            {
                if (Permission.QueryPermission("MOTIVE.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveMotive"] = Permission.QueryPermission("MOTIVE.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    HttpContext.Session["CanDeleteMotive"] = Permission.QueryPermission("MOTIVE.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    return View("~/Areas/YMMHRSystem/Views/Motive/Index.cshtml", motive.GetMotives());
                }
                else
                {
                    return View("~/Views/Shared/AccessDenied.cshtml");
                }
            }
            catch(Exception ex)
            {
                log.WriteToErrorLog("HR System", "Index Motive", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Index");
                throw ex;
            }
        }

        /// <summary>
        /// Load view with new/exisiting Motive.
        /// </summary>
        /// <param name="motiveId"></param>
        /// <returns></returns>
        public ActionResult LoadMotive(long motiveId)
        {
            try
            {
                Session["LoadedMotiveId"] = motiveId;

                DtoMotive dtoMotive = motive.Load(motiveId);

                return View("~/Areas/YMMHRSystem/Views/Motive/MotiveDetailDialog.cshtml", dtoMotive);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Saves a Type.
        /// </summary>
        /// <param name="dtoMotive"></param>
        /// <returns></returns>
        public ActionResult SaveMotive(DtoMotive dtoMotive)
        {
            try
            {
                dtoMotive.IdMotive = Convert.ToInt64(Session["LoadedMotiveId"]);
                if (dtoMotive.IdMotive == 0)
                {
                    if (motive.GetMotiveId(dtoMotive.DescriptionMotive) != 0)
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                dtoMotive.UserCreated = Session["UserName"].ToString();
                motive.Save(dtoMotive);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// Elimina un motivo de salida. 
        /// </summary>
        /// <param name="motiveId"></param>
        /// <returns></returns>
        public bool DeleteMotive(long motiveId)
        {
            try
            {
                return motive.DeleteMotive(motiveId) ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}