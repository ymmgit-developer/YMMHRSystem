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
    public class PrizeTeam
    {
        Log log = new Log();
        User user = new User();
        SQLTools oDatabase = new SQLTools();
        #region Standard Methods
        /// <summary>
        /// Loads the PrizeTeam DTO
        /// </summary>
        /// <param name="prizeTeamId"></param>
        /// <returns>DtoPrizeTeam Loaded</returns>
        public DtoPrizeTeam Load(long prizeTeamId)
        {
            DtoPrizeTeam prizeTeam = new DtoPrizeTeam();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (prizeTeamId != 0)
                {
                    mapping.Load<DtoPrizeTeam>("PrizeTeam", new DtoPrizeTeam(), "PrizeTeamId=" + prizeTeamId);
                    prizeTeam = (DtoPrizeTeam)mapping.dtoList.FirstOrDefault().Dto;
                }

                prizeTeam.AssociateList = LoadPrizeTeamAssociateList(prizeTeamId);
                prizeTeam.AttendanceList = LoadPrizeTeamAttendanceList(prizeTeamId);
                prizeTeam.DockAuditList = LoadPrizeTeamDockAuditList(prizeTeamId);
                prizeTeam.ProductivityList = LoadPrizeTeamProductivityList(prizeTeamId);
                prizeTeam.SecurityCrossList = LoadPrizeTeamSecurityCrossList(prizeTeamId);
                prizeTeam.ScrapList = LoadPrizeTeamScrapList(prizeTeamId);

                return prizeTeam;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize Team", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Register PrizeTeam 
        /// </summary>
        /// <param name="prizeTeam"></param>
        /// <returns>PrizeTeam registered</returns>
        public void Save(DtoPrizeTeam prizeTeam)
        {
            try
            {
                if (prizeTeam.PrizeTeamId == 0)
                {
                    prizeTeam.CreatedBy = user.GetUserName(SQLTools.userId.ToString());
                    prizeTeam.DateAdded = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = prizeTeam, TableName = "PrizeTeam" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save PrizeTeam", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple PrizeTeam with fields.
        /// </summary>
        /// <returns>Load PrizeTeam Dto</returns>
        public List<DtoPrizeTeam> LoadMultiple()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoPrizeTeam> prizeTeamList = new List<DtoPrizeTeam>();

                mapping.Load<DtoPrizeTeam>("SELECT PrizeTeamId, Name, DateAdded, CreatedBy, TeamPoints FROM PrizeTeam ORDER BY PrizeTeamId", "PrizeTeam", new DtoPrizeTeam());
                prizeTeamList.AddRange(mapping.dtoList.Select(renglon => (DtoPrizeTeam)renglon.Dto));

                return prizeTeamList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple PrizeTeam", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple PrizeTeam with fields.
        /// </summary>
        /// <returns>Load PrizeTeam Dto</returns>
        public List<DtoPrizeTeamAssociate> LoadMultipleTeamAssociates(long prizeTeamId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoPrizeTeamAssociate> prizeTeamList = new List<DtoPrizeTeamAssociate>();

                mapping.Load<DtoPrizeTeamAssociate>("SELECT PrizeTeamAssociateId, PrizeAssociateId, PrizeTeamId, Associate FROM PrizeTeamAssociates WHERE PrizeTeamId = " + prizeTeamId, "PrizeTeamAssociates", new DtoPrizeTeamAssociate());
                prizeTeamList.AddRange(mapping.dtoList.Select(renglon => (DtoPrizeTeamAssociate)renglon.Dto));

                return prizeTeamList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Team Associates", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleTeamAssociates");
                throw ex;
            }
        }
        /// <summary>
        /// Loads the Prize Team Productivity DTO
        /// </summary>
        /// <param name="prizeTeamProductivityId"></param>
        /// <returns>DtoPrizeTeam Loaded</returns>
        public DtoPrizeTeamProductivity LoadPrizeTeamProductivity(long prizeTeamProductivityId)
        {
            DtoPrizeTeamProductivity prizeTeamProductivity = new DtoPrizeTeamProductivity();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (prizeTeamProductivityId != 0)
                {
                    mapping.Load<DtoPrizeTeamProductivity>("PrizeTeamProductivity", new DtoPrizeTeamProductivity(), "PrizeTeamProductivityId=" + prizeTeamProductivityId);
                    prizeTeamProductivity = (DtoPrizeTeamProductivity)mapping.dtoList.FirstOrDefault().Dto;
                }

                return prizeTeamProductivity;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize Team Productivity", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadPrizeTeamProductivity");
                throw ex;
            }
        }
        /// <summary>
        /// Load Prize Team Productivity List
        /// </summary>
        /// <returns>Load Prize Team Productivity List</returns>
        public List<DtoPrizeTeamProductivity> LoadPrizeTeamProductivityList(long prizeTeamId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoPrizeTeamProductivity> prizeTeamList = new List<DtoPrizeTeamProductivity>();

                mapping.Load<DtoPrizeTeamProductivity>("SELECT PrizeTeamProductivityId, Year, Month, Points, PrizeTeamId FROM PrizeTeamProductivity WHERE PrizeTeamId = " + prizeTeamId + " ORDER BY PrizeTeamId", "PrizeTeamProductivity", new DtoPrizeTeamProductivity());
                prizeTeamList.AddRange(mapping.dtoList.Select(renglon => (DtoPrizeTeamProductivity)renglon.Dto));

                return prizeTeamList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize Associate Productivity List", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadPrizeTeamProductivityList");
                throw ex;
            }
        }
        /// <summary>
        /// Loads the Prize Team Scrap DTO
        /// </summary>
        /// <param name="prizeTeamScrapId"></param>
        /// <returns>DtoPrizeTeam Loaded</returns>
        public DtoPrizeTeamScrap LoadPrizeTeamScrap(long prizeTeamScrapId)
        {
            DtoPrizeTeamScrap prizeTeam = new DtoPrizeTeamScrap();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (prizeTeamScrapId != 0)
                {
                    mapping.Load<DtoPrizeTeamScrap>("PrizeTeamScrap", new DtoPrizeTeamScrap(), "PrizeTeamScrapId=" + prizeTeamScrapId);
                    prizeTeam = (DtoPrizeTeamScrap)mapping.dtoList.FirstOrDefault().Dto;
                }

                return prizeTeam;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize Team Scrap", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadPrizeTeamScrap");
                throw ex;
            }
        }
        /// <summary>
        /// Load Prize Team Scrap List
        /// </summary>
        /// <returns>Load Prize Team Scrap List</returns>
        public List<DtoPrizeTeamScrap> LoadPrizeTeamScrapList(long prizeTeamId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoPrizeTeamScrap> prizeTeamList = new List<DtoPrizeTeamScrap>();

                mapping.Load<DtoPrizeTeamScrap>("SELECT PrizeTeamScrapId, Year, Month, Points, PrizeTeamId FROM PrizeTeamScrap WHERE PrizeTeamId = " + prizeTeamId + " ORDER BY PrizeTeamId", "PrizeTeamScrap", new DtoPrizeTeamScrap());
                prizeTeamList.AddRange(mapping.dtoList.Select(renglon => (DtoPrizeTeamScrap)renglon.Dto));

                return prizeTeamList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize Team Scrap List", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadPrizeTeamScrapList");
                throw ex;
            }
        }
        /// <summary>
        /// Loads the Prize Team Attendance DTO
        /// </summary>
        /// <param name="prizeTeamAttendanceId"></param>
        /// <returns>DtoPrizeTeam Loaded</returns>
        public DtoPrizeTeamAttendance LoadPrizeTeamAttendance(long prizeTeamAttendanceId)
        {
            DtoPrizeTeamAttendance prizeTeam = new DtoPrizeTeamAttendance();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (prizeTeamAttendanceId != 0)
                {
                    mapping.Load<DtoPrizeTeamAttendance>("PrizeTeamAttendance", new DtoPrizeTeamAttendance(), "PrizeTeamAttendanceId=" + prizeTeamAttendanceId);
                    prizeTeam = (DtoPrizeTeamAttendance)mapping.dtoList.FirstOrDefault().Dto;
                }

                return prizeTeam;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize Team Attendance", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadPrizeTeamAttendance");
                throw ex;
            }
        }
        /// <summary>
        /// Load Prize Team Attendance List
        /// </summary>
        /// <returns>Load Prize Team Attendance List</returns>
        public List<DtoPrizeTeamAttendance> LoadPrizeTeamAttendanceList(long prizeTeamId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoPrizeTeamAttendance> prizeTeamList = new List<DtoPrizeTeamAttendance>();

                mapping.Load<DtoPrizeTeamAttendance>("SELECT PrizeTeamAttendanceId, Year, Month, Points, PrizeTeamId FROM PrizeTeamAttendance WHERE PrizeTeamId = " + prizeTeamId + " ORDER BY PrizeTeamId", "PrizeTeamAttendance", new DtoPrizeTeamAttendance());
                prizeTeamList.AddRange(mapping.dtoList.Select(renglon => (DtoPrizeTeamAttendance)renglon.Dto));

                return prizeTeamList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize Team Attendance List", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadPrizeTeamAttendanceList");
                throw ex;
            }
        }
        /// <summary>
        /// Loads the Prize Team DockAudit DTO
        /// </summary>
        /// <param name="prizeTeamDockAuditId"></param>
        /// <returns>DtoPrizeTeam Loaded</returns>
        public DtoPrizeTeamDockAudit LoadPrizeTeamDockAudit(long prizeTeamDockAuditId)
        {
            DtoPrizeTeamDockAudit prizeTeam = new DtoPrizeTeamDockAudit();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (prizeTeamDockAuditId != 0)
                {
                    mapping.Load<DtoPrizeTeamDockAudit>("PrizeTeamDockAudit", new DtoPrizeTeamDockAudit(), "PrizeTeamDockAuditId=" + prizeTeamDockAuditId);
                    prizeTeam = (DtoPrizeTeamDockAudit)mapping.dtoList.FirstOrDefault().Dto;
                }

                return prizeTeam;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize Team Dock Audit", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadPrizeTeamDockAudit");
                throw ex;
            }
        }
        /// <summary>
        /// Load Prize Team Dock Audit List
        /// </summary>
        /// <returns>Load Prize Team Dock Audit List</returns>
        public List<DtoPrizeTeamDockAudit> LoadPrizeTeamDockAuditList(long prizeTeamId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoPrizeTeamDockAudit> prizeTeamList = new List<DtoPrizeTeamDockAudit>();

                mapping.Load<DtoPrizeTeamDockAudit>("SELECT PrizeTeamDockAuditId, Year, Month, Points, PrizeTeamId FROM PrizeTeamDockAudit WHERE PrizeTeamId = " + prizeTeamId + " ORDER BY PrizeTeamId", "PrizeTeamDockAudit", new DtoPrizeTeamDockAudit());
                prizeTeamList.AddRange(mapping.dtoList.Select(renglon => (DtoPrizeTeamDockAudit)renglon.Dto));

                return prizeTeamList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize Team Dock Audit List", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadPrizeTeamDockAuditList");
                throw ex;
            }
        }
        /// <summary>
        /// Loads the Prize Team Security Cross DTO
        /// </summary>
        /// <param name="prizeTeamPrizeId"></param>
        /// <returns>DtoPrizeTeam Loaded</returns>
        public DtoPrizeTeamSecurityCross LoadPrizeTeamSecurityCross(long prizeTeamSecurityCrossId)
        {
            DtoPrizeTeamSecurityCross prizeTeam = new DtoPrizeTeamSecurityCross();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (prizeTeamSecurityCrossId != 0)
                {
                    mapping.Load<DtoPrizeTeamSecurityCross>("PrizeTeamSecurityCross", new DtoPrizeTeamSecurityCross(), "PrizeTeamSecurityCrossId=" + prizeTeamSecurityCrossId);
                    prizeTeam = (DtoPrizeTeamSecurityCross)mapping.dtoList.FirstOrDefault().Dto;
                }

                return prizeTeam;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize Team Security Cross", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadPrizeTeamSecurityCross");
                throw ex;
            }
        }
        /// <summary>
        /// Load Prize Team Security Cross List
        /// </summary>
        /// <returns>Load Prize Team Security Cross List</returns>
        public List<DtoPrizeTeamSecurityCross> LoadPrizeTeamSecurityCrossList(long prizeTeamId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoPrizeTeamSecurityCross> prizeTeamList = new List<DtoPrizeTeamSecurityCross>();

                mapping.Load<DtoPrizeTeamSecurityCross>("SELECT PrizeTeamSecurityCrossId, Year, Month, Points, PrizeTeamId FROM PrizeTeamSecurityCross WHERE PrizeTeamId = " + prizeTeamId + " ORDER BY PrizeTeamId", "PrizeTeamSecurityCross", new DtoPrizeTeamSecurityCross());
                prizeTeamList.AddRange(mapping.dtoList.Select(renglon => (DtoPrizeTeamSecurityCross)renglon.Dto));

                return prizeTeamList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize Team Security Cross List", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadPrizeTeamSecurityCrossList");
                throw ex;
            }
        }
        /// <summary>
        /// Load Prize Team Point Log List
        /// </summary>
        /// <returns>Load Prize Team Point Log List</returns>
        public List<DtoPrizeTeamPointLog> LoadPrizeTeamPointLogList(long prizeTeamId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoPrizeTeamPointLog> prizeTeamPointLogList = new List<DtoPrizeTeamPointLog>();

                mapping.Load<DtoPrizeTeamPointLog>("SELECT PrizeTeamPointLogId, PrizeTeamId, MovementType, Quantity, Date FROM PrizeTeamPointLog WHERE PrizeTeamId = " + prizeTeamId + " ORDER BY PrizeTeamId", "PrizeTeamPointLog", new DtoPrizeTeamPointLog());
                prizeTeamPointLogList.AddRange(mapping.dtoList.Select(renglon => (DtoPrizeTeamPointLog)renglon.Dto));

                return prizeTeamPointLogList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize Team Point Log List", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadPrizeTeamPointLogList");
                throw ex;
            }
        }
        /// <summary>
        /// Load Prize Team Associate List
        /// </summary>
        /// <returns>Load Prize Team Associate List</returns>
        public List<DtoPrizeTeamAssociate> LoadPrizeTeamAssociateList(long prizeTeamId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoPrizeTeamAssociate> prizeTeamPointLogList = new List<DtoPrizeTeamAssociate>();

                mapping.Load<DtoPrizeTeamAssociate>("SELECT PrizeTeamAssociateId, PrizeAssociateId, PrizeTeamId, Associate FROM PrizeTeamAssociates WHERE PrizeTeamId = " + prizeTeamId + " ORDER BY PrizeTeamId", "PrizeTeamAssociates", new DtoPrizeTeamAssociate());
                prizeTeamPointLogList.AddRange(mapping.dtoList.Select(renglon => (DtoPrizeTeamAssociate)renglon.Dto));

                return prizeTeamPointLogList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Prize Team Associates List", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadPrizeTeamAssociateList");
                throw ex;
            }
        }
        #endregion

        #region General Methods   
        /// <summary>
        /// Get Prize Team Points
        /// </summary>
        /// <param name="prizeTeamId"></param>
        /// <returns></returns>
        public int GetPrizeTeamPoints(long prizeTeamId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT TeamPoints FROM PrizeTeam WHERE PrizeTeamId = " + prizeTeamId;

                dataRow = oDatabase.GetRow(sqlString, "Get Team Points");

                if (dataRow == null) return 0;

                return Convert.ToInt32(dataRow["TeamPoints"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Prize Team Points", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetPrizeTeamPoints");
                throw ex;
            }

        }
        /// <summary>
        /// Validate Prize Team Associate Relation
        /// </summary>
        /// <param name="prizeTeamId"></param>
        /// <returns></returns>
        public bool ValidatePrizeTeamAssociateRelation(long prizeAssociateId, long prizeTeamId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT PrizeTeamAssociateId FROM PrizeTeamAssociates WHERE PrizeAssociateId = " + prizeAssociateId + " AND PrizeTeamId = " + prizeTeamId;

                dataRow = oDatabase.GetRow(sqlString, "Get Team Associate Id");

                return dataRow == null ? false : true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Validate Prize Team Associate Relation", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "ValidatePrizeTeamAssociateRelation");
                throw ex;
            }

        }
        /// <summary>
        /// Update Associate Team
        /// </summary>
        /// <param name="prizeTeamId"></param>
        /// <returns></returns>
        public bool UpdateAssociateTeam(long prizeAssociateId, string team)
        {
            try
            {
                string sqlString = "UPDATE PrizeAssociates SET Team = '" + team + "' WHERE PrizeAssociateId = " + prizeAssociateId;
                oDatabase.ExecuteNonQuery(sqlString, "Update Associate Team");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Update Associate Team", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "UpdateAssociateTeam");
                throw ex;
            }

        }
        /// <summary>
        /// Deletes a Prize Team Associate. 
        /// </summary>
        /// <param name="prizeTeamAssociateId"></param>
        /// <returns></returns>
        public bool DeletePrizeTeamAssociate(long prizeTeamAssociateId, long prizeAssociateId)
        {
            try
            {
                string query = "DELETE PrizeTeamAssociates WHERE PrizeTeamAssociateId =" + prizeTeamAssociateId;
                oDatabase.ExecuteNonQuery(query, "Remove PrizeTeamProductivity");

                query = "UPDATE PrizeAssociates SET Team = '' WHERE PrizeAssociateId =" + prizeAssociateId;
                oDatabase.ExecuteNonQuery(query, "Remove Associate Team");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Prize Team Associate", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeletePrizeTeamAssociate");
                return false;
            }

        }
        /// <summary>
        /// Register Team Associate 
        /// </summary>
        /// <param name="prizeTeamAssociate"></param>
        /// <returns>Associate registered</returns>
        public void SavePrizeTeamAssociate(DtoPrizeTeamAssociate prizeTeamAssociate)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = prizeTeamAssociate, TableName = "PrizeTeamAssociates" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Prize Team Associate", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SavePrizeTeamAssociate");
                throw ex;
            }
        }
        /// <summary>
        /// Get Prize Associate Id
        /// </summary>
        /// <param name="prizeTeamAssociateId"></param>
        /// <returns></returns>
        public long GetPrizeAssociateId(long prizeTeamAssociateId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT PrizeAssociateId FROM PrizeTeamAssociates WHERE PrizeTeamAssociateId = " + prizeTeamAssociateId;

                dataRow = oDatabase.GetRow(sqlString, "Get Prize Associate Id");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["PrizeAssociateId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Prize Associate Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetPrizeAssociateId");
                throw ex;
            }

        }
        /// <summary>
        /// Get PrizeTeam Id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public long GetPrizeTeamId(string name)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT PrizeTeamId FROM PrizeTeam WHERE Name = '" + name + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get PrizeTeamId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["PrizeTeamId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get PrizeTeam Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetPrizeTeamId");
                throw ex;
            }

        }
        /// <summary>
        /// Get Prize Team Name
        /// </summary>
        /// <param name="prizeTeamId"></param>
        /// <returns></returns>
        public string GetPrizeTeamName(long prizeTeamId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT Name FROM PrizeTeam WHERE PrizeTeamId = " + prizeTeamId;

                dataRow = oDatabase.GetRow(sqlString, "Get Prize Team Name");

                if (dataRow == null) return "";

                return dataRow["Name"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Prize Team Name", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetPrizeTeamName");
                throw ex;
            }

        }
        /// <summary>
        /// Deletes a PrizeTeam. 
        /// </summary>
        /// <param name="prizeTeamId"></param>
        /// <returns></returns>
        public bool DeletePrizeTeam(long prizeTeamId)
        {
            try
            {
                string query = "DELETE PrizeTeam WHERE PrizeTeamId =" + prizeTeamId;
                oDatabase.ExecuteNonQuery(query, "Remove PrizeTeam");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Prize Team", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeletePrizeTeam");
                return false;
            }

        }
        /// <summary>
        /// Get Prize Team Attendance Id
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public long GetPrizeTeamAttendanceId(string month, string year, long prizeTeamId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT PrizeTeamAttendanceId FROM PrizeTeamAttendance WHERE Month = '" + month + "' AND Year = '" + year + "' AND PrizeTeamId = " + prizeTeamId;

                dataRow = oDatabase.GetRow(sqlString, "Get PrizeTeamId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["PrizeTeamAttendanceId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Prize Team Attendance Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetPrizeTeamAttendanceId");
                throw ex;
            }

        }
        /// <summary>
        /// Deletes a Prize Associate Attendance. 
        /// </summary>
        /// <param name="prizeTeamAttendanceId"></param>
        /// <returns></returns>
        public bool DeletePrizeTeamAttendance(long prizeTeamAttendanceId)
        {
            try
            {
                string query = "DELETE PrizeTeamAttendance WHERE PrizeTeamAttendanceId =" + prizeTeamAttendanceId;
                oDatabase.ExecuteNonQuery(query, "Remove PrizeTeamAttendance");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Prize Team Attendance", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeletePrizeTeamAttendance");
                return false;
            }

        }
        /// <summary>
        /// Register Attendance 
        /// </summary>
        /// <param name="prizeTeam"></param>
        /// <returns>Attendance registered</returns>
        public void SavePrizeTeamAttendance(DtoPrizeTeamAttendance prizeTeam)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = prizeTeam, TableName = "PrizeTeamAttendance" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Prize Team Attendance", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SavePrizeTeamAttendance");
                throw ex;
            }
        }
        /// <summary>
        /// Prize Team Productivity Id
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public long GetPrizeTeamProductivityId(string month, string year, long prizeTeamId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT PrizeTeamProductivityId FROM PrizeTeamProductivity WHERE Month = '" + month + "' AND Year = '" + year + "' AND PrizeTeamId = " + prizeTeamId;

                dataRow = oDatabase.GetRow(sqlString, "Get PrizeTeamProductivityId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["PrizeTeamProductivityId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Prize Team Productivity Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetPrizeTeamProductivityId");
                throw ex;
            }

        }
        /// <summary>
        /// Register Team Productivity 
        /// </summary>
        /// <param name="prizeTeam"></param>
        /// <returns>AssociateProductivity registered</returns>
        public void SavePrizeTeamProductivity(DtoPrizeTeamProductivity prizeTeam)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = prizeTeam, TableName = "PrizeTeamProductivity" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Prize Team Productivity", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SavePrizeTeamProductivity");
                throw ex;
            }
        }
        /// <summary>
        /// Deletes a Prize Team Attendance. 
        /// </summary>
        /// <param name="prizeTeamProductivityId"></param>
        /// <returns></returns>
        public bool DeletePrizeTeamProductivity(long prizeTeamProductivityId)
        {
            try
            {
                string query = "DELETE PrizeTeamProductivity WHERE PrizeTeamProductivityId =" + prizeTeamProductivityId;
                oDatabase.ExecuteNonQuery(query, "Remove PrizeTeamProductivity");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Prize Team Productivity", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeletePrizeTeamProductivity");
                return false;
            }

        }
        /// <summary>
        ///  Gets Prize Team Scrap Id
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="prizeTeamId"></param>
        /// <returns></returns>
        public long GetPrizeTeamScrapId(string year, string month, long prizeTeamId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT PrizeTeamScrapId FROM PrizeTeamScrap WHERE Month = '" + month + "' AND Year = '" + year + "' AND PrizeTeamId = " + prizeTeamId;

                dataRow = oDatabase.GetRow(sqlString, "Get PrizeTeamScrapId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["PrizeTeamScrapId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Prize Team Scrap Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetPrizeTeamScrapId");
                throw ex;
            }

        }
        /// <summary>
        /// Register Team Scrap 
        /// </summary>
        /// <param name="prizeTeam"></param>
        /// <returns>Associate Scrap registered</returns>
        public void SavePrizeTeamScrap(DtoPrizeTeamScrap prizeTeam)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = prizeTeam, TableName = "PrizeTeamScrap" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Prize Team Scrap", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SavePrizeTeamScrap");
                throw ex;
            }
        }
        /// <summary>
        /// Deletes a Prize Team Scrap. 
        /// </summary>
        /// <param name="prizeTeamScrapId"></param>
        /// <returns></returns>
        public bool DeletePrizeTeamScrap(long prizeTeamScrapId)
        {
            try
            {
                string query = "DELETE PrizeTeamScrap WHERE PrizeTeamScrapId =" + prizeTeamScrapId;
                oDatabase.ExecuteNonQuery(query, "Remove PrizeTeamScrap");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Prize Team Scrap", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeletePrizeTeamScrap");
                return false;
            }

        }
        /// <summary>
        /// Get Prize Team DockAudit Id
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="prizeTeamId"></param>
        /// <returns></returns>
        public long GetPrizeTeamDockAuditId(string month, string year, long prizeTeamId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT PrizeTeamDockAuditId FROM PrizeTeamDockAudits WHERE Month = '" + month + "' AND Year = '" + year + "' AND PrizeTeamId = " + prizeTeamId;

                dataRow = oDatabase.GetRow(sqlString, "Get PrizeTeamDockAuditId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["PrizeTeamDockAuditId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Prize Team DockAudit Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetPrizeTeamDockAuditId");
                throw ex;
            }

        }
        /// <summary>
        /// Register Dock Audit 
        /// </summary>
        /// <param name="prizeTeam"></param>
        /// <returns>DockAudit registered</returns>
        public void SavePrizeTeamDockAudit(DtoPrizeTeamDockAudit prizeTeam)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = prizeTeam, TableName = "PrizeTeamDockAudit" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Prize Team Dock Audit", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SavePrizeTeamDockAudit");
                throw ex;
            }
        }
        /// <summary>
        /// Deletes a Prize Team Dock Audit. 
        /// </summary>
        /// <param name="prizeTeamDockAuditId"></param>
        /// <returns></returns>
        public bool DeletePrizeTeamDockAudit(long prizeTeamDockAuditId)
        {
            try
            {
                string query = "DELETE PrizeTeamDockAudits WHERE PrizeTeamDockAuditId =" + prizeTeamDockAuditId;
                oDatabase.ExecuteNonQuery(query, "Remove Prize Team Dock Audit");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Prize Team DockAudit", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeletePrizeTeamDockAudit");
                return false;
            }

        }
        /// <summary>
        /// Get Prize Team Security Cross Id
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="prizeTeamId"></param>
        /// <returns></returns>
        public long GetPrizeTeamSecurityCrossId(string month, string year, long prizeTeamId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT PrizeTeamSecurityCrossId FROM PrizeTeamSecurityCross WHERE Month = '" + month + "' AND Year = '" + year + "' AND PrizeTeamId = " + prizeTeamId;

                dataRow = oDatabase.GetRow(sqlString, "Get PrizeTeamSecurityCrossId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["PrizeTeamSecurityCrossId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Prize Team Security Cross Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetPrizeTeamSecurityCrossId");
                throw ex;
            }

        }
        /// <summary>
        /// Save Prize Team Security Cross
        /// </summary>
        /// <param name="prizeTeam"></param>
        /// <returns>Prize registered</returns>
        public void SavePrizeTeamSecurityCross(DtoPrizeTeamSecurityCross prizeTeam)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = prizeTeam, TableName = "PrizeTeamSecurityCross" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Prize Team Security Cross", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SavePrizeTeamSecurityCross");
                throw ex;
            }
        }
        /// <summary>
        /// Deletes a Prize Team Security Cross. 
        /// </summary>
        /// <param name="prizeTeamSecurityCrossId"></param>
        /// <returns></returns>
        public bool DeletePrizeTeamSecurityCross(long prizeTeamSecurityCrossId)
        {
            try
            {
                string query = "DELETE PrizeTeamSecurityCross WHERE PrizeTeamSecurityCrossId =" + prizeTeamSecurityCrossId;
                oDatabase.ExecuteNonQuery(query, "Remove PrizeTeamSecurityCross");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Prize Team Security Cross", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeletePrizeTeamSecurityCross");
                return false;
            }

        }
        /// <summary>
        /// Loads Point Configuration DTO
        /// </summary>
        /// <param name="prizeTeamConfigurationId"></param>
        /// <returns>Point Configuration Loaded</returns>
        public DtoPrizeTeamConfiguration LoadPointConfiguration(long prizeTeamConfigurationId)
        {
            DtoPrizeTeamConfiguration prizeTeam = new DtoPrizeTeamConfiguration();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (prizeTeamConfigurationId != 0)
                {
                    mapping.Load<DtoPrizeTeamConfiguration>("PrizeTeamConfiguration", new DtoPrizeTeamConfiguration(), "PrizeTeamConfigurationId=" + prizeTeamConfigurationId);
                    prizeTeam = (DtoPrizeTeamConfiguration)mapping.dtoList.FirstOrDefault().Dto;
                }

                return prizeTeam;
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
        /// <param name="prizeTeamConfiguration"></param>
        public void SavePointConfiguration(DtoPrizeTeamConfiguration prizeTeamConfiguration)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = prizeTeamConfiguration, TableName = "PrizeTeamConfiguration" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Prize Team Configuration", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SavePointConfiguration");
                throw ex;
            }
        }
        /// <summary>
        /// Save Points
        /// </summary>
        /// <param name="prizeTeamConfiguration"></param>
        public void SaveTeamPoints(int teamPoints, int newPoints, long prizeTeamId, bool isAdd)
        {
            try
            {
                int totalPoints = isAdd ? teamPoints + newPoints : teamPoints - newPoints;
                string query = "UPDATE PrizeTeam SET TeamPoints = " + totalPoints + "  WHERE PrizeTeamId =" + prizeTeamId;

                oDatabase.ExecuteNonQuery(query, "Add/Remove Prize Points");
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Team Points", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveAssociatePoints");
            }
        }
        /// <summary>
        /// Save multiple Prize Associates
        /// </summary>
        /// <param name="prizeAssociate"></param>
        public void SaveMultipleAssociateTeamPoints(List<DtoPrizeTeamAssociate> prizeTeamAssociate, int quantity, bool isAdd)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                PrizeAssociate prizeAssociate = new PrizeAssociate();
                List<DtoPrizeAssociate> prizeAssociates = prizeAssociate.LoadMultiple();

                var teamAssociates = prizeAssociates.Where(x => prizeTeamAssociate.Any(y => y.PrizeAssociateId == x.PrizeAssociateId)).ToList();

                teamAssociates.ForEach(item =>
                {
                    if (isAdd)
                    {
                        item.TeamPoints = item.TeamPoints + quantity;
                    }
                    else
                    {
                        item.TeamPoints = item.TeamPoints - quantity;
                        if (item.TeamPoints < 0)
                        {
                            item.TeamPoints = 0;
                        }
                    }
                    mapping.dtoList.Add(new DBFrameworkDto() { Dto = item, TableName = "PrizeAssociates" });
                });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Multiple Associate Team Points", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveMultipleAssociateTeamPoints");
                throw ex;
            }
        }
        /// <summary>
        /// Save Prize Team Point Log
        /// </summary>
        /// <param name="prizeTeamPointLog"></param>
        public void SavePrizeTeamPointLog(DtoPrizeTeamPointLog prizeTeamPointLog)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = prizeTeamPointLog, TableName = "PrizeTeamPointLog" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Prize Team Point Log", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SavePrizeTeamPointLog");
                throw ex;
            }
        }
        #endregion
    }
}
