using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class FindingTypeController : Controller
    {
        FindingType findingType = new FindingType();

        // GET: YMMHRSystem/FindingType
        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("FINDING.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveFindingType"] = Permission.QueryPermission("FINDING.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanDeleteFindingType"] = Permission.QueryPermission("FINDING.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View("~/Areas/YMMHRSystem/Views/FindingType/Index.cshtml", findingType.LoadMultiple());
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
        /// Load view with new/exisiting FindingType.
        /// </summary>
        /// <param name="findingTypeId"></param>
        /// <returns></returns>
        public ActionResult LoadFindingType(long findingTypeId)
        {
            try
            {
                Session["LoadedFindingTypeId"] = findingTypeId;

                DtoFindingType dtoFindingType = findingType.Load(findingTypeId);

                return View("~/Areas/YMMHRSystem/Views/FindingType/FindingTypeDetailDialog.cshtml", dtoFindingType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a FindingType.
        /// </summary>
        /// <param name="dtoFindingType"></param>
        /// <returns></returns>
        public ActionResult SaveFindingType(DtoFindingType dtoFindingType)
        {

            try
            {

                dtoFindingType.FindingTypeId = Convert.ToInt64(Session["LoadedFindingTypeId"]);
                if (dtoFindingType.FindingTypeId == 0)
                {
                    if (findingType.GetFindingTypeId(dtoFindingType.Name) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }

                findingType.Save(dtoFindingType);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a FindingType. 
        /// </summary>
        /// <param name="findingTypeId"></param>
        /// <returns></returns>
        public string DeleteFindingType(long findingTypeId)
        {
            try
            {
                return findingType.DeleteFindingType(findingTypeId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
