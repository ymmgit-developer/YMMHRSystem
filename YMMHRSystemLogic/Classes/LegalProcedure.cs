using DBFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class LegalProcedure
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        #region Standard Methods
        /// <summary>
        /// Loads the LegalProcedure DTO
        /// </summary>
        /// <param name="legalProcedureId"></param>
        /// <returns>DtoLegalProcedure Loaded</returns>
        public DtoLegalProcedure Load(long legalProcedureId)
        {
            DtoLegalProcedure legalProcedure = new DtoLegalProcedure();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (legalProcedureId != 0)
                {
                    mapping.Load<DtoLegalProcedure>("LegalProcedures", new DtoLegalProcedure(), "LegalProcedureId=" + legalProcedureId);
                    legalProcedure = (DtoLegalProcedure)mapping.dtoList.FirstOrDefault().Dto;
                }

                return legalProcedure;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Legal Procedure", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Register LegalProcedure 
        /// </summary>
        /// <param name="legalProcedure"></param>
        /// <returns>LegalProcedure registered</returns>
        public void Save(DtoLegalProcedure legalProcedure)
        {
            try
            {
                User user = new User();

                if (legalProcedure.LegalProcedureId == 0)
                {
                    legalProcedure.DateAdded = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = legalProcedure, TableName = "LegalProcedures" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Legal Procedure", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple LegalProcedure with fields.
        /// </summary>
        /// <returns>Load LegalProcedure Dto</returns>
        public List<DtoLegalProcedure> LoadMultiple(string processFilter = "")
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoLegalProcedure> legalProcedureList = new List<DtoLegalProcedure>();
                if (processFilter == "")
                {
                    mapping.Load<DtoLegalProcedure>("SELECT LegalProcedureId, Name, Process, UserCreated, DateAdded FROM LegalProcedures ORDER BY LegalProcedureId", "LegalProcedures", new DtoLegalProcedure());
                }
                else
                {
                    mapping.Load<DtoLegalProcedure>("SELECT LegalProcedureId, Name, Process, UserCreated, DateAdded FROM LegalProcedures WHERE Process = '" + processFilter + "' ORDER BY LegalProcedureId", "LegalProcedures", new DtoLegalProcedure());
                }
               
                legalProcedureList.AddRange(mapping.dtoList.Select(renglon => (DtoLegalProcedure)renglon.Dto));

                return legalProcedureList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Legal Procedures", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        #endregion

        #region General Methods    
        /// <summary>
        /// Get LegalProcedure Id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public long GetLegalProcedureId(string name)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT LegalProcedureId FROM LegalProcedures WHERE Name = '" + name + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get LegalProcedureId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["LegalProcedureId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Legal Procedure Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetLegalProcedureId");
                throw ex;
            }

        }
        /// <summary>
        /// Deletes a LegalProcedure. 
        /// </summary>
        /// <param name="legalProcedureId"></param>
        /// <returns></returns>
        public bool DeleteLegalProcedure(long legalProcedureId)
        {
            try
            {
                string query = "DELETE LegalProcedures WHERE LegalProcedureId =" + legalProcedureId;
                oDatabase.ExecuteNonQuery(query, "Remove LegalProcedure");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Legal Procedure", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteLegalProcedure");
                return false;
            }

        }
        #endregion
    }
}
