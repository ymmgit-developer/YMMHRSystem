using DBFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace YMMHRSystemLogic
{
    public class FirstNotificationEntryExit
    {
        Log log = new Log();
        DBFrameworkMapping mapping = new DBFrameworkMapping();
        SQLTools oDatabase = new SQLTools();

        #region General Methods
        public bool Save(DtoFirstNotificationEntryExit dtoFirstNotificationEntryExit)
        {
            try
            {
                User user = new User();
                if (dtoFirstNotificationEntryExit.IdNotification == 0)
                {
                    dtoFirstNotificationEntryExit.DateAdded = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = dtoFirstNotificationEntryExit, TableName = "FirstNotificationEntryExit" });

                mapping.Save();
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Register In-Out", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                return false;
            }
        }
        public bool Delete(long IdNotification)
        {
            try
            {
                string query = "DELETE FirstNotificationEntryExit WHERE IdNotification =" + @IdNotification;
                oDatabase.ExecuteNonQuery(query, "Remove FirstNotification In-Out");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Register In-Out", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                return false;
            }
        }

        public List<DtoFirstNotificationEntryExit> GetRecords()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoFirstNotificationEntryExit> listrecords = new List<DtoFirstNotificationEntryExit>();
                mapping.Load<DtoFirstNotificationEntryExit>("SELECT * FROM FirstNotificationEntryExit ORDER BY IdNotification ASC", "FirstNotificationEntryExit", new DtoFirstNotificationEntryExit());
                listrecords.AddRange(mapping.dtoList.Select(renglon => (DtoFirstNotificationEntryExit)renglon.Dto));
                return listrecords;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Records Second Notification EntryExit", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Get");
                throw ex;
            }
        }

        public DtoFirstNotificationEntryExit GetRecord(long IdNotification)
        {
            DBFrameworkMapping mapping = new DBFrameworkMapping();
            DtoFirstNotificationEntryExit record = new DtoFirstNotificationEntryExit();
            mapping.Load<DtoFirstNotificationEntryExit>("FirstNotificationEntryExit", new DtoFirstNotificationEntryExit(), "IdNotification = " + IdNotification);
            record = (DtoFirstNotificationEntryExit)mapping.dtoList.First().Dto;
            return record;
        }
        #endregion

        #region Process methods
        public List<DtoEmailToNotify> GetNotifyTo(string IdApplicant)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoEmailToNotify> listToNotify = new List<DtoEmailToNotify>();
                mapping.Load<DtoEmailToNotify>("SELECT EmailToNotify FROM FirstNotificationEntryExit WHERE IdApplicant = " + IdApplicant, "FirstNotificationEntryExit", new DtoEmailToNotify());
                listToNotify.AddRange(mapping.dtoList.Select(renglon => (DtoEmailToNotify)renglon.Dto));
                return listToNotify;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Contacts to First Notification", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetNotifyTo");
                throw ex;
            }
        }
        #endregion
    }
}
