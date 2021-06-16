using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class ImmigrationProcedureController : Controller
    {
        ImmigrationProcedure immigrationProcedure = new ImmigrationProcedure();
        SendEmail sendEmail = new SendEmail();
        EmailNotification emailNotification = new EmailNotification();

        // GET: YMMHRSystem/ImmigrationProcedure
        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("IMMIGRATION.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveImmigrationProcedure"] = Permission.QueryPermission("IMMIGRATION.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanDeleteImmigrationProcedure"] = Permission.QueryPermission("IMMIGRATION.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    if (Role.QueryRole("GA", Convert.ToInt64(Session["UserId"])) || Role.QueryRole("Admin", Convert.ToInt64(Session["UserId"])))
                    {
                        return View("~/Areas/YMMHRSystem/Views/ImmigrationProcedure/Index.cshtml", immigrationProcedure.LoadMultiple());
                    }
                    else
                    {
                        return View("~/Areas/YMMHRSystem/Views/ImmigrationProcedure/Index.cshtml", immigrationProcedure.LoadMultiple(Session["UserName"].ToString()));
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
        /// Load view with new/exisiting Immigration Procedure.
        /// </summary>
        /// <param name="immigrationProcedureId"></param>
        /// <returns></returns>
        public ActionResult LoadImmigrationProcedure(long immigrationProcedureId)
        {
            try
            {
                Session["LoadedImmigrationProcedureId"] = immigrationProcedureId;

                DtoImmigrationProcedure dtoImmigrationProcedure = immigrationProcedure.Load(immigrationProcedureId);

                return View("~/Areas/YMMHRSystem/Views/ImmigrationProcedure/ImmigrationProcedureDetailDialog.cshtml", dtoImmigrationProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a ImmigrationProcedure.
        /// </summary>
        /// <param name="dtoImmigrationProcedure"></param>
        /// <returns></returns>
        public ActionResult SaveImmigrationProcedure(DtoImmigrationProcedure dtoImmigrationProcedure)
        {
            try
            {
                dtoImmigrationProcedure.ImmigrationProcedureId = Convert.ToInt64(Session["LoadedImmigrationProcedureId"]);

                dtoImmigrationProcedure.CreatedBy = Session["UserName"].ToString();
                immigrationProcedure.Save(dtoImmigrationProcedure);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Approve an Immigration Procedure.
        /// </summary>
        /// <returns></returns>
        public ActionResult ApproveImmigrationProcedure()
        {
            try
            {
                long immigrationProcedureId = Convert.ToInt64(Session["LoadedImmigrationProcedureId"]);
                DtoImmigrationProcedure dtoImmigrationProcedure = immigrationProcedure.Load(immigrationProcedureId);
                immigrationProcedure.ChangeImmigrationProcedureStatus(2, immigrationProcedureId);
                sendEmail.SendEmailTemplate("YMM HR System: Immigration Procedure Approval", "TemplateImmigrationProcedureApproval", new[,]
{
                        {"$APPLICANT$", dtoImmigrationProcedure.CreatedBy},
                        {"$ASSOCIATE$", dtoImmigrationProcedure.Associate},
                        {"$PROCEDURE$", dtoImmigrationProcedure.ProcedureType},
                        {"$DOCUMENT$", dtoImmigrationProcedure.DocumentType},
                        {"$DATE$", dtoImmigrationProcedure.ExpirationDate.ToString("dd/MM/yyyy")}
                    }, sendEmail.GetAdminEmail(), emailNotification.GetLegalAffairContacts(1).Split(',').ToList());

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Rejects an Immigration Procedure.
        /// </summary>
        /// <returns></returns>
        public ActionResult RejectImmigrationProcedure(string rejectionMotive)
        {
            try
            {
                long immigrationProcedureId = Convert.ToInt64(Session["LoadedImmigrationProcedureId"]);
                DtoImmigrationProcedure dtoImmigrationProcedure = immigrationProcedure.Load(immigrationProcedureId);
                immigrationProcedure.ChangeImmigrationProcedureStatus(3, immigrationProcedureId, rejectionMotive);
                sendEmail.SendEmailTemplate("YMM HR System: Immigration Procedure Rejection", "TemplateImmigrationProcedureRejection", new[,]
{
                        {"$APPLICANT$", dtoImmigrationProcedure.CreatedBy},
                        {"$ASSOCIATE$", dtoImmigrationProcedure.Associate},
                        {"$PROCEDURE$", dtoImmigrationProcedure.ProcedureType},
                        {"$DOCUMENT$", dtoImmigrationProcedure.DocumentType},
                        {"$DATE$", dtoImmigrationProcedure.ExpirationDate.ToString("dd/MM/yyyy")},
                        {"$MOTIVE$", dtoImmigrationProcedure.RejectionMotive }
                    }, sendEmail.GetAdminEmail(), emailNotification.GetLegalAffairContacts(1).Split(',').ToList());


                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Removes a ImmigrationProcedure. 
        /// </summary>
        /// <param name="immigrationProcedureId"></param>
        /// <returns></returns>
        public string DeleteImmigrationProcedure(long immigrationProcedureId)
        {
            try
            {
                return immigrationProcedure.DeleteImmigrationProcedure(immigrationProcedureId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
