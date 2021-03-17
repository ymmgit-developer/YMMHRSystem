using DBFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMMHRSystemLogic;

namespace YMMHRSystemLogic
{
    public class CoffeeBreak
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        SendEmail sendEmail = new SendEmail();
        EmailNotification emailNotification = new EmailNotification();

        #region Standard Methods
        /// <summary>
        /// Loads the Coffee Break DTO
        /// </summary>
        /// <param name="coffeeBreakId"></param>
        /// <returns>DtoCoffeeBreak Loaded</returns>
        public DtoCoffeeBreak Load(long coffeeBreakId)
        {
            DtoCoffeeBreak coffeeBreak = new DtoCoffeeBreak();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (coffeeBreakId != 0)
                {
                    mapping.Load<DtoCoffeeBreak>("CoffeeBreaks", new DtoCoffeeBreak(), "CoffeeBreakId=" + coffeeBreakId);
                    coffeeBreak = (DtoCoffeeBreak)mapping.dtoList.FirstOrDefault().Dto;
                }

                return coffeeBreak;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load CoffeeBreak", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Register CoffeeBreak 
        /// </summary>
        /// <param name="coffeeBreak"></param>
        /// <returns>CoffeeBreak registered</returns>
        public void Save(DtoCoffeeBreak coffeeBreak)
        {
            try
            {
                User user = new User();
                if (coffeeBreak.CoffeeBreakId == 0)
                {
                    coffeeBreak.Responsable = user.GetUserName(SQLTools.userId.ToString());
                    coffeeBreak.DateAdded = DateTime.Now;
                    coffeeBreak.Status = 1;

                    sendEmail.SendEmailTemplate("YMM HR System: Coffee Break Request", "TemplateCoffeeBreakRequest", new[,]
                    {
                        {"$APPLICANT$", coffeeBreak.Responsable},
                        {"$TYPE$", coffeeBreak.Type},
                        {"$ROOM$", coffeeBreak.Room},
                        {"$DATE$", coffeeBreak.StartDate?.ToString("dd/MM/yyyy")}
                    }, sendEmail.GetAdminEmail(), emailNotification.GetCoffeeBreakContacts(1).Split(',').ToList());
                }

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = coffeeBreak, TableName = "CoffeeBreaks" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Coffee Break", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple CoffeeBreak with fields.
        /// </summary>
        /// <returns>Load CoffeeBreak Dto</returns>
        public List<DtoCoffeeBreak> LoadMultiple()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoCoffeeBreak> coffeeBreakList = new List<DtoCoffeeBreak>();

                mapping.Load<DtoCoffeeBreak>("SELECT CoffeeBreakId, Responsable, AttendeeQuantity, InstructorQuantity, Room, Type, StartTime, FinishTime, StartDate, FinishDate, Status, DateAdded FROM CoffeeBreaks ORDER BY CoffeeBreakId", "CoffeeBreaks", new DtoCoffeeBreak());
                coffeeBreakList.AddRange(mapping.dtoList.Select(renglon => (DtoCoffeeBreak)renglon.Dto));

                return coffeeBreakList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple CoffeeBreak", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        #endregion

        #region General Methods    
        /// <summary>
        /// Deletes a CoffeeBreak. 
        /// </summary>
        /// <param name="coffeeBreakId"></param>
        /// <returns></returns>
        public bool DeleteCoffeeBreak(long coffeeBreakId)
        {
            try
            {
                string query = "DELETE CoffeeBreaks WHERE CoffeeBreakId =" + coffeeBreakId;
                oDatabase.ExecuteNonQuery(query, "Remove CoffeeBreak");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete CoffeeBreak", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteCoffeeBreak");
                return false;
            }

        }
        /// <summary>
        /// Get CoffeeBreak status
        /// </summary>
        /// <param name="coffeeBreakId"></param>
        /// <returns></returns>
        public int GetCoffeeBreakStatus(long coffeeBreakId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT Status FROM CoffeeBreaks WHERE CoffeeBreakId = " + coffeeBreakId;

                dataRow = oDatabase.GetRow(sqlString, "Get CoffeeBreak Status");

                if (dataRow == null) return 0;

                return Convert.ToInt32(dataRow["Status"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get CoffeeBreak Status", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetCoffeeBreakStatus");
                throw ex;
            }
        }
        #endregion
    }
}
