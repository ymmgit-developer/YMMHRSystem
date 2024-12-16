using DBFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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

                mapping.Load<DtoDiner>("SELECT DinerId, Type, Dishes, UnitPrice, Total, Date, StartDate, FinishDate, UserCreated FROM Diner ORDER BY DinerId DESC", "Diner", new DtoDiner());
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

                mapping.Load<DtoDiner>("SELECT DinerId, Type, Dishes, UnitPrice, Total, Date, StartDate, FinishDate, UserCreated FROM Diner WHERE Date BETWEEN '" + startDate + "' AND '" + endDate + "' ORDER BY DinerId DESC", "Diner", new DtoDiner());
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
        public void SaveExtra(DtoExtraordinaryDiner extraDiner, long userId = 0)
        {
            try
            {
                User user = new User();

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
        /// Register Diner 
        /// </summary>
        /// <param name="extraDiner"></param>
        /// <returns>Diner registered</returns>
        public void SaveMultipleExtra(List<DtoExtraordinaryDiner> extraDinerList, long userId = 0)
        {
            try
            {
                string associateList = "", emailTitle = "", templateName = "";
                User user = new User();
                DBFrameworkMapping mapping = new DBFrameworkMapping();


                foreach (var item in extraDinerList)
                {
                    if (item.Status != 3)
                    {
                        item.DateAdded = DateTime.Now;
                        item.Status = 1;
                        item.CreatedBy = userId;
                    }
                    mapping.dtoList.Add(new DBFrameworkDto() { Dto = item, TableName = "ExtraordinaryDiner" });
                    associateList += item.AssociateName + "<br>";
                }

                mapping.Save();

                if (extraDinerList[0].Status != 3)
                {
                    emailTitle = "YMM HR System: Extra Diner Request";
                    templateName = "TemplateExtraDinerRequest";
                }
                else
                {
                    emailTitle = "YMM HR System: Extra Diner Confirmation";
                    templateName = "TemplateExtraDinerConfirmation";
                }

                List<string> contacts = emailNotification.GetExtraDinerContacts(1).Split(',').ToList();
                contacts.Add(user.GetUserEmail(extraDinerList[0].CreatedBy));
                if (extraDinerList[0].Contacts != null && !string.IsNullOrEmpty(extraDinerList[0].Contacts))
                {
                    List<string> ccContacts = extraDinerList[0].Contacts.Split(',').ToList();
                    contacts.AddRange(ccContacts.Distinct());
                }
                sendEmail.SendEmailTemplate(emailTitle, templateName, new[,]
                {
                        {"$APPLICANT$", extraDinerList[0].UserCreated},
                        {"$TYPE$", extraDinerList[0].Type},
                        {"$ASSOCIATES$", associateList},
                        {"$DATE$", extraDinerList[0].Date?.ToString("dd/MM/yyyy") },
                        {"$TIME$", extraDinerList[0].Time.ToString()},
                        {"$MOTIVE$", extraDinerList[0].Motive}
                 }, sendEmail.GetAdminEmail(), contacts);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Multiple Extra Diner", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveMultipleExtra");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Diner with fields.
        /// </summary>
        /// <returns>Load Diner Dto</returns>
        public List<DtoExtraordinaryDiner> LoadMultipleExtra(string userFilter = "")
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoExtraordinaryDiner> extraDinerList = new List<DtoExtraordinaryDiner>();
                if (userFilter == "")
                {
                    //mapping.Load<DtoExtraordinaryDiner>("SELECT TOP 300 * FROM (SELECT DISTINCT DateAdded, ExtraordinaryDinerId, AssociateName, Process, Date, Time, Type, TypeCost, Lading, LadingCost, Motive, UserCreated, UserModified, Status, CreatedBy, Contacts FROM ExtraordinaryDiner) AS TempTable ORDER BY Date DESC, Status", "ExtraordinaryDiner", new DtoExtraordinaryDiner());
                    //mapping.Load<DtoExtraordinaryDiner>("EXEC GetUniqueExtraordinaryDiner;", "GetUniqueExtraordinaryDiner", new DtoExtraordinaryDiner());
                    mapping.Load<DtoExtraordinaryDiner>("SELECT TOP 200 ExtraordinaryDinerId, AssociateName, Process, Date, Time, Type, Motive, TypeCost, Lading, LadingCost, Motive, DateAdded, UserCreated, UserModified, Status, CreatedBy, Contacts FROM ExtraordinaryDiner ORDER BY ExtraordinaryDinerId DESC, Status", "ExtraordinaryDiner", new DtoExtraordinaryDiner());
                }
                else
                {
                    mapping.Load<DtoExtraordinaryDiner>("SELECT TOP 200 ExtraordinaryDinerId, AssociateName, Process, Date, Time, Type, Motive, TypeCost, Lading, LadingCost, Motive, DateAdded, UserCreated, UserModified, Status, CreatedBy, Contacts FROM ExtraordinaryDiner WHERE UserCreated = '" + userFilter + "' ORDER BY ExtraordinaryDinerId DESC, Status", "ExtraordinaryDiner", new DtoExtraordinaryDiner());
                }

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

                mapping.Load<DtoExtraordinaryDiner>("SELECT ExtraordinaryDinerId, AssociateName, Process, Date, Time, Type, Motive, TypeCost, Lading, LadingCost, Motive, DateAdded, UserCreated, UserModified, Status, CreatedBy, Contacts FROM ExtraordinaryDiner WHERE Date BETWEEN '" + startDate + "' AND '" + endDate + "' ORDER BY ExtraordinaryDinerId DESC, Status", "ExtraordinaryDiner", new DtoExtraordinaryDiner());
                extraDinerList.AddRange(mapping.dtoList.Select(renglon => (DtoExtraordinaryDiner)renglon.Dto));

                return extraDinerList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Extra Diners With Filter", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleExtraWithFilter");
                throw ex;
            }
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="applicant"></param>
        /// <param name="type"></param>
        /// <param name="time"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<DtoExtraordinaryDiner> LoadMultipleCommonExtra(string applicant, string type, DateTime date)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoExtraordinaryDiner> extraDinerList = new List<DtoExtraordinaryDiner>();

                mapping.Load<DtoExtraordinaryDiner>("SELECT ExtraordinaryDinerId, AssociateName, Process, Date, Time, Type, Motive, TypeCost, Lading, LadingCost, Motive, DateAdded, UserCreated, UserModified, Status, CreatedBy, Contacts FROM ExtraordinaryDiner WHERE UserCreated = '" + applicant + "' AND Type = '" + type + "' AND DateAdded = '" + date.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Status = 1 ORDER BY ExtraordinaryDinerId DESC, Status", "ExtraordinaryDiner", new DtoExtraordinaryDiner());
                extraDinerList.AddRange(mapping.dtoList.Select(renglon => (DtoExtraordinaryDiner)renglon.Dto));

                return extraDinerList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Common Extra Diners With Filters", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleCommonExtra");
                throw ex;
            }
        }
        #endregion

        #region General Methods    
        /// <summary>
        /// Confirm Extra Diner
        /// </summary>
        /// <param name="extraordinaryDinerId"></param>
        public void ConfirmExtraordinaryDiner(long extraordinaryDinerId, long userId)
        {
            try
            {
                DtoExtraordinaryDiner dtoExtraordinaryDiner = LoadExtra(extraordinaryDinerId);

                List<DtoExtraordinaryDiner> commonExtraDiner = LoadMultipleCommonExtra(dtoExtraordinaryDiner.UserCreated, dtoExtraordinaryDiner.Type, dtoExtraordinaryDiner.DateAdded);
                commonExtraDiner.ForEach(x => x.Status = 3);

                SaveMultipleExtra(commonExtraDiner, userId);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Diner Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "ConfirmExtraordinaryDiner");
                throw ex;
            }
        }
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
        /// Cancels a Extra Diner. 
        /// </summary>
        /// <param name="extraDinerId"></param>
        /// <returns></returns>
        public bool CancelExtraDiner(long extraDinerId)
        {
            try
            {
                DtoExtraordinaryDiner dtoExtraordinaryDiner = LoadExtra(extraDinerId);

                string query = "UPDATE ExtraordinaryDiner SET Status = 2 WHERE ExtraordinaryDinerId =" + extraDinerId;
                oDatabase.ExecuteNonQuery(query, "Cancel Extra Diner");

                User user = new User();
                List<string> contacts = emailNotification.GetExtraDinerContacts(1).Split(',').ToList();
                contacts.Add(user.GetUserEmail(dtoExtraordinaryDiner.CreatedBy));
                try
                {
                    sendEmail.SendEmailTemplate("YMM HR System: Extra Diner Cancellation", "TemplateExtraDinerCancellation", new[,]
                    {
                        {"$APPLICANT$", dtoExtraordinaryDiner.UserCreated},
                        {"$TYPE$", dtoExtraordinaryDiner.Type},
                        {"$ASSOCIATE$", dtoExtraordinaryDiner.AssociateName},
                        {"$DATE$", dtoExtraordinaryDiner.Date?.ToString("dd/MM/yyyy")}
                    }, sendEmail.GetAdminEmail(), contacts);
                }
                catch (Exception)
                {
                }

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Cancel Extraordinary Diner", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "CancelExtraDiner");
                return false;
            }

        }
        #endregion

        /// <summary>
        /// Date validation. 
        /// </summary>
        /// <param name="dateOrdered"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public (bool success, string message) ValidateDinerDate(DateTime dateOrdered, string time)
        {
            dateOrdered = Convert.ToDateTime(dateOrdered.ToString("dd-MM-yyyy") + " " + time);

            if (dateOrdered.Subtract(DateTime.Now).TotalHours < 4 || dateOrdered < DateTime.Now)
            {
                return (false, "Please request diner within 4-hour window.");
            }

            if (dateOrdered.Subtract(DateTime.Now).Days == 1)
            {
                TimeSpan deadline = TimeSpan.Parse("16:56", CultureInfo.InvariantCulture);
                if (TimeSpan.Compare(DateTime.Now.TimeOfDay, deadline) == 1)
                {
                    return (false, "Please request next-day diner earlier than 4:56 PM.");
                }
            }

            if ((dateOrdered.DayOfWeek == DayOfWeek.Saturday || dateOrdered.DayOfWeek == DayOfWeek.Sunday) && DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                TimeSpan deadline = TimeSpan.Parse("10:30", CultureInfo.InvariantCulture);
                if (TimeSpan.Compare(DateTime.Now.TimeOfDay, deadline) == 1)
                {
                    return (false, "Please request Saturday diner earlier than 10:30 AM Friday.");
                }
            }

            if (dateOrdered.Day == DateTime.Now.Day)
            {
                TimeSpan deadline = TimeSpan.Parse("15:30", CultureInfo.InvariantCulture);
                if (TimeSpan.Compare(DateTime.Now.TimeOfDay, deadline) == 1)
                {
                    return (false, "Please request same-day diner earlier than 3:30 PM.");
                }
            }

            return (true, string.Empty); // Validación exitosa
        }
    }
}
