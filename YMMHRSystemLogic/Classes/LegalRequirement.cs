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
    public class LegalRequirement
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        #region Standard Methods
        /// <summary>
        /// Loads the LegalRequirement DTO
        /// </summary>
        /// <param name="legalRequirementId"></param>
        /// <returns>DtoLegalRequirement Loaded</returns>
        public DtoLegalRequirement Load(long legalRequirementId)
        {
            DtoLegalRequirement legalRequirement = new DtoLegalRequirement();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (legalRequirementId != 0)
                {
                    mapping.Load<DtoLegalRequirement>("LegalRequirements", new DtoLegalRequirement(), "LegalRequirementId=" + legalRequirementId);
                    legalRequirement = (DtoLegalRequirement)mapping.dtoList.FirstOrDefault().Dto;
                }

                legalRequirement.AttachmentList = LoadMultipleAttachments(legalRequirement.LegalRequirementId);

                return legalRequirement;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load LegalRequirement", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Register LegalRequirement 
        /// </summary>
        /// <param name="legalRequirement"></param>
        /// <returns>LegalRequirement registered</returns>
        public void Save(DtoLegalRequirement legalRequirement)
        {
            try
            {
                User user = new User();

                if (legalRequirement.LegalRequirementId == 0)
                {
                    legalRequirement.UserCreated = user.GetUserName(SQLTools.userId.ToString());
                    legalRequirement.DateAdded = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = legalRequirement, TableName = "LegalRequirements" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save LegalRequirement", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple LegalRequirement with fields.
        /// </summary>
        /// <returns>Load LegalRequirement Dto</returns>
        public List<DtoLegalRequirement> LoadMultiple()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoLegalRequirement> legalRequirementList = new List<DtoLegalRequirement>();

                mapping.Load<DtoLegalRequirement>("SELECT LegalRequirementId, LegalProcedure, Institution, EmissionDate, ExpirationDate, RenewalDate, UserCreated, DateAdded FROM LegalRequirements ORDER BY LegalRequirementId", "LegalRequirements", new DtoLegalRequirement());
                legalRequirementList.AddRange(mapping.dtoList.Select(renglon => (DtoLegalRequirement)renglon.Dto));

                return legalRequirementList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple LegalRequirement", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<DtoLegalRequirement> LoadMultipleWithFilter(string startDate, string endDate)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoLegalRequirement> legalRequirementList = new List<DtoLegalRequirement>();

                mapping.Load<DtoLegalRequirement>("SELECT LegalRequirementId, LegalProcedure, Institution, EmissionDate, ExpirationDate, RenewalDate, UserCreated, DateAdded FROM LegalRequirements WHERE DateAdded BETWEEN '" + startDate + "' AND '" + endDate + "' ORDER BY LegalRequirementId", "LegalRequirements", new DtoLegalRequirement());
                legalRequirementList.AddRange(mapping.dtoList.Select(renglon => (DtoLegalRequirement)renglon.Dto));

                return legalRequirementList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Legal Requirement With Filter", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleWithFilter");
                throw ex;
            }
        }
        #endregion

        #region General Methods    
        /// <summary>
        /// Deletes a LegalRequirement. 
        /// </summary>
        /// <param name="legalRequirementId"></param>
        /// <returns></returns>
        public bool DeleteLegalRequirement(long legalRequirementId)
        {
            try
            {
                string query = "DELETE LegalRequirements WHERE LegalRequirementId =" + legalRequirementId;
                oDatabase.ExecuteNonQuery(query, "Remove LegalRequirement");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Legal Requirement", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteLegalRequirement");
                return false;
            }

        }
        /// <summary>
        /// Adds an attachment 
        /// </summary>
        /// <param name="dtoLegalRequirementAttachment"></param>
        /// <returns></returns>
        public bool AddAttachment(DtoLegalRequirementAttachment dtoLegalRequirementAttachment)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = dtoLegalRequirementAttachment, TableName = "LegalRequirementAttachments" });

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
        /// Get Legal Requirement Attachment File 
        /// </summary>
        /// <returns>File name</returns>
        public string GetLegalRequirementAttachmentFile(long legalRequirementAttachmentId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT FileName FROM LegalRequirementAttachments WHERE LegalRequirementAttachmentId = " + legalRequirementAttachmentId;

                dataRow = oDatabase.GetRow(sqlString, "Get File Name");

                if (dataRow == null) return "";

                return dataRow["FileName"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get LegalRequirement Attachment File", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetLegalRequirementAttachmentFile");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Attachments
        /// </summary>
        /// <param name="legalRequirementId"></param>
        /// <returns></returns>
        public List<DtoLegalRequirementAttachment> LoadMultipleAttachments(long legalRequirementId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoLegalRequirementAttachment> attachmentList = new List<DtoLegalRequirementAttachment>();

                mapping.Load<DtoLegalRequirementAttachment>("SELECT LegalRequirementAttachmentId, LegalRequirementId, FileName FROM LegalRequirementAttachments WHERE LegalRequirementId = " + legalRequirementId + " ORDER BY LegalRequirementAttachmentId", "LegalRequirementAttachments", new DtoLegalRequirementAttachment());
                attachmentList.AddRange(mapping.dtoList.Select(renglon => (DtoLegalRequirementAttachment)renglon.Dto));

                return attachmentList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Legal Requirement Attachment", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleAttachments");
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

                string query = "DELETE LegalRequirementAttachments WHERE LegalRequirementAttachmentId =" + attachmentId;
                oDatabase.ExecuteNonQuery(query, "Remove Attachment");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Attachment", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteAttachment");
                return false;
            }
        }
        #endregion
    }
}
