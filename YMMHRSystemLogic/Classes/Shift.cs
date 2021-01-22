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
    public class Shift
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        #region Standard Methods
        /// <summary>
        /// Loads the Shift DTO
        /// </summary>
        /// <param name="shiftId"></param>
        /// <returns>DtoShift Loaded</returns>
        public DtoShift Load(long shiftId)
        {
            DtoShift shift = new DtoShift();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (shiftId != 0)
                {
                    mapping.Load<DtoShift>("Shifts", new DtoShift(), "ShiftId=" + shiftId);
                    shift = (DtoShift)mapping.dtoList.FirstOrDefault().Dto;
                }

                return shift;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Shift", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Register Shift 
        /// </summary>
        /// <param name="shift"></param>
        /// <returns>Shift registered</returns>
        public void Save(DtoShift shift)
        {
            try
            {
                User user = new User();

                if (shift.ShiftId == 0)
                {
                    shift.UserCreated = user.GetUserName(SQLTools.userId.ToString());
                    shift.DateAdded = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = shift, TableName = "Shifts" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Shift", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Shift with fields.
        /// </summary>
        /// <returns>Load Shift Dto</returns>
        public List<DtoShift> LoadMultiple()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoShift> shiftList = new List<DtoShift>();

                mapping.Load<DtoShift>("SELECT ShiftId, Name, Symbol, UserCreated, DateAdded FROM Shifts ORDER BY ShiftId", "Shift", new DtoShift());
                shiftList.AddRange(mapping.dtoList.Select(renglon => (DtoShift)renglon.Dto));

                return shiftList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Shift", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        #endregion

        #region General Methods    
        /// <summary>
        /// Get Shift Id
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public long GetShiftId(string name)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT ShiftId FROM Shifts WHERE Name = '" + name + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get ShiftId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["ShiftId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Shift Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetShiftId");
                throw ex;
            }

        }
        /// <summary>
        /// Deletes a Shift. 
        /// </summary>
        /// <param name="shiftId"></param>
        /// <returns></returns>
        public bool DeleteShift(long shiftId)
        {
            try
            {
                string query = "DELETE Shifts WHERE ShiftId =" + shiftId;
                oDatabase.ExecuteNonQuery(query, "Remove Shift");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Shift", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteShift");
                return false;
            }

        }
        #endregion
    }
}
