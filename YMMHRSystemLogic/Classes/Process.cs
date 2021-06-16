using DBFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class Process
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        #region Standard Methods
        /// <summary>
        /// Loads the Process DTO
        /// </summary>
        /// <param name="processId"></param>
        /// <returns>DtoProcess Loaded</returns>
        public DtoProcess Load(long processId)
        {
            DtoProcess process = new DtoProcess();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (processId != 0)
                {
                    mapping.Load<DtoProcess>("Processes", new DtoProcess(), "ProcessId=" + processId);
                    process = (DtoProcess)mapping.dtoList.FirstOrDefault().Dto;
                }

                return process;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Process", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Register Process 
        /// </summary>
        /// <param name="process"></param>
        /// <returns>Process registered</returns>
        public void Save(DtoProcess process)
        {
            try
            {
                User user = new User();

                if (process.ProcessId == 0)
                {
                    process.DateAdded = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = process, TableName = "Processes" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Process", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Process with fields.
        /// </summary>
        /// <returns>Load Process Dto</returns>
        public List<DtoProcess> LoadMultiple()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoProcess> shiftList = new List<DtoProcess>();

                mapping.Load<DtoProcess>("SELECT ProcessId, Name, Code, UserCreated, DateAdded FROM Processes ORDER BY ProcessId", "Processes", new DtoProcess());
                shiftList.AddRange(mapping.dtoList.Select(renglon => (DtoProcess)renglon.Dto));

                return shiftList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Processes", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        #endregion

        #region General Methods    
        /// <summary>
        /// Get Process Id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public long GetProcessId(string name)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT ProcessId FROM Processes WHERE Name = '" + name + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get ProcessId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["ProcessId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Process Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetProcessId");
                throw ex;
            }

        }
        /// <summary>
        /// Get Process name
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public string GetProcessName(long processId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT Name FROM Processes WHERE ProcessId = " + processId;

                dataRow = oDatabase.GetRow(sqlString, "Get Process Id");

                if (dataRow == null) return "";

                return dataRow["Name"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Process Name", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetProcessName");
                throw ex;
            }

        }
        /// <summary>
        /// Deletes a Process. 
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public bool DeleteProcess(long processId)
        {
            try
            {
                string query = "DELETE Processes WHERE ProcessId =" + processId;
                oDatabase.ExecuteNonQuery(query, "Remove Process");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Process", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteProcess");
                return false;
            }

        }
        #endregion
    }
}
