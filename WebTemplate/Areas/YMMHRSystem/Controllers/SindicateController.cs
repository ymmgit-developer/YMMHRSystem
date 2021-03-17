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
    public class SindicateController : Controller
    {
        Sindicate sindicate = new Sindicate();

        // GET: YMMHRSystem/Sindicate
        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("SINDICATE.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveSindicate"] = Permission.QueryPermission("SINDICATE.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanDeleteSindicate"] = Permission.QueryPermission("SINDICATE.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View("~/Areas/YMMHRSystem/Views/Sindicate/Index.cshtml", sindicate.LoadMultiple());
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
        /// Load view with new/exisiting Sindicate.
        /// </summary>
        /// <param name="sindicateId"></param>
        /// <returns></returns>
        public ActionResult LoadSindicate(long sindicateId)
        {
            try
            {
                Session["LoadedSindicateId"] = sindicateId;

                DtoSindicate dtoSindicate = sindicate.Load(sindicateId);

                return View("~/Areas/YMMHRSystem/Views/Sindicate/SindicateDetailDialog.cshtml", dtoSindicate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Sindicate.
        /// </summary>
        /// <param name="dtoSindicate"></param>
        /// <returns></returns>
        public ActionResult SaveSindicate(DtoSindicate dtoSindicate)
        {

            try
            {
                dtoSindicate.SindicateId = Convert.ToInt64(Session["LoadedSindicateId"]);
                if (dtoSindicate.SindicateId == 0)
                {
                    if (sindicate.GetSindicateId(dtoSindicate.Issue) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }

                sindicate.Save(dtoSindicate);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a Sindicate. 
        /// </summary>
        /// <param name="sindicateId"></param>
        /// <returns></returns>
        public string DeleteSindicate(long sindicateId)
        {
            try
            {
                return sindicate.DeleteSindicate(sindicateId) ? "true" : "false";
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
                return View("~/Areas/YMMHRSystem/Views/Sindicate/AttachmentDialog.cshtml");
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
        public ActionResult AttachmentListDialog(long sindicateId)
        {
            try
            {
                Session["LoadedSindicateId"] = sindicateId;
                List<DtoSindicateAttachment> dtoAttachmentList = sindicate.LoadMultipleAttachments(sindicateId);
                return View("~/Areas/YMMHRSystem/Views/Sindicate/SindicateAttachmentList.cshtml", dtoAttachmentList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Adds a Sindicate Attachment. 
        /// </summary>
        /// <param name="SindicateFileId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddAttachment(HttpPostedFileBase file)
        {
            try
            {
                DtoSindicateAttachment dtoSindicateAttachment = new DtoSindicateAttachment();
                dtoSindicateAttachment.SindicateId = Convert.ToInt64(Session["LoadedSindicateId"]);

                if (file != null)
                {
                    string checkextension = Path.GetExtension(file.FileName).ToLower();
                    if (checkextension == ".pdf")
                    {
                        string path = Server.MapPath("~/SindicateFiles/Attachments/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        dtoSindicateAttachment.FileName = path + file.FileName;
                        file.SaveAs(path + file.FileName);
                    }
                    else
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }

                }
                return sindicate.AddAttachment(dtoSindicateAttachment) ? Json("true", JsonRequestBehavior.AllowGet) : Json("false", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Removes a attachment file. 
        /// </summary>
        /// <param name="sindicateAttachmentId"></param>
        /// <returns></returns>
        public string DeleteAttachmentFile(long sindicateAttachmentId)
        {
            try
            {
                string fileName = sindicate.GetSindicateAttachmentFile(sindicateAttachmentId);
                return sindicate.DeleteAttachment(sindicateAttachmentId, fileName) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Get attachment file
        /// </summary>
        /// <param name="sindicateAttachmentId"></param>
        /// <returns>file</returns>
        public ActionResult GetAttachmentPDF(long sindicateAttachmentId)
        {
            try
            {
                string file = sindicate.GetSindicateAttachmentFile(sindicateAttachmentId);
                return File(file, "application/pdf");
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ContractDialog()
        {
            try
            {
                return View("~/Areas/YMMHRSystem/Views/Sindicate/ContractDialog.cshtml");
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
        public ActionResult ContractListDialog()
        {
            try
            {
                List<DtoSindicateContract> dtoContractList = sindicate.LoadMultipleContracts();
                return View("~/Areas/YMMHRSystem/Views/Sindicate/SindicateContractList.cshtml", dtoContractList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Adds a Sindicate Contract. 
        /// </summary>
        /// <param name="SindicateFileId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddContract(HttpPostedFileBase file)
        {
            try
            {
                DtoSindicateContract dtoSindicateContract = new DtoSindicateContract();

                if (file != null)
                {
                    string checkextension = Path.GetExtension(file.FileName).ToLower();
                    if (checkextension == ".pdf")
                    {
                        string path = Server.MapPath("~/SindicateFiles/Contracts/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        dtoSindicateContract.FileName = path + file.FileName;
                        dtoSindicateContract.DateAdded = DateTime.Now;
                        file.SaveAs(path + file.FileName);
                    }
                    else
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }

                }
                return sindicate.AddContract(dtoSindicateContract) ? Json("true", JsonRequestBehavior.AllowGet) : Json("false", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Removes a attachment file. 
        /// </summary>
        /// <param name="sindicateContractId"></param>
        /// <returns></returns>
        public string DeleteContractFile(long sindicateContractId)
        {
            try
            {
                string fileName = sindicate.GetSindicateContractFile(sindicateContractId);
                return sindicate.DeleteContract(sindicateContractId, fileName) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Get attachment file
        /// </summary>
        /// <param name="sindicateContractId"></param>
        /// <returns>file</returns>
        public ActionResult GetContractPDF(long sindicateContractId)
        {
            try
            {
                string file = sindicate.GetSindicateContractFile(sindicateContractId);
                return File(file, "application/pdf");
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
