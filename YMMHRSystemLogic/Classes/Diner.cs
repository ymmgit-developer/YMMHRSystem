using DBFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class Diner
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        SendEmail sendEmail = new SendEmail();
        EmailNotification emailNotification = new EmailNotification();

        #region Standard Methods
        /// <summary>
        /// Loads the Diner DTO
        /// </summary>
        /// <param name="dinerId"></param>
        /// <returns>DtoDiner Loaded</returns>
        public DtoDiner Load(long dinerId)
        {
            DtoDiner Diner = new DtoDiner();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (dinerId != 0)
                {
                    mapping.Load<DtoDiner>("Diner", new DtoDiner(), "DinerId=" + dinerId);
                    Diner = (DtoDiner)mapping.dtoList.FirstOrDefault().Dto;
                }

                return Diner;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Diner", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Register Diner 
        /// </summary>
        /// <param name="diner"></param>
        /// <returns>Diner registered</returns>
        public void Save(DtoDiner diner)
        {
            try
            {
                User user = new User();

                if (diner.DinerId == 0)
                {
                    diner.UserCreated = user.GetUserName(SQLTools.userId.ToString());
                    diner.Date = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = diner, TableName = "Diner" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Diner", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Diner with fields.
        /// </summary>
        /// <returns>Load Diner Dto</returns>
        public List<DtoDiner> LoadMultiple()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoDiner> shiftList = new List<DtoDiner>();

                mapping.Load<DtoDiner>("SELECT DinerId, Type, Dishes, UnitPrice, Total, Date, StartDate, FinishDate, UserCreated FROM Diner ORDER BY DinerId", "Diner", new DtoDiner());
                shiftList.AddRange(mapping.dtoList.Select(renglon => (DtoDiner)renglon.Dto));

                return shiftList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Diners", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<DtoDiner> LoadMultipleWithFilter(string startDate, string endDate)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoDiner> dinerList = new List<DtoDiner>();

                mapping.Load<DtoDiner>("SELECT DinerId, Type, Dishes, UnitPrice, Total, Date, StartDate, FinishDate, UserCreated FROM Diner WHERE Date BETWEEN '" + startDate + "' AND '" + endDate + "' ORDER BY DinerId", "Diner", new DtoDiner());
                dinerList.AddRange(mapping.dtoList.Select(renglon => (DtoDiner)renglon.Dto));

                return dinerList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Diners with Filter", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleWithFilter");
                throw ex;
            }
        }

        /// <summary>
        /// Loads the Extra Diner DTO
        /// </summary>
        /// <param name="dinerId"></param>
        /// <returns>DtoDiner Loaded</returns>
        public DtoExtraordinaryDiner LoadExtra(long dinerId)
        {
            DtoExtraordinaryDiner extraDiner = new DtoExtraordinaryDiner();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (dinerId != 0)
                {
                    mapping.Load<DtoExtraordinaryDiner>("ExtraordinaryDiner", new DtoExtraordinaryDiner(), "ExtraordinaryDinerId=" + dinerId);
                    extraDiner = (DtoExtraordinaryDiner)mapping.dtoList.FirstOrDefault().Dto;
                }

                return extraDiner;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Extraordinary Diner", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadExtra");
                throw ex;
            }
        }
        /// <summary>
        /// Register Diner 
        /// </summary>
        /// <param name="extraDiner"></param>
        /// <returns>Diner registered</returns>
        public void SaveExtra(DtoExtraordinaryDiner extraDiner)
        {
            try
            {
                User user = new User();

                if (extraDiner.ExtraordinaryDinerId == 0)
                {
                    extraDiner.UserCreated = user.GetUserName(SQLTools.userId.ToString());
                    extraDiner.DateAdded = DateTime.Now;

                    sendEmail.SendEmailTemplate("YMM HR System: Extra Diner Request", "TemplateExtraDinerRequest", new[,]
{
                        {"$APPLICANT$", extraDiner.UserCreated},
                        {"$TYPE$", extraDiner.Type},
                        {"$ASSOCIATE$", extraDiner.AssociateName},
                        {"$DATE$", extraDiner.Date?.ToString("dd/MM/yyyy")}
                    }, sendEmail.GetAdminEmail(), emailNotification.GetExtraDinerContacts(1).Split(',').ToList());
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = extraDiner, TableName = "ExtraordinaryDiner" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Extra Diner", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveExtra");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Diner with fields.
        /// </summary>
        /// <returns>Load Diner Dto</returns>
        public List<DtoExtraordinaryDiner> LoadMultipleExtra()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoExtraordinaryDiner> extraDinerList = new List<DtoExtraordinaryDiner>();

                mapping.Load<DtoExtraordinaryDiner>("SELECT ExtraordinaryDinerId, AssociateName, Process, Date, Time, Type, Motive, TypeCost, Lading, LadingCost, Motive, DateAdded, UserCreated FROM ExtraordinaryDiner ORDER BY ExtraordinaryDinerId", "ExtraordinaryDiner", new DtoExtraordinaryDiner());
                extraDinerList.AddRange(mapping.dtoList.Select(renglon => (DtoExtraordinaryDiner)renglon.Dto));

                return extraDinerList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Extra Diners", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleExtra");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<DtoExtraordinaryDiner> LoadMultipleExtraWithFilter(string startDate, string endDate)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoExtraordinaryDiner> extraDinerList = new List<DtoExtraordinaryDiner>();

                mapping.Load<DtoExtraordinaryDiner>("SELECT ExtraordinaryDinerId, AssociateName, Process, Date, Time, Type, Motive, Cost, Lading, LadingCost, Motive, DateAdded, UserCreated FROM ExtraordinaryDiner WHERE DateAdded BETWEEN '" + startDate + "' AND '" + endDate + "' ORDER BY ExtraordinaryDinerId", "ExtraordinaryDiner", new DtoExtraordinaryDiner());
                extraDinerList.AddRange(mapping.dtoList.Select(renglon => (DtoExtraordinaryDiner)renglon.Dto));

                return extraDinerList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Extra Diners With Filter", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleExtraWithFilter");
                throw ex;
            }
        }
        #endregion

        #region General Methods    
        /// <summary>
        /// Get Diner Id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public long GetDinerId(string type, DateTime? startDate, DateTime? finishDate)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT DinerId FROM Diner WHERE Type = '" + type + "' AND StartDate = '" + startDate?.ToString("yyyyMMdd") + "' AND FinishDate = '" + finishDate?.ToString("yyyyMMdd") + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get DinerId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["DinerId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Diner Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetDinerId");
                throw ex;
            }

        }
        /// <summary>
        /// Deletes a Diner. 
        /// </summary>
        /// <param name="dinerId"></param>
        /// <returns></returns>
        public bool DeleteDiner(long dinerId)
        {
            try
            {
                string query = "DELETE Diner WHERE DinerId =" + dinerId;
                oDatabase.ExecuteNonQuery(query, "Remove Diner");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Diner", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteDiner");
                return false;
            }

        }

        /// <summary>
        /// Get Extraordinary Diner Id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public long GetExtraDinerId(string associateName, string type, DateTime? date, TimeSpan time)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT ExtraordinaryDinerId FROM ExtraordinaryDiner WHERE AssociateName = '" + associateName + "' AND Type = '" + type + "' AND Date = '" + date?.ToString("yyyyMMdd") + "' AND Time = '" + time + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get Extra Diner Id");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["ExtraordinaryDinerId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Extraordinary Diner Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetExtraDinerId");
                throw ex;
            }

        }
        /// <summary>
        /// Deletes a Extra Diner. 
        /// </summary>
        /// <param name="extraDinerId"></param>
        /// <returns></returns>
        public bool DeleteExtraDiner(long extraDinerId)
        {
            try
            {
                string query = "DELETE ExtraordinaryDiner WHERE ExtraordinaryDinerId =" + extraDinerId;
                oDatabase.ExecuteNonQuery(query, "Remove Extra Diner");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Extraordinary Diner", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteExtraDiner");
                return false;
            }

        }
        #endregion
    }
}
