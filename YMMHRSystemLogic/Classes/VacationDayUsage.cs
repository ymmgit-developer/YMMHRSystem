using DBFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic.Classes
{
    public class VacationDayUsage
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        public bool Save(DtoVacationDayUsage VacationDayUsage)
        {
            try
            {
                string query = "INSERT INTO VacationDayUsage(VacationId, HistoryAnniversaryId, DaysUsed) VALUES (" + VacationDayUsage.VacationId.ToString() + ", " + VacationDayUsage.HistoryAnniversaryId.ToString() + " ," + VacationDayUsage.DaysUsed.ToString() + ")";
                oDatabase.ExecuteNonQuery(query, "Insert Into VacationDayUsage");
                //DBFrameworkMapping mapping = new DBFrameworkMapping();
                //mapping.dtoList.Add(new DBFrameworkDto() { Dto = VacationDayUsage, TableName = "VacationDayUsage" });
                //mapping.Save();
                return true;
            }
            catch (Exception ex) 
            {
                log.WriteToErrorLog("HR System", "Save Data in VacationDayUsage", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                return false;
            }
        }
    }
}
