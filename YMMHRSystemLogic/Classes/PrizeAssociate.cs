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
    public class PrizeAssociate
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        #region Standard Methods
        /// <summary>
        /// Loads the PrizeAssociate DTO
        /// </summary>
        /// <param name="prizeAssociateId"></param>
        /// <returns>DtoPrizeAssociate Loaded</returns>
        public DtoPrizeAssociate Load(long prizeAssociateId)
        {
            DtoPrizeAssociate prizeAssociate = new DtoPrizeAssociate();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (prizeAssociateId != 0)
                {
                    mapping.Load<DtoPrizeAssociate>("PrizeAssociates", new DtoPrizeAssociate(), "PrizeAssociateId=" + prizeAssociateId);
                    prizeAssociate = (DtoPrizeAssociate)mapping.dtoList.FirstOrDefault().Dto;
                }

                prizeAssociate.AssociateMonthList = LoadPrizeAssociateMonthList(prizeAssociateId);
                prizeAssociate.AssociateYearList = LoadPrizeAssociateAssociateYearList(prizeAssociateId);
                prizeAssociate.AttendanceList = LoadPrizeAssociateAttendanceList(prizeAssociateId);
                prizeAssociate.FindingList = LoadPrizeAssociateFindingList(prizeAssociateId);
                prizeAssociate.PrizeList = LoadPrizeAssociatePrizeList(prizeAssociateId);

                return prizeAssociate;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize Associate", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Register PrizeAssociate 
        /// </summary>
        /// <param name="prizeAssociate"></param>
        /// <returns>PrizeAssociate registered</returns>
        public void Save(DtoPrizeAssociate prizeAssociate)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = prizeAssociate, TableName = "PrizeAssociates" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save PrizeAssociate", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary>
        /// Save multiple Prize Associates
        /// </summary>
        /// <param name="prizeAssociates"></param>
        public void SaveMultiple(List<DtoPrizeAssociate> prizeAssociates)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                prizeAssociates.ForEach(item =>
                {
                    mapping.dtoList.Add(new DBFrameworkDto() { Dto = item, TableName = "PrizeAssociates" });
                });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Multiple Prize Associates", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveMultiple");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple PrizeAssociate with fields.
        /// </summary>
        /// <returns>Load PrizeAssociate Dto</returns>
        public List<DtoPrizeAssociate> LoadMultiple()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoPrizeAssociate> prizeAssociateList = new List<DtoPrizeAssociate>();

                mapping.Load<DtoPrizeAssociate>("SELECT PrizeAssociateId, WorkerFileId, Associate, Department, Team, AssociatePoints, TeamPoints FROM PrizeAssociates ORDER BY PrizeAssociateId", "PrizeAssociate", new DtoPrizeAssociate());
                prizeAssociateList.AddRange(mapping.dtoList.Select(renglon => (DtoPrizeAssociate)renglon.Dto));

                return prizeAssociateList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple PrizeAssociate", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        /// <summary>
        /// Loads the Prize Associate Month DTO
        /// </summary>
        /// <param name="prizeAssociateMonthId"></param>
        /// <returns>DtoPrizeAssociate Loaded</returns>
        public DtoPrizeAssociateMonth LoadPrizeAssociateMonth(long prizeAssociateMonthId)
        {
            DtoPrizeAssociateMonth prizeAssociate = new DtoPrizeAssociateMonth();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (prizeAssociateMonthId != 0)
                {
                    mapping.Load<DtoPrizeAssociateMonth>("PrizeAssociateMonth", new DtoPrizeAssociateMonth(), "PrizeAssociateMonthId=" + prizeAssociateMonthId);
                    prizeAssociate = (DtoPrizeAssociateMonth)mapping.dtoList.FirstOrDefault().Dto;
                }

                return prizeAssociate;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize Associate Month", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadPrizeAssociateMonth");
                throw ex;
            }
        }
        /// <summary>
        /// Load Prize Associate Month List
        /// </summary>
        /// <returns>Load Prize Associate Month List</returns>
        public List<DtoPrizeAssociateMonth> LoadPrizeAssociateMonthList(long prizeAssociateId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoPrizeAssociateMonth> prizeAssociateList = new List<DtoPrizeAssociateMonth>();

                mapping.Load<DtoPrizeAssociateMonth>("SELECT PrizeAssociateMonthId, Year, Month, Points, PrizeAssociateId FROM PrizeAssociateMonth WHERE PrizeAssociateId = " + prizeAssociateId + " ORDER BY PrizeAssociateId", "PrizeAssociateMonth", new DtoPrizeAssociateMonth());
                prizeAssociateList.AddRange(mapping.dtoList.Select(renglon => (DtoPrizeAssociateMonth)renglon.Dto));

                return prizeAssociateList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize Associate Month List", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadPrizeAssociateMonthList");
                throw ex;
            }
        }
        /// <summary>
        /// Loads the Prize Associate Year DTO
        /// </summary>
        /// <param name="prizeAssociateYearId"></param>
        /// <returns>DtoPrizeAssociate Loaded</returns>
        public DtoPrizeAssociateYear LoadPrizeAssociateYear(long prizeAssociateYearId)
        {
            DtoPrizeAssociateYear prizeAssociate = new DtoPrizeAssociateYear();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (prizeAssociateYearId != 0)
                {
                    mapping.Load<DtoPrizeAssociateYear>("PrizeAssociateYear", new DtoPrizeAssociateYear(), "PrizeAssociateYearId=" + prizeAssociateYearId);
                    prizeAssociate = (DtoPrizeAssociateYear)mapping.dtoList.FirstOrDefault().Dto;
                }

                return prizeAssociate;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize Associate Year", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadPrizeAssociateYear");
                throw ex;
            }
        }
        /// <summary>
        /// Load Prize Associate Year List
        /// </summary>
        /// <returns>Load Prize Associate Year List</returns>
        public List<DtoPrizeAssociateYear> LoadPrizeAssociateAssociateYearList(long prizeAssociateId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoPrizeAssociateYear> prizeAssociateList = new List<DtoPrizeAssociateYear>();

                mapping.Load<DtoPrizeAssociateYear>("SELECT PrizeAssociateYearId, Year, Points, PrizeAssociateId FROM PrizeAssociateYear WHERE PrizeAssociateId = " + prizeAssociateId + " ORDER BY PrizeAssociateId", "PrizeAssociateYear", new DtoPrizeAssociateYear());
                prizeAssociateList.AddRange(mapping.dtoList.Select(renglon => (DtoPrizeAssociateYear)renglon.Dto));

                return prizeAssociateList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize Associate Year List", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadPrizeAssociateYearList");
                throw ex;
            }
        }
        /// <summary>
        /// Loads the Prize Associate Attendance DTO
        /// </summary>
        /// <param name="prizeAssociateAttendanceId"></param>
        /// <returns>DtoPrizeAssociate Loaded</returns>
        public DtoPrizeAssociateAttendance LoadPrizeAssociateAttendance(long prizeAssociateAttendanceId)
        {
            DtoPrizeAssociateAttendance prizeAssociate = new DtoPrizeAssociateAttendance();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (prizeAssociateAttendanceId != 0)
                {
                    mapping.Load<DtoPrizeAssociateAttendance>("PrizeAssociateAttendance", new DtoPrizeAssociateAttendance(), "PrizeAssociateAttendanceId=" + prizeAssociateAttendanceId);
                    prizeAssociate = (DtoPrizeAssociateAttendance)mapping.dtoList.FirstOrDefault().Dto;
                }

                return prizeAssociate;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize Associate Attendance", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadPrizeAssociateAttendance");
                throw ex;
            }
        }
        /// <summary>
        /// Load Prize Associate Attendance List
        /// </summary>
        /// <returns>Load Prize Associate Attendance List</returns>
        public List<DtoPrizeAssociateAttendance> LoadPrizeAssociateAttendanceList(long prizeAssociateId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoPrizeAssociateAttendance> prizeAssociateList = new List<DtoPrizeAssociateAttendance>();

                mapping.Load<DtoPrizeAssociateAttendance>("SELECT PrizeAssociateAttendanceId, Year, Month, Points, PrizeAssociateId FROM PrizeAssociateAttendance WHERE PrizeAssociateId = " + prizeAssociateId + " ORDER BY PrizeAssociateId", "PrizeAssociateAttendance", new DtoPrizeAssociateAttendance());
                prizeAssociateList.AddRange(mapping.dtoList.Select(renglon => (DtoPrizeAssociateAttendance)renglon.Dto));

                return prizeAssociateList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize Associate Attendance List", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadPrizeAssociateAttendanceList");
                throw ex;
            }
        }
        /// <summary>
        /// Loads the Prize Associate Finding DTO
        /// </summary>
        /// <param name="prizeAssociateFindingId"></param>
        /// <returns>DtoPrizeAssociate Loaded</returns>
        public DtoPrizeAssociateFinding LoadPrizeAssociateFinding(long prizeAssociateFindingId)
        {
            DtoPrizeAssociateFinding prizeAssociate = new DtoPrizeAssociateFinding();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (prizeAssociateFindingId != 0)
                {
                    mapping.Load<DtoPrizeAssociateFinding>("PrizeAssociateFindings", new DtoPrizeAssociateFinding(), "PrizeAssociateFindingId=" + prizeAssociateFindingId);
                    prizeAssociate = (DtoPrizeAssociateFinding)mapping.dtoList.FirstOrDefault().Dto;
                }

                return prizeAssociate;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize Associate Finding", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadPrizeAssociateFinding");
                throw ex;
            }
        }
        /// <summary>
        /// Load Prize Associate Finding List
        /// </summary>
        /// <returns>Load Prize Associate Finding List</returns>
        public List<DtoPrizeAssociateFinding> LoadPrizeAssociateFindingList(long prizeAssociateId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoPrizeAssociateFinding> prizeAssociateList = new List<DtoPrizeAssociateFinding>();

                mapping.Load<DtoPrizeAssociateFinding>("SELECT PrizeAssociateFindingId, Year, Month, Finding, Points, PrizeAssociateId FROM PrizeAssociateFindings WHERE PrizeAssociateId = " + prizeAssociateId + " ORDER BY PrizeAssociateId", "PrizeAssociateFindings", new DtoPrizeAssociateFinding());
                prizeAssociateList.AddRange(mapping.dtoList.Select(renglon => (DtoPrizeAssociateFinding)renglon.Dto));

                return prizeAssociateList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize Associate Finding List", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadPrizeAssociateFindingList");
                throw ex;
            }
        }
        /// <summary>
        /// Loads the Prize Associate Prize DTO
        /// </summary>
        /// <param name="prizeAssociatePrizeId"></param>
        /// <returns>DtoPrizeAssociate Loaded</returns>
        public DtoPrizeAssociatePrize LoadPrizeAssociatePrize(long prizeAssociatePrizeId)
        {
            DtoPrizeAssociatePrize prizeAssociate = new DtoPrizeAssociatePrize();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (prizeAssociatePrizeId != 0)
                {
                    mapping.Load<DtoPrizeAssociatePrize>("PrizeAssociatePrizes", new DtoPrizeAssociatePrize(), "PrizeAssociatePrizeId=" + prizeAssociatePrizeId);
                    prizeAssociate = (DtoPrizeAssociatePrize)mapping.dtoList.FirstOrDefault().Dto;
                }

                return prizeAssociate;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize Associate Prize", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadPrizeAssociatePrize");
                throw ex;
            }
        }
        /// <summary>
        /// Load Prize Associate Prizes List
        /// </summary>
        /// <returns>Load Prize Associate Prizes List</returns>
        public List<DtoPrizeAssociatePrize> LoadPrizeAssociatePrizeList(long prizeAssociateId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoPrizeAssociatePrize> prizeAssociateList = new List<DtoPrizeAssociatePrize>();

                mapping.Load<DtoPrizeAssociatePrize>("SELECT PrizeAssociatePrizeId, Year, Month, PrizeId, Prize, Points, PrizeAssociateId, PrizeLogId, Date FROM PrizeAssociatePrizes WHERE PrizeAssociateId = " + prizeAssociateId + " ORDER BY PrizeAssociateId", "PrizeAssociatePrizes", new DtoPrizeAssociatePrize());
                prizeAssociateList.AddRange(mapping.dtoList.Select(renglon => (DtoPrizeAssociatePrize)renglon.Dto));

                return prizeAssociateList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize Associate Prizes List", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadPrizeAssociatePrizeList");
                throw ex;
            }
        }
        /// <summary>
        /// Load Prize Associate Prizes List
        /// </summary>
        /// <returns>Load Prize Associate Prizes List</returns>
        public List<DtoPrizeAssociatePointLog> LoadPrizeAssociatePointLogList(long prizeAssociateId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoPrizeAssociatePointLog> prizeAssociatePointLogList = new List<DtoPrizeAssociatePointLog>();

                mapping.Load<DtoPrizeAssociatePointLog>("SELECT PrizeAssociatePointLogId, PrizeAssociateId, MovementType, Motive, Associate, Quantity, Date FROM PrizeAssociatePointLog WHERE PrizeAssociateId = " + prizeAssociateId + " ORDER BY PrizeAssociateId", "PrizeAssociatePointLog", new DtoPrizeAssociatePointLog());
                prizeAssociatePointLogList.AddRange(mapping.dtoList.Select(renglon => (DtoPrizeAssociatePointLog)renglon.Dto));

                return prizeAssociatePointLogList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize Associate Prizes List", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadPrizeAssociatePrizeList");
                throw ex;
            }
        }
        #endregion

        #region General Methods   
        /// <summary>
        /// Get PrizeAssociate Associate Points
        /// </summary>
        /// <param name="prizeAssociateId"></param>
        /// <returns></returns>
        public int GetPrizeAssociatePoints(long prizeAssociateId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT AssociatePoints FROM PrizeAssociates WHERE PrizeAssociateId = " + prizeAssociateId;

                dataRow = oDatabase.GetRow(sqlString, "Get Associate Points");

                if (dataRow == null) return 0;

                return Convert.ToInt32(dataRow["AssociatePoints"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Prize Associate Points", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetPrizeAssociatePoints");
                throw ex;
            }

        }
        /// <summary>
        /// Get PrizeAssociate Id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public long GetPrizeAssociateId(string name)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT PrizeAssociateId FROM PrizeAssociates WHERE Associate = '" + name + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get PrizeAssociateId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["PrizeAssociateId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get PrizeAssociate Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetPrizeAssociateId");
                throw ex;
            }

        }
        /// <summary>
        /// Get Prize Associate Name
        /// </summary>
        /// <param name="prizeAssociateId"></param>
        /// <returns></returns>
        public string GetPrizeAssociateName(long prizeAssociateId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT Associate FROM PrizeAssociates WHERE PrizeAssociateId = " + prizeAssociateId;

                dataRow = oDatabase.GetRow(sqlString, "Get Prize Associate Names");

                if (dataRow == null) return "";

                return dataRow["Associate"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Prize Associate Name", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetPrizeAssociateName");
                throw ex;
            }

        }
        /// <summary>
        /// Deletes a PrizeAssociate. 
        /// </summary>
        /// <param name="prizeAssociateId"></param>
        /// <returns></returns>
        public bool DeletePrizeAssociate(long prizeAssociateId)
        {
            try
            {
                string query = "DELETE PrizeAssociates WHERE PrizeAssociateId =" + prizeAssociateId;
                oDatabase.ExecuteNonQuery(query, "Remove PrizeAssociate");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Prize Associate", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeletePrizeAssociate");
                return false;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtoWorkers"></param>
        public void DeleteInactiveWorkers(List<DtoWorkerFile> dtoWorkers)
        {
            try
            {
                List<string> deleteList = new List<string>();
                foreach (var item in dtoWorkers)
                {
                    deleteList.Add("DELETE PrizeAssociates WHERE WorkerFileId =" + item.WorkerFileId);
                }
                oDatabase.ExecuteMultipleNonQueryList(deleteList, "Remove Inactive workers");
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Inactive Workers", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteInactiveWorkers");
            }
        }
        /// <summary>
        /// Get Prize Associate Attendance Id
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public long GetPrizeAssociateAttendanceId(string month, string year, long prizeAssociateId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT PrizeAssociateAttendanceId FROM PrizeAssociateAttendance WHERE Month = '" + month + "' AND Year = '" + year + "' AND PrizeAssociateId = " + prizeAssociateId;

                dataRow = oDatabase.GetRow(sqlString, "Get PrizeAssociateId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["PrizeAssociateAttendanceId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Prize Associate Attendance Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetPrizeAssociateAttendanceId");
                throw ex;
            }

        }
        /// <summary>
        /// Deletes a Prize Associate Attendance. 
        /// </summary>
        /// <param name="prizeAssociateAttendanceId"></param>
        /// <returns></returns>
        public bool DeletePrizeAssociateAttendance(long prizeAssociateAttendanceId)
        {
            try
            {
                string query = "DELETE PrizeAssociateAttendance WHERE PrizeAssociateAttendanceId =" + prizeAssociateAttendanceId;
                oDatabase.ExecuteNonQuery(query, "Remove PrizeAssociateAttendance");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Prize Associate Attendance", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeletePrizeAssociateAttendance");
                return false;
            }

        }
        /// <summary>
        /// Register Attendance 
        /// </summary>
        /// <param name="prizeAssociate"></param>
        /// <returns>Attendance registered</returns>
        public void SaveAttendance(DtoPrizeAssociateAttendance prizeAssociate)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = prizeAssociate, TableName = "PrizeAssociateAttendance" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Attendance", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveAttendance");
                throw ex;
            }
        }
        /// <summary>
        /// Prize Associate Month Id
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public long GetPrizeAssociateMonthId(string month, string year, long prizeAssociateId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT PrizeAssociateMonthId FROM PrizeAssociateMonth WHERE Month = '" + month + "' AND Year = '" + year + "' AND PrizeAssociateId = " + prizeAssociateId;

                dataRow = oDatabase.GetRow(sqlString, "Get PrizeAssociateMonthId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["PrizeAssociateMonthId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Prize Associate Month Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetPrizeAssociateMonthId");
                throw ex;
            }

        }
        /// <summary>
        /// Register AssociateMonth 
        /// </summary>
        /// <param name="prizeAssociate"></param>
        /// <returns>AssociateMonth registered</returns>
        public void SaveAssociateMonth(DtoPrizeAssociateMonth prizeAssociate)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = prizeAssociate, TableName = "PrizeAssociateMonth" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Associate Month", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveAssociateMonth");
                throw ex;
            }
        }
        /// <summary>
        /// Deletes a Prize Associate Attendance. 
        /// </summary>
        /// <param name="prizeAssociateMonthId"></param>
        /// <returns></returns>
        public bool DeletePrizeAssociateMonth(long prizeAssociateMonthId)
        {
            try
            {
                string query = "DELETE PrizeAssociateMonth WHERE PrizeAssociateMonthId =" + prizeAssociateMonthId;
                oDatabase.ExecuteNonQuery(query, "Remove PrizeAssociateMonth");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Prize Associate Month", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeletePrizeAssociateMonth");
                return false;
            }

        }
        /// <summary>
        /// Prize Associate Year Id
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public long GetPrizeAssociateYearId(string year, long prizeAssociateId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT PrizeAssociateYearId FROM PrizeAssociateYear WHERE Year = '" + year + "' AND PrizeAssociateId = " + prizeAssociateId;

                dataRow = oDatabase.GetRow(sqlString, "Get PrizeAssociateYearId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["PrizeAssociateYearId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Prize Associate Year Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetPrizeAssociateYearId");
                throw ex;
            }

        }
        /// <summary>
        /// Register Associate Year 
        /// </summary>
        /// <param name="prizeAssociate"></param>
        /// <returns>Associate Year registered</returns>
        public void SaveAssociateYear(DtoPrizeAssociateYear prizeAssociate)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = prizeAssociate, TableName = "PrizeAssociateYear" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Prize Associate Year", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveAssociateYear");
                throw ex;
            }
        }
        /// <summary>
        /// Deletes a Prize Associate Year. 
        /// </summary>
        /// <param name="prizeAssociateYearId"></param>
        /// <returns></returns>
        public bool DeletePrizeAssociateYear(long prizeAssociateYearId)
        {
            try
            {
                string query = "DELETE PrizeAssociateYear WHERE PrizeAssociateYearId =" + prizeAssociateYearId;
                oDatabase.ExecuteNonQuery(query, "Remove PrizeAssociateYear");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Prize Associate Year", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeletePrizeAssociateYear");
                return false;
            }

        }
        /// <summary>
        /// Get Prize Associate Finding Id
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="finding"></param>
        /// <returns></returns>
        public long GetPrizeAssociateFindingId(string month, string year, string finding, long prizeAssociateId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT PrizeAssociateFindingId FROM PrizeAssociateFindings WHERE Month = '" + month + "' AND Year = '" + year + "' AND Finding = '" + finding + "' AND PrizeAssociateId = " + prizeAssociateId;

                dataRow = oDatabase.GetRow(sqlString, "Get PrizeAssociateFindingId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["PrizeAssociateFindingId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Prize Associate Finding Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetPrizeAssociateFindingId");
                throw ex;
            }

        }
        /// <summary>
        /// Register Finding 
        /// </summary>
        /// <param name="prizeAssociate"></param>
        /// <returns>Finding registered</returns>
        public void SaveFinding(DtoPrizeAssociateFinding prizeAssociate)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = prizeAssociate, TableName = "PrizeAssociateFindings" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Finding", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveFinding");
                throw ex;
            }
        }
        /// <summary>
        /// Deletes a Prize Associate Finding. 
        /// </summary>
        /// <param name="prizeAssociateFindingId"></param>
        /// <returns></returns>
        public bool DeletePrizeAssociateFinding(long prizeAssociateFindingId)
        {
            try
            {
                string query = "DELETE PrizeAssociateFindings WHERE PrizeAssociateFindingId =" + prizeAssociateFindingId;
                oDatabase.ExecuteNonQuery(query, "Remove PrizeAssociateFinding");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Prize Associate Finding", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeletePrizeAssociateFinding");
                return false;
            }

        }
        /// <summary>
        /// Get Prize Associate Prize Id
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="prize"></param>
        /// <returns></returns>
        public long GetPrizeAssociatePrizeId(string month, string year, string prize, long prizeAssociateId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT PrizeAssociatePrizeId FROM PrizeAssociatePrizes WHERE Month = '" + month + "' AND Year = '" + year + "' AND Prize = '" + prize + "' AND PrizeAssociateId = " + prizeAssociateId;

                dataRow = oDatabase.GetRow(sqlString, "Get PrizeAssociatePrizeId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["PrizeAssociatePrizeId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Prize Associate Prize Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetPrizeAssociatePrizeId");
                throw ex;
            }

        }
        /// <summary>
        /// Register Prize 
        /// </summary>
        /// <param name="prizeAssociate"></param>
        /// <returns>Prize registered</returns>
        public void SavePrize(DtoPrizeAssociatePrize prizeAssociate)
        {
            try
            {
                prizeAssociate.Date = DateTime.Now;
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = prizeAssociate, TableName = "PrizeAssociatePrizes" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Prize Associate Prizes", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SavePrize");
                throw ex;
            }
        }
        /// <summary>
        /// Deletes a Prize Associate Prize. 
        /// </summary>
        /// <param name="prizeAssociatePrizeId"></param>
        /// <returns></returns>
        public bool DeletePrizeAssociatePrize(long prizeAssociatePrizeId)
        {
            try
            {
                string query = "DELETE PrizeAssociatePrizes WHERE PrizeAssociatePrizeId =" + prizeAssociatePrizeId;
                oDatabase.ExecuteNonQuery(query, "Remove PrizeAssociatePrize");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Prize Associate Prize", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeletePrizeAssociatePrize");
                return false;
            }

        }
        /// <summary>
        /// Loads Point Configuration DTO
        /// </summary>
        /// <param name="prizeAssociateConfigurationId"></param>
        /// <returns>Point Configuration Loaded</returns>
        public DtoPrizeAssociateConfiguration LoadPointConfiguration(long prizeAssociateConfigurationId)
        {
            DtoPrizeAssociateConfiguration prizeAssociate = new DtoPrizeAssociateConfiguration();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (prizeAssociateConfigurationId != 0)
                {
                    mapping.Load<DtoPrizeAssociateConfiguration>("PrizeAssociateConfiguration", new DtoPrizeAssociateConfiguration(), "PrizeAssociateConfigurationId=" + prizeAssociateConfigurationId);
                    prizeAssociate = (DtoPrizeAssociateConfiguration)mapping.dtoList.FirstOrDefault().Dto;
                }

                return prizeAssociate;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Point Configuration", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadPointConfiguration");
                throw ex;
            }
        }
        /// <summary>
        /// Save Point Configuration
        /// </summary>
        /// <param name="prizeAssociateConfiguration"></param>
        public void SavePointConfiguration(DtoPrizeAssociateConfiguration prizeAssociateConfiguration)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = prizeAssociateConfiguration, TableName = "PrizeAssociateConfiguration" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Prize Associate Configuration", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SavePointConfiguration");
                throw ex;
            }
        }
        /// <summary>
        /// Save Points
        /// </summary>
        /// <param name="prizeAssociateConfiguration"></param>
        public void SaveAssociatePoints(int associatePoints, int newPoints, long prizeAssociateId, bool isAdd)
        {
            try
            {
                int totalPoints = isAdd ? associatePoints + newPoints : associatePoints - newPoints;
                string query = "UPDATE PrizeAssociates SET AssociatePoints = " + totalPoints + "  WHERE PrizeAssociateId =" + prizeAssociateId;

                oDatabase.ExecuteNonQuery(query, "Add/Remove Prize Points");
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Associate Points", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveAssociatePoints");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void AddPrizeAssociates()
        {
            try
            {
                WorkerFile workerFile = new WorkerFile();
                List<DtoWorkerFile> workers = workerFile.LoadMultipleUnionWorkers(true);
                List<DtoPrizeAssociate> actualPrizeWorkers = LoadMultiple();
                List<DtoPrizeAssociate> prizeWorkers = new List<DtoPrizeAssociate>();

                if (actualPrizeWorkers.Count > 0)
                {
                    List<DtoWorkerFile> newWorkers = workers.Where(x => !actualPrizeWorkers.Any(y => y.WorkerFileId == x.WorkerFileId)).ToList();
                    foreach (var item in newWorkers)
                    {
                        prizeWorkers.Add(new DtoPrizeAssociate
                        {
                            Associate = item.Names,
                            Department = item.Process,
                            WorkerFileId = item.WorkerFileId,
                            Team = "",
                            AssociatePoints = 0,
                            TeamPoints = 0
                        });
                    }
                }
                else
                {
                    foreach (var item in workers)
                    {
                        prizeWorkers.Add(new DtoPrizeAssociate
                        {
                            Associate = item.Names,
                            Department = item.Process,
                            WorkerFileId = item.WorkerFileId,
                            Team = "",
                            AssociatePoints = 0,
                            TeamPoints = 0
                        });
                    }
                }
                if (prizeWorkers.Count > 0)
                {
                    SaveMultiple(prizeWorkers);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void DeleteInactivePrizeAssociates()
        {
            WorkerFile workerFile = new WorkerFile();
            List<DtoWorkerFile> inactiveWorkers = workerFile.LoadMultipleUnionWorkers(false);
            List<DtoPrizeAssociate> actualPrizeWorkers = LoadMultiple();

            if (actualPrizeWorkers.Count > 0)
            {
                List<DtoWorkerFile> inactiveAssociates = inactiveWorkers.Where(x => actualPrizeWorkers.Any(y => y.WorkerFileId == x.WorkerFileId) && x.Status == false).ToList();
                DeleteInactiveWorkers(inactiveAssociates);
            }
        }
        /// <summary>
        /// Deletes a Prize Associate Point Log 
        /// </summary>
        /// <param name="prizeAssociatePointLogId"></param>
        /// <returns></returns>
        public bool DeletePrizeAssociatePointLog(long prizeAssociatePointLogId)
        {
            try
            {
                string query = "DELETE PrizeAssociatePointLog WHERE PrizeAssociatePointLogId =" + prizeAssociatePointLogId;
                oDatabase.ExecuteNonQuery(query, "Remove Prize Associate Point log");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Prize Associate Point Log", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeletePrizeAssociatePointLog");
                return false;
            }

        }
        /// <summary>
        /// Save Prize Associate Point Log
        /// </summary>
        /// <param name="prizeAssociatePointLog"></param>
        public void SavePrizeAssociatePointLog(DtoPrizeAssociatePointLog prizeAssociatePointLog)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = prizeAssociatePointLog, TableName = "PrizeAssociatePointLog" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Prize Associate Point Log", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SavePrizeAssociatePointLog");
                throw ex;
            }
        }
        #endregion
    }
}
