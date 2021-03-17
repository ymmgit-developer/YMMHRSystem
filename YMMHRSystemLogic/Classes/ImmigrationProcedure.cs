using DBFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMMHRSystemLogic;

namespace YMMHRSystemLogic
{
    public class ImmigrationProcedure
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        SendEmail sendEmail = new SendEmail();
        EmailNotification emailNotification = new EmailNotification();

        #region Standard Methods
        /// <summary>
        /// Loads the ImmigrationProcedure DTO
        /// </summary>
        /// <param name="immigrationProcedureId"></param>
        /// <returns>DtoImmigrationProcedure Loaded</returns>
        public DtoImmigrationProcedure Load(long immigrationProcedureId)
        {
            DtoImmigrationProcedure immigrationProcedure = new DtoImmigrationProcedure();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (immigrationProcedureId != 0)
                {
                    mapping.Load<DtoImmigrationProcedure>("ImmigrationProcedures", new DtoImmigrationProcedure(), "ImmigrationProcedureId=" + immigrationProcedureId);
                    immigrationProcedure = (DtoImmigrationProcedure)mapping.dtoList.FirstOrDefault().Dto;
                }

                return immigrationProcedure;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load ImmigrationProcedure", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Register ImmigrationProcedure 
        /// </summary>
        /// <param name="immigrationProcedure"></param>
        /// <returns>ImmigrationProcedure registered</returns>
        public void Save(DtoImmigrationProcedure immigrationProcedure)
        {
            try
            {
                User user = new User();

                if (immigrationProcedure.ImmigrationProcedureId == 0)
                {
                    immigrationProcedure.Date = DateTime.Now;
                    immigrationProcedure.Status = 1;
                    immigrationProcedure.CreatedBy = user.GetUserName(SQLTools.userId.ToString());

                    sendEmail.SendEmailTemplate("YMM HR System: Immigration Procedure Request", "TemplateImmigrationProcedureRequest", new[,]
                    {
                        {"$APPLICANT$", immigrationProcedure.CreatedBy},
                        {"$ASSOCIATE$", immigrationProcedure.Associate},
                        {"$PROCEDURE$", immigrationProcedure.ProcedureType},
                        {"$DOCUMENT$", immigrationProcedure.DocumentType},
                        {"$DATE$", immigrationProcedure.ExpirationDate.ToString("dd/MM/yyyy")}
                    }, sendEmail.GetAdminEmail(), emailNotification.GetLegalAffairContacts(1).Split(',').ToList());
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = immigrationProcedure, TableName = "ImmigrationProcedures" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save ImmigrationProcedure", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple ImmigrationProcedure with fields.
        /// </summary>
        /// <returns>Load ImmigrationProcedure Dto</returns>
        public List<DtoImmigrationProcedure> LoadMultiple()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoImmigrationProcedure> immigrationProcedureList = new List<DtoImmigrationProcedure>();

                mapping.Load<DtoImmigrationProcedure>("SELECT ImmigrationProcedureId, Associate, Process, DocumentType, Motive, ProcedureType, ExpirationDate, Justification, RejectionMotive, Date, CreatedBy, Status FROM ImmigrationProcedures ORDER BY ImmigrationProcedureId", "ImmigrationProcedure", new DtoImmigrationProcedure());
                immigrationProcedureList.AddRange(mapping.dtoList.Select(renglon => (DtoImmigrationProcedure)renglon.Dto));

                return immigrationProcedureList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple ImmigrationProcedure", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        #endregion

        #region General Methods    
        /// <summary>
        /// Get ImmigrationProcedure Id
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public long GetImmigrationProcedureId(string name)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT ImmigrationProcedureId FROM ImmigrationProcedures WHERE Name = '" + name + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get ImmigrationProcedureId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["ImmigrationProcedureId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get ImmigrationProcedure Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetImmigrationProcedureId");
                throw ex;
            }

        }
        /// <summary>
        /// Deletes a ImmigrationProcedure. 
        /// </summary>
        /// <param name="immigrationProcedureId"></param>
        /// <returns></returns>
        public bool DeleteImmigrationProcedure(long immigrationProcedureId)
        {
            try
            {
                string query = "DELETE ImmigrationProcedures WHERE ImmigrationProcedureId =" + immigrationProcedureId;
                oDatabase.ExecuteNonQuery(query, "Remove ImmigrationProcedure");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete ImmigrationProcedure", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteImmigrationProcedure");
                return false;
            }

        }
        /// <summary>
        /// Changes Immigration Procedure Status
        /// </summary>
        /// <param name="status"></param>
        /// <param name="immigrationProcedureId"></param>
        public void ChangeImmigrationProcedureStatus(int status, long immigrationProcedureId, string rejectionMotive = "")
        {
            try
            {
                string query = "";
                if (string.IsNullOrEmpty(rejectionMotive))
                {
                     query = "UPDATE ImmigrationProcedures SET Status = " + status + " WHERE ImmigrationProcedureId =" + immigrationProcedureId;
                }
                else
                {
                    query = "UPDATE ImmigrationProcedures SET Status = " + status + ", RejectionMotive = '" + rejectionMotive + "' WHERE ImmigrationProcedureId =" + immigrationProcedureId;
                }
                
                oDatabase.ExecuteNonQuery(query, "Change Immigration Procedure Status");
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Change Immigration Procedure Status", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "ChangeImmigrationProcedureStatus");
            }
        }
        #endregion
    }
}
