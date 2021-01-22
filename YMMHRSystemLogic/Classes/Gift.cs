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
    public class Gift
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        #region Standard Methods
        /// <summary>
        /// Loads the Gift DTO
        /// </summary>
        /// <param name="giftId"></param>
        /// <returns>DtoGift Loaded</returns>
        public DtoGift Load(long giftId)
        {
            DtoGift gift = new DtoGift();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (giftId != 0)
                {
                    mapping.Load<DtoGift>("Gifts", new DtoGift(), "GiftId=" + giftId);
                    gift = (DtoGift)mapping.dtoList.FirstOrDefault().Dto;
                }

                gift.GiftLogList = LoadMultipleGiftLog(giftId);

                return gift;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Gift", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Register Gift 
        /// </summary>
        /// <param name="gift"></param>
        /// <returns>Gift registered</returns>
        public void Save(DtoGift gift)
        {
            try
            {
                User user = new User();

                if (gift.GiftId == 0)
                {
                    gift.Date = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = gift, TableName = "Gifts" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Gift", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Gift with fields.
        /// </summary>
        /// <returns>Load Gift Dto</returns>
        public List<DtoGift> LoadMultiple(bool withActiveFilter = false)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoGift> giftList = new List<DtoGift>();
                if (withActiveFilter)
                {
                    mapping.Load<DtoGift>("SELECT GiftId, Name, Quantity, Date, Status FROM Gifts WHERE Status = 1 AND Quantity > 0 ORDER BY GiftId DESC, Status DESC", "Gift", new DtoGift());
                }
                else
                {
                    mapping.Load<DtoGift>("SELECT GiftId, Name, Quantity, Date, Status FROM Gifts ORDER BY GiftId DESC, Status DESC", "Gift", new DtoGift());
                }      
                giftList.AddRange(mapping.dtoList.Select(renglon => (DtoGift)renglon.Dto));

                return giftList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Gift", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Gift Log with fields.
        /// </summary>
        /// <returns>Load Gift Log Dto</returns>
        public List<DtoGiftLog> LoadMultipleGiftLog(long giftId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoGiftLog> giftList = new List<DtoGiftLog>();

                mapping.Load<DtoGiftLog>("SELECT MovementType, Associate, Quantity, Date, GiftId FROM GiftLog WHERE GiftId = " + giftId + " ORDER BY GiftId", "GiftLog", new DtoGiftLog());
                giftList.AddRange(mapping.dtoList.Select(renglon => (DtoGiftLog)renglon.Dto));

                return giftList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Gift Log", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleGiftLog");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Gift Log with fields by Associate.
        /// </summary>
        /// <returns>Load Gift Log Dto</returns>
        public List<DtoGiftLog> LoadMultipleGiftLogByAssociate(string associate)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoGiftLog> giftList = new List<DtoGiftLog>();

                mapping.Load<DtoGiftLog>("SELECT MovementType, Gift, Associate, Quantity, Date, GiftId FROM GiftLog WHERE Associate = '" + associate + "' ORDER BY GiftId", "GiftLog", new DtoGiftLog());
                giftList.AddRange(mapping.dtoList.Select(renglon => (DtoGiftLog)renglon.Dto));

                return giftList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Gift Log By Associate", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleGiftLogByAssociate");
                throw ex;
            }
        }
        #endregion

        #region General Methods    
        /// <summary>
        /// Get Gift Name
        /// </summary>
        /// <param name="giftId"></param>
        /// <returns></returns>
        public string GetGiftName(long giftId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT Name FROM Gifts WHERE GiftId = " + giftId;

                dataRow = oDatabase.GetRow(sqlString, "Get Gift Name");

                if (dataRow == null) return "";

                return dataRow["Name"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Gift Name", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetGiftName");
                throw ex;
            }

        }
        /// <summary>
        /// Get Gift Id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public long GetGiftId(string name)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT GiftId FROM Gifts WHERE Name = '" + name + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get GiftId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["GiftId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Gift Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetGiftId");
                throw ex;
            }

        }
        /// <summary>
        /// Deletes a Gift. 
        /// </summary>
        /// <param name="giftId"></param>
        /// <returns></returns>
        public bool DeleteGift(long giftId)
        {
            try
            {
                string query = "";
                string queryValidation = "SELECT Count(GiftId) FROM GiftLog WHERE GiftId =" + giftId;

                DataRow row = oDatabase.GetRow(queryValidation, "Validate Gift Log Existence");

                if (Convert.ToInt32(row[0]) == 0)
                {
                    query = "DELETE Gifts WHERE GiftId =" + giftId;
                    oDatabase.ExecuteNonQuery(query, "Remove Gift");
                }
                else
                {
                    query = "UPDATE Gifts SET Status = 0 WHERE GiftId =" + giftId;
                    oDatabase.ExecuteNonQuery(query, "Remove Gift");
                }

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Gift", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteGift");
                return false;
            }

        }
        /// <summary>
        /// Insert Gift Log
        /// </summary>
        /// <param name="movementType"></param>
        /// <param name="associate"></param>
        /// <param name="quantity"></param>
        /// <param name="date"></param>
        public void SaveGiftLog(DtoGiftLog giftLog)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = giftLog, TableName = "GiftLog" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Gift Log", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveGiftLog");
                throw ex;
            }
        }
        /// <summary>
        /// Get Gift Log Id
        /// </summary>
        /// <param name="gift"></param>
        /// <param name="associate"></param>
        /// <param name="date></param>
        /// <returns></returns>
        public long GetGiftLogId(string gift, string associate, DateTime date)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT GiftLogId FROM GiftLog WHERE Gift = '" + gift + "' AND  Associate = '" + associate + "' AND CAST(Date as date) = '" + date.ToString("yyyyMMdd") + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get GiftLogId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["GiftLogId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Gift Log Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetGiftLogId");
                throw ex;
            }

        }
        #endregion
    }
}
