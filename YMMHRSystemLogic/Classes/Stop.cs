using DBFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class Stop
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        #region Standard Methods
        /// <summary>
        /// Loads the Stop DTO
        /// </summary>
        /// <param name="stopId"></param>
        /// <returns>DtoStop Loaded</returns>
        public DtoStop Load(long stopId)
        {
            DtoStop Stop = new DtoStop();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (stopId != 0)
                {
                    mapping.Load<DtoStop>("Stops", new DtoStop(), "StopId=" + stopId);
                    Stop = (DtoStop)mapping.dtoList.FirstOrDefault().Dto;
                }

                return Stop;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Stop", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Register Stop 
        /// </summary>
        /// <param name="Stop"></param>
        /// <returns>Stop registered</returns>
        public void Save(DtoStop Stop)
        {
            try
            {
                User user = new User();

                if (Stop.StopId == 0)
                {
                    Stop.UserCreated = user.GetUserName(SQLTools.userId.ToString());
                    Stop.DateAdded = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = Stop, TableName = "Stops" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Stop", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Stop with fields.
        /// </summary>
        /// <returns>Load Stop Dto</returns>
        public List<DtoStop> LoadMultiple()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoStop> shiftList = new List<DtoStop>();

                mapping.Load<DtoStop>("SELECT StopId, Name, Reference, UserCreated, DateAdded FROM Stops ORDER BY StopId", "Stops", new DtoStop());
                shiftList.AddRange(mapping.dtoList.Select(renglon => (DtoStop)renglon.Dto));

                return shiftList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Stops", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        #endregion

        #region General Methods    
        /// <summary>
        /// Get Stop Id
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public long GetStopId(string name)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT StopId FROM Stops WHERE Name = '" + name + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get StopId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["StopId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Stop Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetStopId");
                throw ex;
            }

        }
        /// <summary>
        /// Deletes a Stop. 
        /// </summary>
        /// <param name="stopId"></param>
        /// <returns></returns>
        public bool DeleteStop(long stopId)
        {
            try
            {
                string query = "DELETE Stops WHERE StopId =" + stopId;
                oDatabase.ExecuteNonQuery(query, "Remove Stop");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Stop", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteStop");
                return false;
            }

        }
        /// <summary>
        /// Gets the stop reference
        /// </summary>
        /// <param name="stopId"></param>
        /// <returns></returns>
        public string GetReference(long stopId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT Reference FROM Stops WHERE StopId=" + stopId;

                dataRow = oDatabase.GetRow(sqlString, "Get Stop Reference");

                if (dataRow == null) return "";

                return dataRow["Reference"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
