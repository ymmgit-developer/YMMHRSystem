using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class LegalRequirementController : Controller
    {
        LegalRequirement legalRequirement = new LegalRequirement();

        // GET: YMMHRSystem/LegalRequirement
        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("LEGALREQUIREMENT.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveLegalRequirement"] = Permission.QueryPermission("LEGALREQUIREMENT.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanDeleteLegalRequirement"] = Permission.QueryPermission("LEGALREQUIREMENT.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View("~/Areas/YMMHRSystem/Views/LegalRequirement/Index.cshtml", legalRequirement.LoadMultiple());
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
        /// Load view with new/exisiting LegalRequirement.
        /// </summary>
        /// <param name="legalRequirementId"></param>
        /// <returns></returns>
        public ActionResult LoadLegalRequirement(long legalRequirementId)
        {
            try
            {
                Session["LoadedLegalRequirementId"] = legalRequirementId;

                DtoLegalRequirement dtoLegalRequirement = legalRequirement.Load(legalRequirementId);

                return View("~/Areas/YMMHRSystem/Views/LegalRequirement/LegalRequirementDetailDialog.cshtml", dtoLegalRequirement);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a LegalRequirement.
        /// </summary>
        /// <param name="dtoLegalRequirement"></param>
        /// <returns></returns>
        public ActionResult SaveLegalRequirement(DtoLegalRequirement dtoLegalRequirement)
        {

            try
            {
                dtoLegalRequirement.LegalRequirementId = Convert.ToInt64(Session["LoadedLegalRequirementId"]);

                legalRequirement.Save(dtoLegalRequirement);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a LegalRequirement. 
        /// </summary>
        /// <param name="legalRequirementId"></param>
        /// <returns></returns>
        public string DeleteLegalRequirement(long legalRequirementId)
        {
            try
            {
                return legalRequirement.DeleteLegalRequirement(legalRequirementId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Load attachment view.
        /// </summary>
        /// <returns></returns>
        public ActionResult AttachmentDialog()
        {
            try
            {
                return View("~/Areas/YMMHRSystem/Views/LegalRequirement/AttachmentDialog.cshtml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Load attachment view.
        /// </summary>
        /// <returns></returns>
        public ActionResult AttachmentListDialog(long legalRequirementId)
        {
            try
            {
                Session["LoadedLegalRequirementId"] = legalRequirementId;
                List<DtoLegalRequirementAttachment> dtoAttachmentList = legalRequirement.LoadMultipleAttachments(legalRequirementId);
                return View("~/Areas/YMMHRSystem/Views/LegalRequirement/LegalRequirementAttachmentList.cshtml", dtoAttachmentList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Adds a LegalRequirement Attachment. 
        /// </summary>
        /// <param name="LegalRequirementFileId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddAttachment(HttpPostedFileBase file)
        {
            try
            {
                DtoLegalRequirementAttachment dtoLegalRequirementAttachment = new DtoLegalRequirementAttachment();
                dtoLegalRequirementAttachment.LegalRequirementId = Convert.ToInt64(Session["LoadedLegalRequirementId"]);

                if (file != null)
                {
                    string checkextension = Path.GetExtension(file.FileName).ToLower();
                    if (checkextension == ".pdf")
                    {
                        string path = Server.MapPath("~/LegalRequirementFiles/Attachments/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        dtoLegalRequirementAttachment.FileName = path + file.FileName;
                        file.SaveAs(path + file.FileName);
                    }
                    else
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }

                }
                return legalRequirement.AddAttachment(dtoLegalRequirementAttachment) ? Json("true", JsonRequestBehavior.AllowGet) : Json("false", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Removes a attachment file. 
        /// </summary>
        /// <param name="legalRequirementAttachmentId"></param>
        /// <returns></returns>
        public string DeleteAttachmentFile(long legalRequirementAttachmentId)
        {
            try
            {
                string fileName = legalRequirement.GetLegalRequirementAttachmentFile(legalRequirementAttachmentId);
                return legalRequirement.DeleteAttachment(legalRequirementAttachmentId, fileName) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Get attachment file
        /// </summary>
        /// <param name="legalRequirementAttachmentId"></param>
        /// <returns>file</returns>
        public ActionResult GetAttachmentPDF(long legalRequirementAttachmentId)
        {
            try
            {
                string file = legalRequirement.GetLegalRequirementAttachmentFile(legalRequirementAttachmentId);
                return File(file, "application/pdf");
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
