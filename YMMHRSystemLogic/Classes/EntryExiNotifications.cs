using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using YMMHRSystemLogic;
using DBFramework;

namespace YMMHRSystemLogic
{
    public class EntryExiNotifications
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();

        #region Standard Methods
        public bool Save(DtoEntryExiNotifications notifications) 
        {
            try
            {
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Delete(long IdNotification)
        {
            try
            {
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Process Methods
        /// <summary>
        /// Retrieves a list of users to notify by e-mail about an entry/exit request approval, 
        /// based on the applicant's ID.
        /// </summary>
        /// <param name="IdApplicant">
        /// The unique identifier for the applicant whose associated notifications are being retrieved.
        /// This parameter filters the records in the <c>Notifications</c> table to find 
        /// users associated with the specified applicant.
        /// </param>
        /// <returns>
        /// A list of <see cref="DtoEntryExiNotifications"/> objects representing users who need to be notified 
        /// about the entry/exit request. If no records are found or an error occurs, 
        /// returns an empty list.
        /// </returns>
        public List<DtoEntryExiNotifications> NotifyTo(string IdApplicant)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoEntryExiNotifications> listnotifyto = new List<DtoEntryExiNotifications>();
                mapping.Load<DtoEntryExiNotifications>("SELECT * FROM Notifications WHERE IdApplicant = " + @IdApplicant, "EntryExitAuthorization", new DtoEntryExitAuthorization());
                listnotifyto.AddRange(mapping.dtoList.Select(row => (DtoEntryExiNotifications)row.Dto));
                return listnotifyto;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Records To Notify", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "NotifyTo");
                return new List<DtoEntryExiNotifications>();
            }
        }
        #endregion
    }
}
