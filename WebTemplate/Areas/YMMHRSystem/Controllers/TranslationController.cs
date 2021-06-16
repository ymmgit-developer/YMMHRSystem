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
    public class TranslationController : Controller
    {
        Translation translation = new Translation();
        SendEmail sendEmail = new SendEmail();
        EmailNotification emailNotification = new EmailNotification();

        // GET: YMMHRSystem/Translation
        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("TRANSLATION.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveTranslation"] = Permission.QueryPermission("TRANSLATION.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanDeleteTranslation"] = Permission.QueryPermission("TRANSLATION.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    if (Role.QueryRole("GA", Convert.ToInt64(Session["UserId"])) || Role.QueryRole("Admin", Convert.ToInt64(Session["UserId"])))
                    {
                        return View("~/Areas/YMMHRSystem/Views/Translation/Index.cshtml", translation.LoadMultiple());
                    }
                    else
                    {
                        return View("~/Areas/YMMHRSystem/Views/Translation/Index.cshtml", translation.LoadMultiple(Session["UserName"].ToString()));
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
        /// Load view with new/exisiting Translation.
        /// </summary>
        /// <param name="translationId"></param>
        /// <returns></returns>
        public ActionResult LoadTranslation(long translationId)
        {
            try
            {
                Session["LoadedTranslationId"] = translationId;

                DtoTranslation dtoTranslation = translation.Load(translationId);

                return View("~/Areas/YMMHRSystem/Views/Translation/TranslationDetailDialog.cshtml", dtoTranslation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Translation.
        /// </summary>
        /// <param name="dtoTranslation"></param>
        /// <returns></returns>
        public ActionResult SaveTranslation(DtoTranslation dtoTranslation)
        {

            try
            {
                dtoTranslation.TranslationId = Convert.ToInt64(Session["LoadedTranslationId"]);
                if (dtoTranslation.TranslationId == 0)
                {
                    dtoTranslation.Associate = Session["UserName"].ToString();
                }              
                translation.Save(dtoTranslation, Convert.ToInt64(Session["UserId"]));
                Session["LoadedTranslationId"] = dtoTranslation.TranslationId;
                string data = dtoTranslation.TranslationId.ToString() + "|" + dtoTranslation.Associate + "|" + dtoTranslation.Status + "|" + dtoTranslation.DateAdded;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a Translation. 
        /// </summary>
        /// <param name="translationId"></param>
        /// <returns></returns>
        public string DeleteTranslation(long translationId)
        {
            try
            {
                return translation.DeleteTranslation(translationId) ? "true" : "false";
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
        public ActionResult AttachmentDialog(bool isTranslated)
        {
            try
            {
                DtoTranslationAttachment dtoTranslationAttachment = new DtoTranslationAttachment();
                dtoTranslationAttachment.IsTranslated = isTranslated;
                return View("~/Areas/YMMHRSystem/Views/Translation/AttachmentDialog.cshtml", dtoTranslationAttachment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Adds a Translation Attachment. |
        /// </summary>
        /// <param name="TranslationFileId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddAttachment(DtoTranslationAttachment dtoTranslationAttachment, HttpPostedFileBase file)
        {
            try
            {
                dtoTranslationAttachment.TranslationId = Convert.ToInt64(Session["LoadedTranslationId"]);

                if (translation.GetTranslationStatus(Convert.ToInt64(Session["LoadedTranslationId"])) != 3)
                {
                    if (file != null)
                    {
                        string path = "";
                        if (dtoTranslationAttachment.IsTranslated)
                        {
                            path = Server.MapPath("~/TranslationFiles/Translations/" + dtoTranslationAttachment.TranslationId + "/");
                        }
                        else
                        {
                            path = Server.MapPath("~/TranslationFiles/Attachments/" + dtoTranslationAttachment.TranslationId + "/");
                        }

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        dtoTranslationAttachment.FileName = path + file.FileName;
                        file.SaveAs(path + file.FileName);

                    }
                    if (translation.AddAttachment(dtoTranslationAttachment))
                    {
                        translation.UpdateTranslationAttachmentJapaneseFields(dtoTranslationAttachment.FileName, dtoTranslationAttachment.TranslationAttachmentId);
                        DtoTranslation dtoTranslation = translation.Load(dtoTranslationAttachment.TranslationId);
                        if (dtoTranslationAttachment.IsTranslated)
                        {
                            return View("~/Areas/YMMHRSystem/Views/Translation/TranslationList.cshtml", dtoTranslation.TranslatedList);
                        }
                        else
                        {
                            return View("~/Areas/YMMHRSystem/Views/Translation/AttachmentList.cshtml", dtoTranslation.AttachmentList);
                        }
                    }
                }

                return Json("false", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Removes a attachment file. 
        /// </summary>
        /// <param name="translationAttachmentId"></param>
        /// <returns></returns>
        public string DeleteAttachmentFile(long translationAttachmentId)
        {
            try
            {
                if (translation.GetTranslationStatus(Convert.ToInt64(Session["LoadedTranslationId"])) != 3)
                {
                    string fileName = translation.GetTranslationAttachmentFile(translationAttachmentId);
                    return translation.DeleteAttachment(translationAttachmentId, fileName) ? "true" : "false";
                }
                else
                {
                    return "false";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Get attachment file
        /// </summary>
        /// <param name="translationAttachmentId"></param>
        /// <returns>file</returns>
        public ActionResult GetAttachment(long translationAttachmentId)
        {
            try
            {
                string file = translation.GetTranslationAttachmentFile(translationAttachmentId);
                return File(file, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(file));
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        /// <summary>
        /// Starts a translation
        /// </summary>
        /// <returns></returns>
        public ActionResult StartTranslation(DtoTranslation dtoTranslation)
        {
            try
            {
                User user = new User();
                dtoTranslation.AssignedTo = Session["UserName"].ToString();
                dtoTranslation.Status = 2;
                translation.Save(dtoTranslation);

                List<string> contacts = emailNotification.GetTranslationContacts(1).Split(',').ToList();
                contacts.Add(user.GetUserEmail(dtoTranslation.CreatedBy));
                sendEmail.SendEmailTemplate("YMM HR System: Translation Start", "TemplateTranslationStart", new[,]
                    {
                        {"$APPLICANT$", dtoTranslation.Associate},
                        {"$ISSUE$", dtoTranslation.Issue},
                        {"$DATE$", dtoTranslation.Deadline?.ToString("dd/MM/yyyy")}
                    }, sendEmail.GetAdminEmail(), contacts);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Ends a Translation.
        /// </summary>
        /// <returns></returns>
        public ActionResult EndTranslation(DtoTranslation dtoTranslation)
        {
            try
            {
                User user = new User();
                var translationsList = translation.LoadMultipleAttachments(dtoTranslation.TranslationId, true, true);
                if (translationsList.Count > 0)
                {
                    dtoTranslation.Status = 3;
                    translation.Save(dtoTranslation);
                    List<string> contacts = emailNotification.GetTranslationContacts(1).Split(',').ToList();
                    contacts.Add(user.GetUserEmail(dtoTranslation.CreatedBy));
                    sendEmail.SendEmailTemplate("YMM HR System: Translation Complete", "TemplateTranslationFinish", new[,]
                    {
                        {"$APPLICANT$", dtoTranslation.Associate},
                        {"$ISSUE$", dtoTranslation.Issue},
                        {"$DATE$", dtoTranslation.Deadline?.ToString("dd/MM/yyyy")}
                    }, sendEmail.GetAdminEmail(), contacts);
                }
                else
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
