using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using YMMHRSystemLogic;
using DBFramework;
using System.Data.SqlTypes;

namespace YMMHRSystemLogic
{
    public class Vacations
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();

        #region General methods
        public bool Save(DtoVacations vacation)
        {
            try
            {
                User user = new User();
                if (vacation.VacationId == 0)
                {
                    vacation.DateRequest = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = vacation, TableName = "Vacations" });

                mapping.Save();
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Record Vacations", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                return false;
            }
        }
        public bool Update(DtoVacations vacation)
        {
            try
            {
                return true;
            }
            catch(Exception ex)
            {
                log.WriteToErrorLog("HR System", "Update Record Vacations", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Update");
                return false;
            }
        }
        public bool Delete(long VacationId)
        {
            try
            {
                return true;
            }
            catch(Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Record Vacations", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Delete");
                return false;
            }
        }
        public List<DtoVacations>GetVacationRequests()
        {
            try
            {
                List<DtoVacations> requestsList = new List<DtoVacations>();
                return requestsList;
            }
            catch(Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Records Vacations", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetVacationRequests");
                throw ex;
            }
        }

        public List<DtoVacations>GetVacationRequestsbyUser(long WorkerFileId) 
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoVacations> dtovacationsList = new List<DtoVacations>();
                mapping.Load<DtoVacations>("SELECT * FROM Vacations WHERE WorkerFileId = " + WorkerFileId.ToString(), "Vacations", new DtoVacations());
                dtovacationsList.AddRange(mapping.dtoList.Select(renglon => (DtoVacations)renglon.Dto));
                return dtovacationsList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Records Vacations by User", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetVacationRequestsbyUser");
                throw ex;
            }
        }

        public List<DtoVacations>GetVacationRequestsbyProcess(string ProcessesName)
        {
            try
            {
                List<DtoVacations> requestsList = new List<DtoVacations>();
                return requestsList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Records Vacations", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetVacationRequests");
                throw ex;
            }
        }

        public long GetIdentityVacations()
        {
            try
            {
                DataRow identity;
                string query = "SELECT MAX(VacationId) as LastValue FROM Vacations";
                identity = oDatabase.GetRow(query, "Get Worker ID");
                return long.Parse(identity["LastValue"].ToString());
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Approve Vacations Records", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "ApproveRequestVacation");
                return 0;
            }
        }
        #endregion
        #region Process methods
        public bool ApproveRequestVacation()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Approve Vacations Records", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "ApproveRequestVacation");
                return false;
            }
        }
        #endregion
    }
}
