using DBFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class SecondNotificationEntryExit
    {
        Log log = new Log();
        DBFrameworkMapping mapping = new DBFrameworkMapping();
        SQLTools oDatabase = new SQLTools();

        #region General Methods
        public bool Save(DtoSecondNotificationEntryExit secondNotificationEntryExit)
        {
            try
            {
                User user = new User();
                if (secondNotificationEntryExit.IdNotification == 0)
                {
                    secondNotificationEntryExit.DateAdded = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = secondNotificationEntryExit, TableName = "SecondNotificationEntryExit" });

                mapping.Save();
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Register Second Notification", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                return false;
            }
        }

        //public bool Update(DtoFirstNotificationEntryExit dtoFirstNotificationEntryExit)
        //{
        //    try
        //    {
        //        string query = "UPDATE EntryExitAuthorization SET Associate = '" + @inputoutput.Associate + "'" +
        //                        ", Process = '" + @inputoutput.Process + "', DateFor = '" + formattedDateFor + "', TimeFor = '" + inputoutput.TimeFor +
        //                        "', IdType = " + @inputoutput.IdType + ", IdSalary = " + @inputoutput.IdSalary + ", IdMotive = " + @inputoutput.IdMotive +
        //                        ", CurrentState = " + 0 + " WHERE IdRecordsInOut = " + @inputoutput.IdRecordsInOut;
        //        oDatabase.ExecuteNonQuery(query, "Update Register Second Notification Entry Exit");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.WriteToErrorLog("HR System", "Save Register Second Notification Entry Exit", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
        //        return false;
        //    }
        //}

        public bool Delete(long IdNotification)
        {
            try
            {
                string query = "DELETE SecondNotificationEntryExit WHERE IdNotification =" + @IdNotification;
                oDatabase.ExecuteNonQuery(query, "Remove Second Notification Entry Exit");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Register In-Out", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                return false;
            }
        }

        public List<DtoSecondNotificationEntryExit> GetRecords()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoSecondNotificationEntryExit> listrecords = new List<DtoSecondNotificationEntryExit>();
                mapping.Load<DtoSecondNotificationEntryExit>("SELECT * FROM SecondNotificationEntryExit ORDER BY IdNotification ASC", "SecondNotificationEntryExit", new DtoSecondNotificationEntryExit());
                listrecords.AddRange(mapping.dtoList.Select(renglon => (DtoSecondNotificationEntryExit)renglon.Dto));
                return listrecords;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Records Second Notification EntryExit", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Get");
                throw ex;
            }
        }
        #endregion

        #region Process methods
        public List<DtoEmailToNotify> GetNotifyTo()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoEmailToNotify> listToNotify = new List<DtoEmailToNotify>();
                mapping.Load<DtoEmailToNotify>("SELECT EmailToNotify FROM SecondNotificationEntryExit", "SecondNotificationEntryExit", new DtoEmailToNotify());
                listToNotify.AddRange(mapping.dtoList.Select(renglon => (DtoEmailToNotify)renglon.Dto));
                return listToNotify;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Contacts to Second Notification", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetNotifyTo");
                throw ex;
            }

        }
        #endregion
    }
}
