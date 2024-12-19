using DBFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace YMMHRSystemLogic
{
    public class HistoryAnniversary
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();

        #region General methods
        public bool UpdateDaysAvailable(int HistoryAnniversaryId, int DaysAvailable)
        {
            try
            {
                string query = "UPDATE HistoryAnniversary SET DaysAvailable = " + DaysAvailable + " WHERE HistoryAnniversaryId = " + HistoryAnniversaryId;
                oDatabase.ExecuteNonQuery(query, "Update Days Available For Vacations");
                return true;
            }
            catch(Exception ex)
            {
                log.WriteToErrorLog("HR System", "Update available days", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "UpdateDaysAvailable");
                return false;
            }
        }
        #endregion

        #region Process methods
        public List<DtoHistoryAnniversary> GetAnniversarysAvailable(long WorkerFileId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoHistoryAnniversary> dtoAnniversarysAvailableList = new List<DtoHistoryAnniversary>();
                mapping.Load<DtoHistoryAnniversary>("SELECT * FROM HistoryAnniversary WHERE DaysAvailable > 0 AND WorkerFileId = " + WorkerFileId.ToString() + " ORDER BY 1 ASC", "HistoryAnniversary", new DtoHistoryAnniversary());
                dtoAnniversarysAvailableList.AddRange(mapping.dtoList.Select(renglon => (DtoHistoryAnniversary)renglon.Dto));
                return dtoAnniversarysAvailableList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Anniversary Records With Available Days For Use By User", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetAnniversarysAvailable");
                throw ex;
            }
        }
        #endregion
    }
}
