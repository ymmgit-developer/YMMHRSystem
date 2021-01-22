using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class LegalProcedureController : Controller
    {
        LegalProcedure legalProcedure = new LegalProcedure();
        // GET: YMMHRSystem/LegalProcedure
        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("LEGAL.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveLegalProcedure"] = Permission.QueryPermission("LEGAL.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanDeleteLegalProcedure"] = Permission.QueryPermission("LEGAL.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View("~/Areas/YMMHRSystem/Views/LegalProcedure/Index.cshtml", legalProcedure.LoadMultiple());
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
        /// Load view with new/exisiting LegalProcedure.
        /// </summary>
        /// <param name="legalProcedureId"></param>
        /// <returns></returns>
        public ActionResult LoadLegalProcedure(long legalProcedureId)
        {
            try
            {
                Session["LoadedLegalProcedureId"] = legalProcedureId;

                DtoLegalProcedure dtoLegalProcedure = legalProcedure.Load(legalProcedureId);

                return View("~/Areas/YMMHRSystem/Views/LegalProcedure/LegalProcedureDetailDialog.cshtml", dtoLegalProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a LegalProcedure.
        /// </summary>
        /// <param name="dtoLegalProcedure"></param>
        /// <returns></returns>
        public ActionResult SaveLegalProcedure(DtoLegalProcedure dtoLegalProcedure)
        {
            DtoLegalProcedure dtoLegalProcedurees = new DtoLegalProcedure();

            try
            {

                dtoLegalProcedurees.LegalProcedureId = Convert.ToInt64(Session["LoadedLegalProcedureId"]);
                if (dtoLegalProcedurees.LegalProcedureId == 0)
                {
                    if (legalProcedure.GetLegalProcedureId(dtoLegalProcedure.Name) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }

                dtoLegalProcedurees.Name = dtoLegalProcedure.Name;
                dtoLegalProcedurees.Process = dtoLegalProcedure.Process;
                dtoLegalProcedurees.UserCreated = dtoLegalProcedure.UserCreated;
                dtoLegalProcedurees.DateAdded = dtoLegalProcedure.DateAdded;

                legalProcedure.Save(dtoLegalProcedurees);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a LegalProcedure. 
        /// </summary>
        /// <param name="LegalProcedureId"></param>
        /// <returns></returns>
        public string DeleteLegalProcedure(long LegalProcedureId)
        {
            try
            {
                return legalProcedure.DeleteLegalProcedure(LegalProcedureId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}