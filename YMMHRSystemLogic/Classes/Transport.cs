using DBFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class Transport
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        SendEmail sendEmail = new SendEmail();
        EmailNotification emailNotification = new EmailNotification();

        #region Standard Methods
        /// <summary>
        /// Loads the Transport DTO
        /// </summary>
        /// <param name="transportId"></param>
        /// <returns>DtoTransport Loaded</returns>
        public DtoTransport Load(long transportId)
        {
            DtoTransport Transport = new DtoTransport();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (transportId != 0)
                {
                    mapping.Load<DtoTransport>("Transports", new DtoTransport(), "TransportId=" + transportId);
                    Transport = (DtoTransport)mapping.dtoList.FirstOrDefault().Dto;
                }

                return Transport;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Transport", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Register Transport 
        /// </summary>
        /// <param name="transport"></param>
        /// <returns>Transport registered</returns>
        public void Save(DtoTransport transport)
        {
            try
            {
                User user = new User();

                if (transport.TransportId == 0)
                {
                    transport.Date = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = transport, TableName = "Transports" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Transport", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Transport with fields.
        /// </summary>
        /// <returns>Load Transport Dto</returns>
        public List<DtoTransport> LoadMultiple()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoTransport> shiftList = new List<DtoTransport>();

                mapping.Load<DtoTransport>("SELECT TransportId, Route, Date, StartDate, FinishDate, Cost, UserCreated, Type FROM Transports ORDER BY TransportId DESC", "Transports", new DtoTransport());
                shiftList.AddRange(mapping.dtoList.Select(renglon => (DtoTransport)renglon.Dto));

                return shiftList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Transports", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<DtoTransport> LoadMultipleWithFilter(string startDate, string endDate)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoTransport> transportList = new List<DtoTransport>();

                mapping.Load<DtoTransport>("SELECT TransportId, Route, Date, StartDate, FinishDate, Cost, UserCreated, Type FROM Transports WHERE Date BETWEEN '" + startDate + "' AND '" + endDate + "' ORDER BY TransportId DESC", "Transports", new DtoTransport());
                transportList.AddRange(mapping.dtoList.Select(renglon => (DtoTransport)renglon.Dto));

                return transportList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Transports With Filter", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleWithFilter");
                throw ex;
            }
        }

        /// <summary>
        /// Loads the Extra Transport DTO
        /// </summary>
        /// <param name="transportId"></param>
        /// <returns>DtoTransport Loaded</returns>
        public DtoExtraordinaryTransport LoadExtra(long transportId)
        {
            DtoExtraordinaryTransport extraTransport = new DtoExtraordinaryTransport();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (transportId != 0)
                {
                    mapping.Load<DtoExtraordinaryTransport>("ExtraordinaryTransports", new DtoExtraordinaryTransport(), "ExtraordinaryTransportId=" + transportId);
                    extraTransport = (DtoExtraordinaryTransport)mapping.dtoList.FirstOrDefault().Dto;
                }

                return extraTransport;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Extraordinary Transport", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadExtra");
                throw ex;
            }
        }
        /// <summary>
        /// Register Transport 
        /// </summary>
        /// <param name="extraTransport"></param>
        /// <returns>Transport registered</returns>
        public void SaveExtra(DtoExtraordinaryTransport extraTransport, long userId = 0)
        {
            try
            {
                User user = new User();

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = extraTransport, TableName = "ExtraordinaryTransports" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Extra Transport", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveExtra");
                throw ex;
            }
        }
        /// <summary>
        /// Register Transport 
        /// </summary>
        /// <param name="extraTransport"></param>
        /// <returns>Transport registered</returns>
        public void SaveMultipleExtra(List<DtoExtraordinaryTransport> extraTransportList, long userId = 0)
        {
            try
            {
                User user = new User();
                string associateList = "", emailTitle = "", templateName = "";
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                foreach (var item in extraTransportList)
                {
                    if (item.Status != 3)
                    {
                        item.Date = DateTime.Now;
                        item.Status = 1;
                        item.CreatedBy = userId;
                    }
                    mapping.dtoList.Add(new DBFrameworkDto() { Dto = item, TableName = "ExtraordinaryTransports" });
                    associateList += item.AssociateName + "<br>";
                }

                mapping.Save();

                List<string> contacts = emailNotification.GetExtraTransportContacts(1).Split(',').ToList();
                if (!string.IsNullOrEmpty(extraTransportList[0].Contacts) && extraTransportList[0].Contacts != null)
                {
                    List<string> ccContacts = extraTransportList[0].Contacts.Split(',').ToList();
                    contacts.AddRange(ccContacts.Distinct());
                }
                contacts.Add(user.GetUserEmail(extraTransportList[0].CreatedBy));
                string date, time = "";
                if (extraTransportList[0].StartDate == null)
                {
                    date = extraTransportList[0].FinishDate?.ToString("dd/MM/yyyy");
                    time = extraTransportList[0].FinishTime.ToString();
                }
                else
                {
                    date = extraTransportList[0].StartDate?.ToString("dd/MM/yyyy");
                    time = extraTransportList[0].StartTime.ToString();
                }
                if (extraTransportList[0].Status != 3)
                {
                    emailTitle = "YMM HR System: Extra Transport Request";
                    templateName = "TemplateExtraTransportRequest";
                }
                else
                {
                    emailTitle = "YMM HR System: Extra Transport Confirmation";
                    templateName = "TemplateExtraTransportConfirmation";
                }

                sendEmail.SendEmailTemplate(emailTitle, templateName, new[,]
                {
                        {"$APPLICANT$", extraTransportList[0].UserCreated},
                        {"$MOTIVE$", extraTransportList[0].Motive},
                        {"$ASSOCIATES$", associateList},
                        {"$DATE$", date},
                        {"$TIME$", time},
                    }, sendEmail.GetAdminEmail(), contacts);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Multiple Extra Transport", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveMultipleExtra");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Transport with fields.
        /// </summary>
        /// <returns>Load Transport Dto</returns>
        public List<DtoExtraordinaryTransport> LoadMultipleExtra(string userFilter = "")
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoExtraordinaryTransport> extraTransportList = new List<DtoExtraordinaryTransport>();
                if (userFilter == "")
                {
                    mapping.Load<DtoExtraordinaryTransport>("SELECT TOP 200 ExtraordinaryTransportId, AssociateName, Process, Route, Stop, StartTime, StartDate, FinishTime, FinishDate, Motive, Cost, Date, UserCreated, UserModified, Status, CreatedBy, Contacts, ShiftChange FROM ExtraordinaryTransports ORDER BY ExtraordinaryTransportId DESC, Status", "ExtraordinaryTransports", new DtoExtraordinaryTransport());
                }
                else
                {
                    mapping.Load<DtoExtraordinaryTransport>("SELECT TOP 200 ExtraordinaryTransportId, AssociateName, Process, Route, Stop, StartTime, StartDate, FinishTime, FinishDate, Motive, Cost, Date, UserCreated, UserModified, Status, CreatedBy, Contacts, ShiftChange FROM ExtraordinaryTransports WHERE UserCreated = '" + userFilter + "' ORDER BY ExtraordinaryTransportId DESC, Status", "ExtraordinaryTransports", new DtoExtraordinaryTransport());
                }

                extraTransportList.AddRange(mapping.dtoList.Select(renglon => (DtoExtraordinaryTransport)renglon.Dto));

                return extraTransportList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Extra Transports", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleExtra");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<DtoExtraordinaryTransport> LoadMultipleExtraWithFilter(string startDate, string endDate)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoExtraordinaryTransport> extraTransportList = new List<DtoExtraordinaryTransport>();

                mapping.Load<DtoExtraordinaryTransport>("SELECT ExtraordinaryTransportId, AssociateName, Process, Route, Stop, StartTime, StartDate, FinishTime, FinishDate, Motive, Cost, Date, UserCreated, UserModified, Status, CreatedBy, Contacts, ShiftChange FROM ExtraordinaryTransports WHERE Date BETWEEN '" + startDate + "' AND '" + endDate + "' ORDER BY ExtraordinaryTransportId DESC, Status", "ExtraordinaryTransports", new DtoExtraordinaryTransport());
                extraTransportList.AddRange(mapping.dtoList.Select(renglon => (DtoExtraordinaryTransport)renglon.Dto));

                return extraTransportList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Extra Transports With Filter", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleExtraWithFilter");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicant"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<DtoExtraordinaryTransport> LoadMultipleCommonExtra(string applicant, DateTime date)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoExtraordinaryTransport> extraTransportList = new List<DtoExtraordinaryTransport>();

                mapping.Load<DtoExtraordinaryTransport>("SELECT ExtraordinaryTransportId, AssociateName, Process, Route, Stop, StartTime, StartDate, FinishTime, FinishDate, Motive, Cost, Date, UserCreated, UserModified, Status, CreatedBy, Contacts, ShiftChange FROM ExtraordinaryTransports WHERE UserCreated = '" + applicant + "' AND Date = '" + date.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Status = 1 ORDER BY ExtraordinaryTransportId DESC, Status", "ExtraordinaryTransports", new DtoExtraordinaryTransport());
                extraTransportList.AddRange(mapping.dtoList.Select(renglon => (DtoExtraordinaryTransport)renglon.Dto));

                return extraTransportList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Common Extra Transports With Filter", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleCommonExtra");
                throw ex;
            }
        }
        #endregion

        #region General Methods  
        public void ConfirmExtraordinaryTransport(long extraordinaryTransportId, long userId)
        {
            try
            {
                DtoExtraordinaryTransport dtoExtraordinaryTransport = LoadExtra(extraordinaryTransportId);

                List<DtoExtraordinaryTransport> commonExtraTransport = LoadMultipleCommonExtra(dtoExtraordinaryTransport.UserCreated, dtoExtraordinaryTransport.Date);
                commonExtraTransport.ForEach(x => x.Status = 3);
                SaveMultipleExtra(commonExtraTransport, userId);

            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Confirm Extraordinary Transport", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "ConfirmExtraordinaryTransport");
                throw ex;
            }
        }
        /// <summary>
        /// Get Transport Id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public long GetTransportId(string route, DateTime startDate, DateTime finishDate)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT TransportId FROM Transports WHERE Route = '" + route + "' AND StartDate = '" + startDate.ToString("yyyyMMdd") + "' AND FinishDate = '" + finishDate.ToString("yyyyMMdd") + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get TransportId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["TransportId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Transport Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetTransportId");
                throw ex;
            }

        }
        /// <summary>
        /// Deletes a Transport. 
        /// </summary>
        /// <param name="transportId"></param>
        /// <returns></returns>
        public bool DeleteTransport(long transportId)
        {
            try
            {
                string query = "DELETE Transports WHERE TransportId =" + transportId;
                oDatabase.ExecuteNonQuery(query, "Remove Transport");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Transport", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteTransport");
                return false;
            }

        }

        /// <summary>
        /// Get Extraordinary Transport Id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public long GetExtraTransportId(string associateName, DateTime? startDate, DateTime? finishDate)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT ExtraordinaryTransportId FROM ExtraordinaryTransports WHERE AssociateName = '" + associateName + "' AND StartDate = '" + startDate?.ToString("yyyyMMdd") + "' AND FinishDate = '" + finishDate?.ToString("yyyyMMdd") + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get Extra Transport Id");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["ExtraordinaryTransportId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Extraordinary Transport Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetExtraTransportId");
                throw ex;
            }

        }
        /// <summary>
        /// Deletes a Extra Transport. 
        /// </summary>
        /// <param name="extraTransportId"></param>
        /// <returns></returns>
        public bool CancelExtraTransport(long extraTransportId)
        {
            try
            {
                DtoExtraordinaryTransport dtoExtraordinaryTransport = LoadExtra(extraTransportId);

                string query = "UPDATE ExtraordinaryTransports SET Status = 2 WHERE ExtraordinaryTransportId =" + extraTransportId;
                oDatabase.ExecuteNonQuery(query, "Cancels Extra Transport");

                User user = new User();

                try
                {
                    List<string> contacts = emailNotification.GetExtraTransportContacts(1).Split(',').ToList();
                    contacts.Add(user.GetUserEmail(dtoExtraordinaryTransport.CreatedBy));
                    sendEmail.SendEmailTemplate("YMM HR System: Extra Transport Cancellation", "TemplateExtraTransportCancellation", new[,]
                    {
                        {"$APPLICANT$", dtoExtraordinaryTransport.UserCreated},
                        {"$ROUTE$", dtoExtraordinaryTransport.Route},
                        {"$STOP$", dtoExtraordinaryTransport.Stop},
                        {"$ASSOCIATE$",dtoExtraordinaryTransport.AssociateName},
                        {"$DATE$", dtoExtraordinaryTransport.StartDate?.ToString("dd/MM/yyyy")}
                    }, sendEmail.GetAdminEmail(), contacts);
                }
                catch (Exception)
                {
                }
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Cancels Extraordinary Transport", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "CancelExtraTransport");
                return false;
            }

        }
        #endregion
    }
}
