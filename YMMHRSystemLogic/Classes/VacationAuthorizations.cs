using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using YMMHRSystemLogic;
using DBFramework;
using System.Data.SqlTypes;
using System.Web.Mvc;
using System.Windows.Media;
using System.Xml.Linq;
using YMMHRSystemLogic.Classes;

namespace YMMHRSystemLogic.Classes
{
    public class VacationAuthorizations
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();

        #region General Methods
        public bool Save(DtoVacationAuthorizations vacationAuthorizations)
        {
            try
            {
                string query = "INSERT INTO VacationAuthorizations (WorkerFileId, UserId, DateAdded) VALUES ('" + vacationAuthorizations.WorkerFileId + "', '" + vacationAuthorizations.UserId + "', '" + vacationAuthorizations.DateAdded.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                oDatabase.ExecuteNonQuery(query, "Save Vacation Notification Record");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Vacation Notification Record", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                return false;
            }
        }

        public bool Update(DtoVacationAuthorizations vacationAuthorizations)
        {
            try
            {
                string query = "UPDATE VacationAuthorizations SET WorkerFileId = "+ vacationAuthorizations.WorkerFileId.ToString() + ", UserId = " + vacationAuthorizations.UserId.ToString() + ", DateModification = '" + vacationAuthorizations.DateModification.ToString() + "' WHERE VacationAuthorizationsId = " + vacationAuthorizations.VacationAuthorizationsId.ToString();
                oDatabase.ExecuteNonQuery(query, "Update Vacation Notification Record");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Update Vacation Notification Record", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                return false;
            }
        }

        public bool Delete(long vacationAuthorizationsId)
        {
            try
            {
                string query = "DELETE FROM VacationAuthorizations WHERE VacationAuthorizationsId = " + vacationAuthorizationsId.ToString();
                oDatabase.ExecuteNonQuery(query, "Delete Vacation Notification Record");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Vacation Notification Record", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Delete");
                return false;
            }
        }
        #endregion

        #region Process Methods
        public List<DtoVacationAuthorizations> Load()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoVacationAuthorizations> dtoDtoVacationAuthorizationsList = new List<DtoVacationAuthorizations>();
                mapping.Load<DtoVacationAuthorizations>("SELECT * FROM VacationAuthorizations", "VacationAuthorizations", new DtoVacationAuthorizations());
                dtoDtoVacationAuthorizationsList.AddRange(mapping.dtoList.Select(renglon => (DtoVacationAuthorizations)renglon.Dto));
                return dtoDtoVacationAuthorizationsList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Vacation Authorizations Records", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }

        public List<DtoVacationAuthorizations> GetApprovers(long WorkerFileId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoVacationAuthorizations> dtoDtoVacationAuthorizationsList = new List<DtoVacationAuthorizations>();
                mapping.Load<DtoVacationAuthorizations>("SELECT UserId FROM VacationAuthorizations WHERE WorkerFileId =  " + WorkerFileId.ToString(), "VacationAuthorizations", new DtoVacationAuthorizations());
                dtoDtoVacationAuthorizationsList.AddRange(mapping.dtoList.Select(renglon => (DtoVacationAuthorizations)renglon.Dto));
                return dtoDtoVacationAuthorizationsList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Vacation Authorizations Records by WorkerField", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "NotifyTheApprover");
                throw ex;
            }
        }

        public List<DtoVacationAuthorizations> GetListWorkersbyApprover(long UserId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoVacationAuthorizations> dtoDtoVacationAuthorizationsList = new List<DtoVacationAuthorizations>();
                mapping.Load<DtoVacationAuthorizations>("SELECT WorkerFileId FROM VacationAuthorizations WHERE UserId =  " + UserId.ToString(), "VacationAuthorizations", new DtoVacationAuthorizations());
                dtoDtoVacationAuthorizationsList.AddRange(mapping.dtoList.Select(renglon => (DtoVacationAuthorizations)renglon.Dto));
                return dtoDtoVacationAuthorizationsList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Vacation Authorizations Records by WorkerField", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "NotifyTheApprover");
                throw ex;
            }
        }
        #endregion
    }
}
