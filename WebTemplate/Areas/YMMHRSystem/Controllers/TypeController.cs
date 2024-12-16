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
    public class TypeController : Controller
    {
        // GET: YMMHRSystem/Type
        Types type = new Types();
        Log log = new Log();

        public ActionResult Index()
        {
            try 
            {
                if (Permission.QueryPermission("TYPE.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveType"] = Permission.QueryPermission("TYPE.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    HttpContext.Session["CanDeleteType"] = Permission.QueryPermission("TYPE.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    return View("~/Areas/YMMHRSystem/Views/Types/Index.cshtml", type.GetTypes());
                }
                else
                {
                    return View("~/Views/Shared/AccessDenied.cshtml");
                }
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Index Type", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Index");
                throw ex;
            }
        }

        /// <summary>
        /// Load view with new/exisiting Type.
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public ActionResult LoadType(long typeId)
        {
            try
            {
                Session["LoadedTypeId"] = typeId;

                DtoType dtoType = type.Load(typeId);

                return View("~/Areas/YMMHRSystem/Views/Types/TypeDetailDialog.cshtml", dtoType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Saves a Type.
        /// </summary>
        /// <param name="dtoType"></param>
        /// <returns></returns>
        public ActionResult SaveType(DtoType dtoType)
        {
            try
            {
                dtoType.IdType = Convert.ToInt64(Session["LoadedTypeId"]);
                if (dtoType.IdType == 0)
                {
                    if (type.GetTypeId(dtoType.DescriptionType) != 0)
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                dtoType.UserCreated = Session["UserName"].ToString();
                type.Save(dtoType);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// Elimina un tipo de salida. 
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public bool DeleteType(long typeId)
        {
            try
            {
                return type.DeleteType(typeId) ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}