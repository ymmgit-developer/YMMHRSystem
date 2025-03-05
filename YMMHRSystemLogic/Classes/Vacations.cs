using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using YMMHRSystemLogic;
using DBFramework;
using System.Data.SqlTypes;
using System.Web.Mvc;
using System.Windows.Media;
using System.Xml.Linq;
using YMMHRSystemLogic.Classes;

namespace YMMHRSystemLogic
{
    public class Vacations
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        HistoryAnniversary historyAnniversary = new HistoryAnniversary();
        VacationDayUsage vacationDayUsage = new VacationDayUsage();
        
        #region General methods
        public bool Save(DtoVacations vacation)
        {
            try
            {
                User user = new User();
                if (vacation.VacationId == 0)
                {
                    vacation.DateRequest = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = vacation, TableName = "Vacations" });
                mapping.Save();
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Record Vacations", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                return false;
            }
        }
        public bool Update(string Comments, long vacationId, int lengthVacation)
        {
            try
            {
                string query = "UPDATE Vacations SET Comments = '" + Comments + "' ,DaysRequest = " + lengthVacation + " WHERE VacationId = " + vacationId;
                oDatabase.ExecuteNonQuery(query, "Update Register Vacations");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Update Record Vacations", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Update");
                return false;
            }
        }
        public bool Delete(long VacationId)
        {
            try
            {
                return true;
            }
            catch(Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Record Vacations", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Delete");
                return false;
            }
        }
        public DtoVacations GetVacationRequestByVacationId(long VacationId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                DtoVacations dtovacation = new DtoVacations();
                mapping.Load<DtoVacations>("Vacations", new DtoVacations(), "VacationId = " + VacationId);
                dtovacation = (DtoVacations)mapping.dtoList.First().Dto;
                return dtovacation;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load One Record Vacation", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetVacationRequestByVacationId");
                throw ex;
            }
        }
        public List<DtoVacations>GetVacationRequestsbyUser(long WorkerFileId) 
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoVacations> dtovacationsList = new List<DtoVacations>();
                mapping.Load<DtoVacations>("SELECT * FROM Vacations WHERE WorkerFileId = " + WorkerFileId.ToString(), "Vacations", new DtoVacations());
                dtovacationsList.AddRange(mapping.dtoList.Select(renglon => (DtoVacations)renglon.Dto));
                return dtovacationsList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Records Vacations by User", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetVacationRequestsbyUser");
                throw ex;
            }
        }

        public List<DtoVacations>GetVacationRequestsbyProcess(string ProcessesName)
        {
            try
            {
                List<DtoVacations> requestsList = new List<DtoVacations>();
                return requestsList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Records Vacations", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetVacationRequests");
                throw ex;
            }
        }

        public long GetIdentityVacations()
        {
            try
            {
                DataRow identity;
                string query = "SELECT MAX(VacationId) as LastValue FROM Vacations";
                identity = oDatabase.GetRow(query, "Get Worker ID");
                return long.Parse(identity["LastValue"].ToString());
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Approve Vacations Records", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "ApproveRequestVacation");
                return 0;
            }
        }
        #endregion
        #region Process methods
        public bool ApproveBossVacation(long VacationId)
        {
            try
            {
                string query = "UPDATE Vacations SET BossAuthorization = 1 WHERE VacationId = " + VacationId.ToString();
                oDatabase.ExecuteNonQuery(query, "Approve Boss Register Vacations");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Approve Vacations Records", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "ApproveRequestVacation");
                return false;
            }
        }

        public bool ApproveHrVacation(long VacationId)
        {
            try
            {
                string query = "UPDATE Vacations SET HrAuthorization = 1 WHERE VacationId = " + VacationId.ToString();
                oDatabase.ExecuteNonQuery(query, "Approve HR Register Vacations");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Approve Vacations Records", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "ApproveRequestVacation");
                return false;
            }
        }

        public int TotalAvailableDays(long WorkerFileId)
        {
            try
            {
                DataRow totalAvailableDays;
                string query = "SELECT SUM(DaysAvailable) AS TotalAvailableDays FROM HistoryAnniversary WHERE WorkerFileId =" + WorkerFileId.ToString() + "AND DATEDIFF(MONTH, DateAwarded, GETDATE()) <= 18";
                totalAvailableDays = oDatabase.GetRow(query, "Get Total Available Days");
                return int.Parse(totalAvailableDays["TotalAvailableDays"].ToString());
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Total Available Days to Vacations", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "TotalAvailableDays");
                return -1;
            }
        }

        public int MonthsOldbyUser(long WorkerFileId)
        {
            try
            {
                DataRow monthsOfAntiquity;
                string query = "SELECT DATEDIFF(MONTH, AdmissionDate, GETDATE()) AS MonthsOfAntiquity FROM WorkerFiles WHERE WorkerFileId = " + WorkerFileId.ToString();
                monthsOfAntiquity = oDatabase.GetRow(query, "Get Months of seniority");
                return int.Parse(monthsOfAntiquity["MonthsOfAntiquity"].ToString());
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Obtaining Months of Seniority", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "TotalAvailableDays");
                return -1;
            }
        }

        public bool AnniversaryVacationSharing(long WorkerFileId, int DaysRequested, long VacationId)
        {
            try
            {
                List<DtoHistoryAnniversary> ListAvailableAnniversaries = new List<DtoHistoryAnniversary>();
                ListAvailableAnniversaries = historyAnniversary.GetAnniversarysAvailable(WorkerFileId);
                int DaysRequestedIterable = DaysRequested;
                while (DaysRequestedIterable > 0)
                {
                    foreach (var availableAnniversaries in ListAvailableAnniversaries)
                    {
                        if (DaysRequestedIterable - availableAnniversaries.DaysAvailable >= 0)
                        {
                            var VacationDayUsageObject = new DtoVacationDayUsage
                            {
                                VacationId = VacationId,
                                HistoryAnniversaryId = int.Parse(availableAnniversaries.HistoryAnniversaryId.ToString()),
                                DaysUsed = int.Parse(availableAnniversaries.DaysAvailable.ToString())
                            };

                            if (vacationDayUsage.Save(VacationDayUsageObject))
                            {
                                DaysRequestedIterable = DaysRequestedIterable - availableAnniversaries.DaysAvailable;
                                historyAnniversary.UpdateDaysAvailable(availableAnniversaries.HistoryAnniversaryId, DaysRequested - DaysRequestedIterable);
                            }
                            else { return false; }
                        }
                        else
                        {
                            int RemainingDays = availableAnniversaries.DaysAvailable - DaysRequestedIterable;
                            var VacationDayUsageObject = new DtoVacationDayUsage
                            {
                                VacationId = VacationId,
                                HistoryAnniversaryId = int.Parse(availableAnniversaries.HistoryAnniversaryId.ToString()),
                                DaysUsed = DaysRequestedIterable
                            };
                            if (vacationDayUsage.Save(VacationDayUsageObject))
                            {
                                DaysRequestedIterable = DaysRequestedIterable - DaysRequestedIterable;
                                historyAnniversary.UpdateDaysAvailable(availableAnniversaries.HistoryAnniversaryId, RemainingDays);
                            }
                            else { return false; }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Divide Available Anniversary Days Among The Requested Days", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "AnniversaryVacationSharing");
                return false;
            }
        }

        public bool StatusUpdate(long VacationId, int Status)
        {
            try
            {
                string query = "UPDATE Vacations SET GralStatus = " + Status + " WHERE VacationId = " + VacationId;
                oDatabase.ExecuteNonQuery(query, "Update Status Register Vacations");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Update Status Vacations Records", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "StatusUpdate");
                return false;
            }
        }

        public bool Blocked(long VacationId)
        {
            try
            {
                string query = "UPDATE Vacations SET Blocked = " + 1 + " WHERE VacationId = " + VacationId;
                oDatabase.ExecuteNonQuery(query, "Update Status Register Vacations");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Blocked Vacations Records", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Blocked");
                return false;
            }
        }

        public List<DtoReportVacations> LoadReportVacations(string startDate, string endDate, long workerfileId)
        {
            try
            {
                string query = "";
                if (workerfileId == 0)
                {
                    query = "SELECT V.VacationId, V.DateRequest, Wf.Names, V.DaysRequest, V.Comments, V.GralStatus, DFV.Date FROM Vacations V JOIN DatesForVacations DFV on DFV.VacationId = V.VacationId JOIN WorkerFiles Wf on V.WorkerFileId = Wf.WorkerFileId WHERE DFV.Date BETWEEN '" + startDate +"' AND '" + endDate +"' ORDER BY 1 DESC";
                }
                else
                {
                    query = "SELECT V.VacationId, V.DateRequest, Wf.Names, V.DaysRequest, V.Comments, V.GralStatus, DFV.Date FROM Vacations V JOIN DatesForVacations DFV on DFV.VacationId = V.VacationId JOIN WorkerFiles Wf on V.WorkerFileId = Wf.WorkerFileId WHERE DFV.Date BETWEEN '" + startDate + "' AND '" + endDate + "' AND V.WorkerFileId = " + workerfileId.ToString() + " ORDER BY 1 DESC";
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoReportVacations> dtoListReportVacations = new List<DtoReportVacations>();
                mapping.Load<DtoReportVacations>(query, "Vacations", new DtoReportVacations());
                dtoListReportVacations.AddRange(mapping.dtoList.Select(renglon => (DtoReportVacations)renglon.Dto));
                return dtoListReportVacations;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Report Vacations", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadReportVacations");
                throw ex;
            }
        }

        public bool ReturnsDaysVacation(long VacationId)
        {
            try
            {
                string query = "EXEC CancelVacationRequest @VacationId = " + VacationId.ToString();
                oDatabase.ExecuteNonQuery(query, "Returns Vacations by Cancel o Revert");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Return Days Vacation", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "ReturnsDaysVacation");
                return false;
            }
        }

        public int DaysAvaiblebyUser(long WorkerFileId)
        {
            try
            {
                DataRow daysAvaible;
                string query = "SELECT SUM(DaysAvailable) AS DaysAvaible FROM HistoryAnniversary WHERE WorkerFileId = " + WorkerFileId.ToString();
                daysAvaible = oDatabase.GetRow(query, "Get Days Avaible");
                return int.Parse(daysAvaible["DaysAvaible"].ToString());
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Days Avaible by User", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DaysAvaiblebyUser");
                return -1;
            }
        }

        public List<DtoDaysAvaible>DaysAvaibleGral()
        {
            try
            {
                string query = "SELECT WorkerFileId , SUM(DaysAvailable) AS 'DaysAvaible' FROM HistoryAnniversary WHERE AnniversaryNumber != 0 GROUP BY WorkerFileId ORDER BY 1 ASC";
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoDaysAvaible> dtoListDaysAvaible = new List<DtoDaysAvaible>();
                mapping.Load<DtoDaysAvaible>(query, "DaysAvaible", new DtoDaysAvaible());
                dtoListDaysAvaible.AddRange(mapping.dtoList.Select(renglon => (DtoDaysAvaible)renglon.Dto));
                return dtoListDaysAvaible;
            }
            catch(Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Days Avaible General", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DaysAvaibleGral");
                throw ex;
            }
        }

        public List<DtoStatusGral>GetStatusGral()
        {
            try
            {
                string query = "WITH CTE_LastRecord AS (SELECT VacationId, WorkerFileId, GralStatus, ROW_NUMBER() OVER (PARTITION BY WorkerFileId ORDER BY VacationId DESC) AS RowNum FROM Vacations) SELECT VacationId, WorkerFileId, GralStatus FROM CTE_LastRecord WHERE RowNum = 1;\r\n";
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoStatusGral> dtoListStatusGral = new List<DtoStatusGral>();
                mapping.Load<DtoStatusGral>(query, "StatusGral", new DtoStatusGral());
                dtoListStatusGral.AddRange(mapping.dtoList.Select(renglon => (DtoStatusGral)renglon.Dto));
                return dtoListStatusGral;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Status General", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetStatusGral");
                throw ex;
            }
        }
        #endregion
    }
}
