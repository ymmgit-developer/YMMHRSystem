using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using YMMHRSystemLogic;
using DBFramework;

namespace YMMHRSystemLogic
{
    
    public class DatesForVacations
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();

        #region Standard Methods
        public bool Save(DtoDatesForVacations date)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = date, TableName = "DatesForVacations" });

                mapping.Save();
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Dates for Vacations", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                return false;
            }
        }
        #endregion

        #region Process Methods
        public List<DtoDatesForVacations> GetDayesVacationRequestsbyVacationId(long VacationId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoDatesForVacations> datesForVacations = new List<DtoDatesForVacations>();
                mapping.Load<DtoDatesForVacations>("SELECT * FROM DatesForVacations WHERE VacationId = " + VacationId, "DatesForVacations", new DtoDatesForVacations());
                datesForVacations.AddRange(mapping.dtoList.Select(renglon => (DtoDatesForVacations)renglon.Dto));
                return datesForVacations;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Dates for Vacations", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        #endregion
    }
}
