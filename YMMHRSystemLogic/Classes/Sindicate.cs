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
    public class Sindicate
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        #region Standard Methods
        /// <summary>
        /// Loads the Sindicate DTO
        /// </summary>
        /// <param name="sindicateId"></param>
        /// <returns>DtoSindicate Loaded</returns>
        public DtoSindicate Load(long sindicateId)
        {
            DtoSindicate sindicate = new DtoSindicate();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (sindicateId != 0)
                {
                    mapping.Load<DtoSindicate>("Sindicate", new DtoSindicate(), "SindicateId=" + sindicateId);
                    sindicate = (DtoSindicate)mapping.dtoList.FirstOrDefault().Dto;
                }

                sindicate.AttachmentList = LoadMultipleAttachments(sindicate.SindicateId);

                return sindicate;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Sindicate", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Register Sindicate 
        /// </summary>
        /// <param name="sindicate"></param>
        /// <returns>Sindicate registered</returns>
        public void Save(DtoSindicate sindicate)
        {
            try
            {
                User user = new User();

                if (sindicate.SindicateId == 0)
                {
                    sindicate.CreatedBy = user.GetUserName(SQLTools.userId.ToString());
                    sindicate.Date = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = sindicate, TableName = "Sindicate" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Sindicate", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Sindicate with fields.
        /// </summary>
        /// <returns>Load Sindicate Dto</returns>
        public List<DtoSindicate> LoadMultiple()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoSindicate> sindicateList = new List<DtoSindicate>();

                mapping.Load<DtoSindicate>("SELECT SindicateId, AdministrativeSupport, UnionFee, CreatedBy, Issue, Date FROM Sindicate ORDER BY SindicateId", "Sindicate", new DtoSindicate());
                sindicateList.AddRange(mapping.dtoList.Select(renglon => (DtoSindicate)renglon.Dto));

                return sindicateList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Sindicate", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Sindicate with fields.
        /// </summary>
        /// <returns>Load Sindicate Dto</returns>
        public List<DtoSindicateContract> LoadMultipleContracts()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoSindicateContract> sindicateList = new List<DtoSindicateContract>();

                mapping.Load<DtoSindicateContract>("SELECT SindicateContractId, FileName, DateAdded FROM SindicateContracts ORDER BY SindicateContractId", "SindicateContracts", new DtoSindicateContract());
                sindicateList.AddRange(mapping.dtoList.Select(renglon => (DtoSindicateContract)renglon.Dto));

                return sindicateList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Sindicate Contracts", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleContracts");
                throw ex;
            }
        }
        #endregion

        #region General Methods    
        /// <summary>
        /// Get Sindicate Id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public long GetSindicateId(string issue)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT SindicateId FROM Sindicate WHERE Issue = '" + issue + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get SindicateId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["SindicateId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Sindicate Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetSindicateId");
                throw ex;
            }

        }
        /// <summary>
        /// Get Sindicate Issue
        /// </summary>
        /// <param name="sindicateId"></param>
        /// <returns></returns>
        public string GetSindicateIssue(long sindicateId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT Issue FROM Sindicate WHERE SindicateId = " + sindicateId;

                dataRow = oDatabase.GetRow(sqlString, "Get Sindicate Issue");

                if (dataRow == null) return "";

                return dataRow["Issue"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Sindicate Issue", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetSindicateIssue");
                throw ex;
            }

        }
        /// <summary>
        /// Deletes a Sindicate. 
        /// </summary>
        /// <param name="sindicateId"></param>
        /// <returns></returns>
        public bool DeleteSindicate(long sindicateId)
        {
            try
            {
                string query = "DELETE Sindicate WHERE SindicateId =" + sindicateId;
                oDatabase.ExecuteNonQuery(query, "Remove Sindicate");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Sindicate", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteSindicate");
                return false;
            }

        }
        /// <summary>
        /// Adds an attachment 
        /// </summary>
        /// <param name="dtoSindicateAttachment"></param>
        /// <returns></returns>
        public bool AddAttachment(DtoSindicateAttachment dtoSindicateAttachment)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = dtoSindicateAttachment, TableName = "SindicateAttachments" });

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
        /// Get Sindicate Attachment File 
        /// </summary>
        /// <returns>File name</returns>
        public string GetSindicateAttachmentFile(long sindicateAttachmentId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT FileName FROM SindicateAttachments WHERE SindicateAttachmentId = " + sindicateAttachmentId;

                dataRow = oDatabase.GetRow(sqlString, "Get File Name");

                if (dataRow == null) return "";

                return dataRow["FileName"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Sindicate Attachment File", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetSindicateAttachmentFile");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Attachments
        /// </summary>
        /// <param name="sindicateId"></param>
        /// <returns></returns>
        public List<DtoSindicateAttachment> LoadMultipleAttachments(long sindicateId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoSindicateAttachment> attachmentList = new List<DtoSindicateAttachment>();

                mapping.Load<DtoSindicateAttachment>("SELECT SindicateAttachmentId, SindicateId, FileName FROM SindicateAttachments WHERE SindicateId = " + sindicateId + " ORDER BY SindicateAttachmentId", "SindicateAttachments", new DtoSindicateAttachment());
                attachmentList.AddRange(mapping.dtoList.Select(renglon => (DtoSindicateAttachment)renglon.Dto));

                return attachmentList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Sindicate Attachment", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleAttachments");
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

                string query = "DELETE SindicateAttachments WHERE SindicateAttachmentId =" + attachmentId;
                oDatabase.ExecuteNonQuery(query, "Remove Attachment");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Attachment", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteAttachment");
                return false;
            }
        }
        public bool AddContract(DtoSindicateContract dtoSindicateContract)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = dtoSindicateContract, TableName = "SindicateContracts" });

                mapping.Save();

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Add Contract", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "AddAContract");
                return false;
            }

        }
        /// <summary>
        /// Get Sindicate Contract File 
        /// </summary>
        /// <returns>File name</returns>
        public string GetSindicateContractFile(long sindicateContractId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT FileName FROM SindicateContracts WHERE SindicateContractId = " + sindicateContractId;

                dataRow = oDatabase.GetRow(sqlString, "Get File Name");

                if (dataRow == null) return "";

                return dataRow["FileName"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Sindicate Contract File", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetSindicateContractFile");
                throw ex;
            }
        }
        /// <summary>
        /// Deletes an Contract. 
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public bool DeleteContract(long contractId, string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                string query = "DELETE SindicateContracts WHERE SindicateContractId =" + contractId;
                oDatabase.ExecuteNonQuery(query, "Remove Contract");

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
