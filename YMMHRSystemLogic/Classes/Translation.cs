using DBFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMMHRSystemLogic;

namespace YMMHRSystemLogic
{
    public class Translation
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        SendEmail sendEmail = new SendEmail();
        EmailNotification emailNotification = new EmailNotification();

        #region Standard Methods
        /// <summary>
        /// Loads the Translation DTO
        /// </summary>
        /// <param name="translationId"></param>
        /// <returns>DtoTranslation Loaded</returns>
        public DtoTranslation Load(long translationId)
        {
            DtoTranslation translation = new DtoTranslation();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (translationId != 0)
                {
                    mapping.Load<DtoTranslation>("Translations", new DtoTranslation(), "TranslationId=" + translationId);
                    translation = (DtoTranslation)mapping.dtoList.FirstOrDefault().Dto;
                }

                translation.AttachmentList = LoadMultipleAttachments(translation.TranslationId, true, false);
                translation.TranslatedList = LoadMultipleAttachments(translation.TranslationId, true, true);

                return translation;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Translation", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Register Translation 
        /// </summary>
        /// <param name="translation"></param>
        /// <returns>Translation registered</returns>
        public void Save(DtoTranslation translation)
        {
            try
            {
                User user = new User();
                if (translation.TranslationId == 0)
                {
                    translation.Associate = user.GetUserName(SQLTools.userId.ToString());
                    translation.DateAdded = DateTime.Now;
                    translation.Status = 1;
                    sendEmail.SendEmailTemplate("YMM HR System: Translation Request", "TemplateTranslationRequest", new[,]
                    {
                        {"$APPLICANT$", translation.Associate},
                        {"$ISSUE$", translation.Issue},
                        {"$DATE$", translation.Deadline?.ToString("dd/MM/yyyy")}
                    }, sendEmail.GetAdminEmail(), emailNotification.GetTranslationContacts(1).Split(',').ToList());
                }

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = translation, TableName = "Translations" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Translation", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Translation with fields.
        /// </summary>
        /// <returns>Load Translation Dto</returns>
        public List<DtoTranslation> LoadMultiple()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoTranslation> translationList = new List<DtoTranslation>();

                mapping.Load<DtoTranslation>("SELECT TranslationId, Associate, Issue, Description, ComplexityLevel, Status, AssignedTo, Deadline, FinalDate, DateAdded FROM Translations ORDER BY TranslationId", "Translations", new DtoTranslation());
                translationList.AddRange(mapping.dtoList.Select(renglon => (DtoTranslation)renglon.Dto));

                return translationList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Translation", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<DtoTranslation> LoadMultipleWithFilter(string startDate, string endDate)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoTranslation> translationList = new List<DtoTranslation>();

                mapping.Load<DtoTranslation>("SELECT TranslationId, Associate, Issue, Description, ComplexityLevel, Status, AssignedTo, Deadline, FinalDate, DateAdded FROM Translations WHERE DateAdded BETWEEN '" + startDate + "' AND '" + endDate + "' ORDER BY TranslationId", "Translations", new DtoTranslation());
                translationList.AddRange(mapping.dtoList.Select(renglon => (DtoTranslation)renglon.Dto));

                return translationList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Translation With Filter", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleWithFilter");
                throw ex;
            }
        }
        #endregion

        #region General Methods    
        /// <summary>
        /// Deletes a Translation. 
        /// </summary>
        /// <param name="translationId"></param>
        /// <returns></returns>
        public bool DeleteTranslation(long translationId)
        {
            try
            {
                string query = "DELETE Translations WHERE TranslationId =" + translationId;
                oDatabase.ExecuteNonQuery(query, "Remove Translation");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Translation", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteTranslation");
                return false;
            }

        }
        /// <summary>
        /// Adds an attachment 
        /// </summary>
        /// <param name="dtoTranslationAttachment"></param>
        /// <returns></returns>
        public bool AddAttachment(DtoTranslationAttachment dtoTranslationAttachment)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = dtoTranslationAttachment, TableName = "TranslationAttachments" });

                mapping.Save();

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Add Attachment", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "AddAttachment");
                return false;
            }

        }
        /// <summary>
        /// Get Translation Attachment File 
        /// </summary>
        /// <returns>File name</returns>
        public string GetTranslationAttachmentFile(long translationAttachmentId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT FileName FROM TranslationAttachments WHERE TranslationAttachmentId = " + translationAttachmentId;

                dataRow = oDatabase.GetRow(sqlString, "Get File Name");

                if (dataRow == null) return "";

                return dataRow["FileName"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Translation Attachment File", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetTranslationAttachmentFile");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Attachments
        /// </summary>
        /// <param name="translationId"></param>
        /// <returns></returns>
        public List<DtoTranslationAttachment> LoadMultipleAttachments(long translationId, bool withFilter = false, bool isTranslated = false)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoTranslationAttachment> attachmentList = new List<DtoTranslationAttachment>();
                if (!withFilter)
                {
                    mapping.Load<DtoTranslationAttachment>("SELECT TranslationAttachmentId, TranslationId, FileName FROM TranslationAttachments WHERE TranslationId = " + translationId + " ORDER BY TranslationAttachmentId", "TranslationAttachments", new DtoTranslationAttachment());
                }
                else
                {
                    mapping.Load<DtoTranslationAttachment>("SELECT TranslationAttachmentId, TranslationId, FileName FROM TranslationAttachments WHERE TranslationId = " + translationId + " AND IsTranslated = " + (isTranslated ? "1" : "0") + " ORDER BY TranslationAttachmentId", "TranslationAttachments", new DtoTranslationAttachment());
                }

                attachmentList.AddRange(mapping.dtoList.Select(renglon => (DtoTranslationAttachment)renglon.Dto));

                return attachmentList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Translation Attachment", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleAttachments");
                throw ex;
            }
        }
        /// <summary>
        /// Deletes an Attachment. 
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <returns></returns>
        public bool DeleteAttachment(long attachmentId, string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                string query = "DELETE TranslationAttachments WHERE TranslationAttachmentId =" + attachmentId;
                oDatabase.ExecuteNonQuery(query, "Remove Attachment");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Attachment", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteAttachment");
                return false;
            }
        }
        /// <summary>
        /// Get Translation status
        /// </summary>
        /// <param name="translationId"></param>
        /// <returns></returns>
        public int GetTranslationStatus(long translationId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT Status FROM Translations WHERE TranslationId = " + translationId;

                dataRow = oDatabase.GetRow(sqlString, "Get Translation Status");

                if (dataRow == null) return 0;

                return Convert.ToInt32(dataRow["Status"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Translation Status", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetTranslationStatus");
                throw ex;
            }
        }
        #endregion
    }
}
