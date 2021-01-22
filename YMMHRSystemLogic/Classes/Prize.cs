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
    public class Prize
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        #region Standard Methods
        /// <summary>
        /// Loads the Prize DTO
        /// </summary>
        /// <param name="prizeId"></param>
        /// <returns>DtoPrize Loaded</returns>
        public DtoPrize Load(long prizeId)
        {
            DtoPrize prize = new DtoPrize();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (prizeId != 0)
                {
                    mapping.Load<DtoPrize>("Prizes", new DtoPrize(), "PrizeId=" + prizeId);
                    prize = (DtoPrize)mapping.dtoList.FirstOrDefault().Dto;
                }

                prize.PrizeLogList = LoadMultiplePrizeLog(prizeId);

                return prize;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Register Prize 
        /// </summary>
        /// <param name="prize"></param>
        /// <returns>Prize registered</returns>
        public void Save(DtoPrize prize)
        {
            try
            {
                User user = new User();

                if (prize.PrizeId == 0)
                {
                    prize.Date = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = prize, TableName = "Prizes" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Prize", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Prize with fields.
        /// </summary>
        /// <returns>Load Prize Dto</returns>
        public List<DtoPrize> LoadMultiple()

        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoPrize> prizeList = new List<DtoPrize>();

                mapping.Load<DtoPrize>("SELECT PrizeId, Name, Points, Quantity, Date, Minimum, Status FROM Prizes ORDER BY PrizeId DESC, Status DESC", "Prize", new DtoPrize());
                prizeList.AddRange(mapping.dtoList.Select(renglon => (DtoPrize)renglon.Dto));

                return prizeList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Prize", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Prize with point filter.
        /// </summary>
        /// <returns>Load Prize Dto</returns>
        public List<DtoPrize> LoadMultipleWithPointFilter(int points)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoPrize> prizeList = new List<DtoPrize>();

                mapping.Load<DtoPrize>("SELECT PrizeId, Name, Points, Quantity, Date, Minimum, Status FROM Prizes WHERE Points <= " + points + " AND Quantity > 0 AND Status = 1 ORDER BY PrizeId DESC, Status DESC", "Prize", new DtoPrize());
                prizeList.AddRange(mapping.dtoList.Select(renglon => (DtoPrize)renglon.Dto));

                return prizeList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Prize", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Prize Log with fields.
        /// </summary>
        /// <returns>Load Prize Log Dto</returns>
        public List<DtoPrizeLog> LoadMultiplePrizeLog(long prizeId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoPrizeLog> prizeList = new List<DtoPrizeLog>();

                mapping.Load<DtoPrizeLog>("SELECT MovementType, Associate, Quantity, Date, PrizeId FROM PrizeLog WHERE PrizeId = " + prizeId + " ORDER BY PrizeId", "PrizeLog", new DtoPrizeLog());
                prizeList.AddRange(mapping.dtoList.Select(renglon => (DtoPrizeLog)renglon.Dto));

                return prizeList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Prize Log", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiplePrizeLog");
                throw ex;
            }
        }
        #endregion

        #region General Methods    
        /// <summary>
        /// Get Prize Name
        /// </summary>
        /// <param name="prizeId"></param>
        /// <returns></returns>
        public string GetPrizeName(long prizeId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT Name FROM Prizes WHERE PrizeId = " + prizeId;

                dataRow = oDatabase.GetRow(sqlString, "Get Prize Name");

                if (dataRow == null) return "";

                return dataRow["Name"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Prize Name", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetPrizeName");
                throw ex;
            }

        }
        /// <summary>
        /// Get Prize Points
        /// </summary>
        /// <param name="prizeId"></param>
        /// <returns></returns>
        public int GetPrizePoints(long prizeId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT Points FROM Prizes WHERE PrizeId = " + prizeId;

                dataRow = oDatabase.GetRow(sqlString, "Get Prize Points");

                if (dataRow == null) return 0;

                return Convert.ToInt32(dataRow["Points"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Prize Points", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetPrizePoints");
                throw ex;
            }

        }
        /// <summary>
        /// Get Prize Id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public long GetPrizeId(string name)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT PrizeId FROM Prizes WHERE Name = '" + name + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get PrizeId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["PrizeId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Prize Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetPrizeId");
                throw ex;
            }

        }
        /// <summary>
        /// Deletes a Prize. 
        /// </summary>
        /// <param name="prizeId"></param>
        /// <returns></returns>
        public bool DeletePrize(long prizeId)
        {
            try
            {
                string query = "";
                string queryValidation = "SELECT Count(PrizeId) FROM PrizeLog WHERE PrizeId =" + prizeId;

                DataRow row = oDatabase.GetRow(queryValidation, "Validate Prize Log Existence");

                if (Convert.ToInt32(row[0]) == 0)
                {
                    query = "DELETE Prizes WHERE PrizeId =" + prizeId;
                    oDatabase.ExecuteNonQuery(query, "Remove Prize");
                }
                else
                {
                    query = "UPDATE Prizes SET Status = 0 WHERE PrizeId =" + prizeId;
                    oDatabase.ExecuteNonQuery(query, "Remove Prize");
                }

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Prize", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeletePrize");
                return false;
            }

        }
        /// <summary>
        /// Deletes a Prize Log. 
        /// </summary>
        /// <param name="prizeLogId"></param>
        /// <returns></returns>
        public bool DeletePrizeLog(long prizeLogId)
        {
            try
            {
                string query = "DELETE PrizeLog WHERE PrizeLogId =" + prizeLogId;
                oDatabase.ExecuteNonQuery(query, "Remove Prize log");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Prize Log", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeletePrizeLog");
                return false;
            }

        }
        /// <summary>
        /// Insert Prize Log
        /// </summary>
        /// <param name="movementType"></param>
        /// <param name="associate"></param>
        /// <param name="quantity"></param>
        /// <param name="date"></param>
        public void SavePrizeLog(DtoPrizeLog prizeLog)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = prizeLog, TableName = "PrizeLog" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Prize Log", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SavePrizeLog");
                throw ex;
            }
        }
        #endregion
    }
}
