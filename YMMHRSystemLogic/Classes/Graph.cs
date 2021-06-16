using DBFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class Graph
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kiFirstHalf"></param>
        /// <returns></returns>
        public List<DtoGraphInt> GetMonthlyTransports(string kiFirstHalf)
        {
            try
            {
                int firstHalf = Convert.ToInt32(kiFirstHalf);

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoGraphInt> transports = new List<DtoGraphInt>();

                mapping.Load<DtoGraphInt>("SELECT DATEPART(YEAR,StartDate) Year, DATEPART(MONTH,StartDate) as X, SUM(DATEDIFF(day, StartDate, DATEADD(day, 1, FinishDate))) AS Y FROM Transports  WHERE StartDate BETWEEN DATEFROMPARTS(" + firstHalf + ", 4, 1) AND DATEFROMPARTS(" + (firstHalf + 1) + ", 3, 31) GROUP BY DATEPART(YEAR, StartDate), DATEPART(MONTH, StartDate) ORDER BY YEAR(StartDate), MONTH(StartDate)", "Transports", new DtoGraphInt());

                transports.AddRange(mapping.dtoList.Select(renglon => (DtoGraphInt)renglon.Dto));

                return transports;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Monthly Transports", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetMonthlyTransports");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kiFirstHalf"></param>
        /// <returns></returns>
        public List<DtoGraphInt> GetMonthlyExtraTransports(string kiFirstHalf)
        {
            try
            {
                int firstHalf = Convert.ToInt32(kiFirstHalf);

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoGraphInt> transports = new List<DtoGraphInt>();

                mapping.Load<DtoGraphInt>("SELECT DATEPART(YEAR, StartDate) Year, DATEPART(MONTH, StartDate) as X, Count(ExtraordinaryTransportId) AS Y FROM ExtraordinaryTransports WHERE StartDate BETWEEN DATEFROMPARTS(" + firstHalf + ", 4, 1) AND DATEFROMPARTS(" + (firstHalf + 1) + ", 3, 31) AND ShiftChange = 0 AND Status = 1 GROUP BY DATEPART(YEAR, StartDate), DATEPART(MONTH, StartDate) ORDER BY YEAR(StartDate), MONTH(StartDate)", "Transports", new DtoGraphInt());

                transports.AddRange(mapping.dtoList.Select(renglon => (DtoGraphInt)renglon.Dto));

                return transports;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Monthly Extra Transports", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetMonthlyExtraTransports");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kiFirstHalf"></param>
        /// <returns></returns>
        public DtoGraph GetTotalTransport(string kiFirstHalf)
        {
            try
            {
                int firstHalf = Convert.ToInt32(kiFirstHalf);

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                DtoGraph transport = new DtoGraph();

                mapping.Load<DtoGraph>("SELECT 'Normal' AS X, SUM(DATEDIFF(day, StartDate, DATEADD(day, 1, FinishDate))) AS Y FROM Transports  WHERE StartDate BETWEEN DATEFROMPARTS(" + firstHalf + ", 4, 1) AND DATEFROMPARTS(" + (firstHalf + 1) + ", 3, 31)", "Transports", new DtoGraph());
                transport = (DtoGraph)mapping.dtoList.FirstOrDefault().Dto;

                return transport;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Total Transports", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetTotalTransports");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kiFirstHalf"></param>
        /// <returns></returns>
        public DtoGraph GetTotalExtraTransport(string kiFirstHalf)
        {
            try
            {
                int firstHalf = Convert.ToInt32(kiFirstHalf);

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                DtoGraph extraTransport = new DtoGraph();

                mapping.Load<DtoGraph>("SELECT 'Extra' as X, Count(ExtraordinaryTransportId) AS Y FROM ExtraordinaryTransports WHERE StartDate BETWEEN DATEFROMPARTS(" + firstHalf + ", 4, 1) AND DATEFROMPARTS(" + (firstHalf + 1) + ", 3, 31) AND ShiftChange = 0 AND Status = 1", "ExtraordinaryTransports", new DtoGraph());
                extraTransport = (DtoGraph)mapping.dtoList.FirstOrDefault().Dto;

                return extraTransport;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Total Extra Transports", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetTotalExtraTransports");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kiFirstHalf"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public List<DtoGraph> GetTotalExtraTransportByProcess(string kiFirstHalf, string month = "0")
        {
            try
            {
                int firstHalf = Convert.ToInt32(kiFirstHalf);

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoGraph> transports = new List<DtoGraph>();
                if (!month.Equals("0"))
                {
                    mapping.Load<DtoGraph>("SELECT Process as X, Count(ExtraordinaryTransportId) AS Y FROM ExtraordinaryTransports WHERE StartDate BETWEEN DATEFROMPARTS(" + firstHalf + ", 4, 1) AND DATEFROMPARTS(" + (firstHalf + 1) + ", 3, 31) AND MONTH(StartDate) = " + month + " AND AND ShiftChange = 0 AND Status = 1 GROUP BY Process", "ExtraTransports", new DtoGraph());
                }
                else
                {
                    mapping.Load<DtoGraph>("SELECT Process as X, Count(ExtraordinaryTransportId) AS Y FROM ExtraordinaryTransports WHERE StartDate BETWEEN DATEFROMPARTS(" + firstHalf + ", 4, 1) AND DATEFROMPARTS(" + (firstHalf + 1) + ", 3, 31) AND ShiftChange = 0 AND Status = 1 GROUP BY Process", "ExtraTransports", new DtoGraph());
                }

                transports.AddRange(mapping.dtoList.Select(renglon => (DtoGraph)renglon.Dto));

                return transports;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Total Extra Transport By Process", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetTotalExtraTransportByProcess");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kiFirstHalf"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public List<DtoGraphStringDecimal> GetCostExtraTransportByProcess(string kiFirstHalf, string month = "0")
        {
            try
            {
                int firstHalf = Convert.ToInt32(kiFirstHalf);

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoGraphStringDecimal> transports = new List<DtoGraphStringDecimal>();
                if (!month.Equals("0"))
                {
                    mapping.Load<DtoGraphStringDecimal>("SELECT Process as X, SUM(Cost) AS Y FROM ExtraordinaryTransports WHERE StartDate BETWEEN DATEFROMPARTS(" + firstHalf + ", 4, 1) AND DATEFROMPARTS(" + (firstHalf + 1) + ", 3, 31) AND MONTH(StartDate) = " + month + " AND ShiftChange = 0 AND Status = 1 GROUP BY Process", "ExtraTransports", new DtoGraphStringDecimal());
                }
                else
                {
                    mapping.Load<DtoGraphStringDecimal>("SELECT Process as X, SUM(Cost) AS Y FROM ExtraordinaryTransports WHERE StartDate BETWEEN DATEFROMPARTS(" + firstHalf + ", 4, 1) AND DATEFROMPARTS(" + (firstHalf + 1) + ", 3, 31) AND ShiftChange = 0 AND Status = 1 GROUP BY Process", "ExtraTransports", new DtoGraphStringDecimal());
                }

                transports.AddRange(mapping.dtoList.Select(renglon => (DtoGraphStringDecimal)renglon.Dto));

                return transports;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Cost Extra Transport By Process", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetCostExtraTransportByProcess");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kiFirstHalf"></param>
        /// <returns></returns>
        public List<DtoGraphDecimal> GetMonthlyExtraTransportCosts(string kiFirstHalf)
        {
            try
            {
                int firstHalf = Convert.ToInt32(kiFirstHalf);

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoGraphDecimal> transports = new List<DtoGraphDecimal>();

                mapping.Load<DtoGraphDecimal>("SELECT DATEPART(YEAR, StartDate) Year, DATEPART(MONTH, StartDate) as X, SUM(COST) AS Y FROM ExtraordinaryTransports WHERE StartDate BETWEEN DATEFROMPARTS(" + firstHalf + ", 4, 1) AND DATEFROMPARTS(" + (firstHalf + 1) + ", 3, 31) AND ShiftChange = 0 GROUP BY DATEPART(YEAR, StartDate), DATEPART(MONTH, StartDate) ORDER BY YEAR(StartDate), MONTH(StartDate)", "Transports", new DtoGraphDecimal());

                transports.AddRange(mapping.dtoList.Select(renglon => (DtoGraphDecimal)renglon.Dto));

                return transports;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Monthly Extra Transport Cost", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetMonthlyExtraTransportCosts");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kiFirstHalf"></param>
        /// <returns></returns>
        public List<DtoGraphDecimal> GetMonthlyTransportCosts(string kiFirstHalf)
        {
            try
            {
                int firstHalf = Convert.ToInt32(kiFirstHalf);

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoGraphDecimal> transports = new List<DtoGraphDecimal>();

                mapping.Load<DtoGraphDecimal>("SELECT DATEPART(YEAR, StartDate) Year, DATEPART(MONTH, StartDate) as X, SUM(COST) AS Y FROM Transports WHERE StartDate BETWEEN DATEFROMPARTS(" + firstHalf + ", 4, 1) AND DATEFROMPARTS(" + (firstHalf + 1) + ", 3, 31) GROUP BY DATEPART(YEAR, StartDate), DATEPART(MONTH, StartDate) ORDER BY YEAR(StartDate), MONTH(StartDate)", "Transports", new DtoGraphDecimal());

                transports.AddRange(mapping.dtoList.Select(renglon => (DtoGraphDecimal)renglon.Dto));

                return transports;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Monthly Transport Cost", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetMonthlyTransportCosts");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kiFirstHalf"></param>
        /// <returns></returns>
        public DtoGraphStringDecimal GetTotalTransportCost(string kiFirstHalf)
        {
            try
            {
                int firstHalf = Convert.ToInt32(kiFirstHalf);

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                DtoGraphStringDecimal transports = new DtoGraphStringDecimal();

                mapping.Load<DtoGraphStringDecimal>("SELECT 'Normal' as X, SUM(COST) AS Y FROM Transports WHERE StartDate BETWEEN DATEFROMPARTS(" + firstHalf + ", 4, 1) AND DATEFROMPARTS(" + (firstHalf + 1) + ", 3, 31)", "Transports", new DtoGraphStringDecimal());
                transports = (DtoGraphStringDecimal)mapping.dtoList.FirstOrDefault().Dto;

                return transports;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Total Transport Cost", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetTotalTransportCost");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kiFirstHalf"></param>
        /// <returns></returns>
        public DtoGraphStringDecimal GetTotalExtraTransportCost(string kiFirstHalf)
        {
            try
            {
                int firstHalf = Convert.ToInt32(kiFirstHalf);

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                DtoGraphStringDecimal transports = new DtoGraphStringDecimal();

                mapping.Load<DtoGraphStringDecimal>("SELECT 'Extra' as X, SUM(COST) AS Y FROM ExtraordinaryTransports WHERE StartDate BETWEEN DATEFROMPARTS(" + firstHalf + ", 4, 1) AND DATEFROMPARTS(" + (firstHalf + 1) + ", 3, 31) AND ShiftChange = 0 AND Status = 1", "ExtraTransports", new DtoGraphStringDecimal());
                transports = (DtoGraphStringDecimal)mapping.dtoList.FirstOrDefault().Dto;

                return transports;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Total Extra Transport Cost", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetTotalExtraTransportCost");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<DtoGraph> GetDinerServiceByType(string startDate, string endDate)
        {
            DBFrameworkMapping mapping = new DBFrameworkMapping();
            List<DtoGraph> diner = new List<DtoGraph>();

            mapping.Load<DtoGraph>("SELECT Type as X, Count(DinerId) AS Y FROM Diner WHERE StartDate BETWEEN '" + startDate + "' AND '" + endDate + "' GROUP BY Type UNION SELECT Type as X, Count(ExtraordinaryDinerId) AS Y FROM ExtraordinaryDiner WHERE Date BETWEEN '" + startDate + "' AND '" + endDate + "' AND Status = 1 GROUP BY Type", "Diner", new DtoGraph());
            diner.AddRange(mapping.dtoList.Select(renglon => (DtoGraph)renglon.Dto));

            return diner;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<DtoGraphStringDecimal> GetDinerServiceCostByType(string startDate, string endDate)
        {
            DBFrameworkMapping mapping = new DBFrameworkMapping();
            List<DtoGraphStringDecimal> diner = new List<DtoGraphStringDecimal>();

            mapping.Load<DtoGraphStringDecimal>("SELECT Type as X, SUM(Total) AS Y FROM Diner WHERE StartDate BETWEEN '" + startDate + "' AND '" + endDate + "' GROUP BY Type UNION SELECT Type as X, SUM(TypeCost + LadingCost) AS Y FROM ExtraordinaryDiner WHERE Date BETWEEN '" + startDate + "' AND '" + endDate + "' AND Status = 1 GROUP BY Type", "Diner", new DtoGraphStringDecimal());
            diner.AddRange(mapping.dtoList.Select(renglon => (DtoGraphStringDecimal)renglon.Dto));

            return diner;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<DtoGraphStringDecimal> GetPaymentComparative(string startDate, string endDate)
        {
            DBFrameworkMapping mapping = new DBFrameworkMapping();
            List<DtoGraphStringDecimal> diner = new List<DtoGraphStringDecimal>();

            mapping.Load<DtoGraphStringDecimal>("SELECT Type as X, SUM(Total) AS Y FROM Diner WHERE StartDate BETWEEN '" + startDate + "' AND '" + endDate + "' AND (TYPE = 'YMM' OR TYPE = 'Associate') GROUP BY Type UNION SELECT 'YMM' as X, SUM(TypeCost + LadingCost) AS Y FROM ExtraordinaryDiner WHERE Date BETWEEN '" + startDate + "' AND '" + endDate + "' AND Status = 1 UNION SELECT 'YMM' AS X, SUM(Total) AS Y FROM Diner WHERE StartDate BETWEEN '" + startDate + "' AND '" + endDate + "' AND Type = 'Saturday'", "Diner", new DtoGraphStringDecimal());
            diner.AddRange(mapping.dtoList.Select(renglon => (DtoGraphStringDecimal)renglon.Dto));

            return diner;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<DtoGraphXYZ> GetExtraDinerServicesByProcess(string startDate, string endDate)
        {
            DBFrameworkMapping mapping = new DBFrameworkMapping();
            List<DtoGraphXYZ> diner = new List<DtoGraphXYZ>();

            mapping.Load<DtoGraphXYZ>("SELECT Process AS X, Type AS Y, Count(ExtraordinaryDinerId) AS Z FROM ExtraordinaryDiner WHERE Date BETWEEN '" + startDate + "' AND '" + endDate + "' AND Status = 1 GROUP BY Process, Type", "Diner", new DtoGraphXYZ());
            diner.AddRange(mapping.dtoList.Select(renglon => (DtoGraphXYZ)renglon.Dto));

            return diner;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kiFirstHalf"></param>
        /// <returns></returns>
        public List<DtoGraphDecimal> GetMonthlySindicateAdministrativeSupport(string kiFirstHalf)
        {
            try
            {
                int firstHalf = Convert.ToInt32(kiFirstHalf);

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoGraphDecimal> transports = new List<DtoGraphDecimal>();

                mapping.Load<DtoGraphDecimal>("SELECT DATEPART(YEAR, Date) Year, DATEPART(MONTH, Date) as X, SUM(AdministrativeSupport) AS Y FROM Sindicate WHERE Date BETWEEN DATEFROMPARTS(" + firstHalf + ", 4, 1) AND DATEFROMPARTS(" + (firstHalf + 1) + ", 3, 31) GROUP BY DATEPART(YEAR, Date), DATEPART(MONTH, Date) ORDER BY YEAR(Date), MONTH(Date)", "Sindicate", new DtoGraphDecimal());

                transports.AddRange(mapping.dtoList.Select(renglon => (DtoGraphDecimal)renglon.Dto));

                return transports;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Monthly Sindicate Administrative Support", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetMonthlySindicateAdministrativeSupport");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kiFirstHalf"></param>
        /// <returns></returns>
        public List<DtoGraphDecimal> GetMonthlySindicateUnionFee(string kiFirstHalf)
        {
            try
            {
                int firstHalf = Convert.ToInt32(kiFirstHalf);

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoGraphDecimal> transports = new List<DtoGraphDecimal>();

                mapping.Load<DtoGraphDecimal>("SELECT DATEPART(YEAR, Date) Year, DATEPART(MONTH, Date) as X, SUM(UnionFee) AS Y FROM Sindicate WHERE Date BETWEEN DATEFROMPARTS(" + firstHalf + ", 4, 1) AND DATEFROMPARTS(" + (firstHalf + 1) + ", 3, 31) GROUP BY DATEPART(YEAR, Date), DATEPART(MONTH, Date) ORDER BY YEAR(Date), MONTH(Date)", "Sindicate", new DtoGraphDecimal());

                transports.AddRange(mapping.dtoList.Select(renglon => (DtoGraphDecimal)renglon.Dto));

                return transports;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Monthly Sindicate Union Fee", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetMonthlySindicateUnionFee");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kiFirstHalf"></param>
        /// <returns></returns>
        public List<DtoGraphStringDecimal> GetTotalSindicateAmounts(string kiFirstHalf)
        {
            try
            {
                int firstHalf = Convert.ToInt32(kiFirstHalf);

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoGraphStringDecimal> sindicateAmounts = new List<DtoGraphStringDecimal>();

                mapping.Load<DtoGraphStringDecimal>("SELECT 'Administative Support' as X, SUM(AdministrativeSupport) AS Y FROM Sindicate WHERE Date BETWEEN DATEFROMPARTS(" + firstHalf + ", 4, 1) AND DATEFROMPARTS(" + (firstHalf + 1) + ", 3, 31) UNION SELECT 'Union Fee' as X, SUM(UnionFee) AS Y FROM Sindicate WHERE Date BETWEEN DATEFROMPARTS(" + firstHalf + ", 4, 1) AND DATEFROMPARTS(" + (firstHalf + 1) + ", 3, 31)", "Sindicate", new DtoGraphStringDecimal());
                sindicateAmounts.AddRange(mapping.dtoList.Select(renglon => (DtoGraphStringDecimal)renglon.Dto));

                return sindicateAmounts;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Total Sindicate Amounts", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetTotalSindicateAmounts");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DtoGraph> GetYearsOfService()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoGraph> yearsOfService = new List<DtoGraph>();
                List<string> yearList = new List<string>();

                string sqlString = "SELECT DATEDIFF(month, AdmissionDate, GETDATE())/12 AS DateDiff FROM WorkerFiles WHERE Status = 1";

                var dataTable = oDatabase.GetTable(sqlString, "Get Years of Service");

                foreach (DataRow row in dataTable.Rows)
                {
                    yearList.Add(row[0].ToString());
                }

                yearsOfService = yearList.GroupBy(x => x)
                 .Select(g => new DtoGraph { X = g.Key, Y = g.Count() }).ToList();


                return yearsOfService;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Years Of Service", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetYearsOfService");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DtoGraph> GetWorkerAges()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoGraph> workerAges = new List<DtoGraph>();
                List<string> yearList = new List<string>();

                string sqlString = "SELECT DATEDIFF(month, DoB, GETDATE())/12 AS DateDiff FROM WorkerFiles WHERE Status = 1";

                var dataTable = oDatabase.GetTable(sqlString, "Get Worker Ages");

                foreach (DataRow row in dataTable.Rows)
                {
                    yearList.Add(row[0].ToString());
                }

                workerAges = yearList.GroupBy(x => x)
                 .Select(g => new DtoGraph { X = g.Key, Y = g.Count() }).ToList();


                return workerAges;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Worker Ages", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetWorkerAges");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kiFirstHalf"></param>
        /// <returns></returns>
        public List<DtoGraph> GetMonthlyAdmissions(string kiFirstHalf)
        {
            try
            {
                int firstHalf = Convert.ToInt32(kiFirstHalf);

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoGraphInt> admissions = new List<DtoGraphInt>();

                mapping.Load<DtoGraphInt>("SELECT DATEPART(YEAR, AdmissionDate) Year, DATEPART(MONTH, AdmissionDate) as X, COUNT(WorkerFileId) AS Y FROM WorkerFiles WHERE AdmissionDate BETWEEN DATEFROMPARTS(" + firstHalf + ", 4, 1) AND DATEFROMPARTS(" + (firstHalf + 1) + ", 3, 31) GROUP BY DATEPART(YEAR, AdmissionDate), DATEPART(MONTH, AdmissionDate) ORDER BY YEAR(AdmissionDate), MONTH(AdmissionDate)", "Sindicate", new DtoGraphInt());

                admissions.AddRange(mapping.dtoList.Select(renglon => (DtoGraphInt)renglon.Dto));

                List<DtoGraph> admissionFinal = new List<DtoGraph>(){
                    new DtoGraph() { X = "April", Y = 0 },
                    new DtoGraph() { X = "May", Y = 0 },
                    new DtoGraph() { X = "June", Y = 0 },
                    new DtoGraph() { X = "July", Y = 0 },
                    new DtoGraph() { X = "August", Y = 0 },
                    new DtoGraph() { X = "September", Y = 0 },
                    new DtoGraph() { X = "October", Y = 0 },
                    new DtoGraph() { X = "November", Y = 0 },
                    new DtoGraph() { X = "December", Y = 0 },
                    new DtoGraph() { X = "January", Y = 0 },
                    new DtoGraph() { X = "February", Y = 0 },
                    new DtoGraph() { X = "March", Y = 0 },
                };

                foreach (var item in admissionFinal)
                {
                    var itemFound = admissions.FirstOrDefault(x => x.X == DateTime.ParseExact(item.X, "MMMM", CultureInfo.InvariantCulture).Month);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Y;
                    }
                }

                return admissionFinal;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Monthly Admissions", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetMonthlyAdmissions");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kiFirstHalf"></param>
        /// <returns></returns>
        public List<DtoGraph> GetMonthlyDismissals(string kiFirstHalf)
        {
            try
            {
                int firstHalf = Convert.ToInt32(kiFirstHalf);

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoGraphInt> dismissals = new List<DtoGraphInt>();

                mapping.Load<DtoGraphInt>("SELECT DATEPART(YEAR, Date) Year, DATEPART(MONTH, Date) as X, COUNT(DismissalId) AS Y FROM Dismissals WHERE Date BETWEEN DATEFROMPARTS(" + firstHalf + ", 4, 1) AND DATEFROMPARTS(" + (firstHalf + 1) + ", 3, 31) GROUP BY DATEPART(YEAR, Date), DATEPART(MONTH, Date) ORDER BY YEAR(Date), MONTH(Date)", "Dismissals", new DtoGraphInt());

                dismissals.AddRange(mapping.dtoList.Select(renglon => (DtoGraphInt)renglon.Dto));

                List<DtoGraph> dismissalFinal = new List<DtoGraph>(){
                    new DtoGraph() { X = "April", Y = 0 },
                    new DtoGraph() { X = "May", Y = 0 },
                    new DtoGraph() { X = "June", Y = 0 },
                    new DtoGraph() { X = "July", Y = 0 },
                    new DtoGraph() { X = "August", Y = 0 },
                    new DtoGraph() { X = "September", Y = 0 },
                    new DtoGraph() { X = "October", Y = 0 },
                    new DtoGraph() { X = "November", Y = 0 },
                    new DtoGraph() { X = "December", Y = 0 },
                    new DtoGraph() { X = "January", Y = 0 },
                    new DtoGraph() { X = "February", Y = 0 },
                    new DtoGraph() { X = "March", Y = 0 },
                };

                foreach (var item in dismissalFinal)
                {
                    var itemFound = dismissals.FirstOrDefault(x => x.X == DateTime.ParseExact(item.X, "MMMM", CultureInfo.InvariantCulture).Month);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Y;
                    }
                }

                return dismissalFinal;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Monthly Dismissals", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetMonthlyDismissals");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kiFirstHalf"></param>
        /// <returns></returns>
        public List<DtoGraph> GetTotalAdmissionDismissal(string kiFirstHalf)
        {
            try
            {
                int firstHalf = Convert.ToInt32(kiFirstHalf);

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoGraph> admissionDismissal = new List<DtoGraph>();

                mapping.Load<DtoGraph>("SELECT 'Admission' as X, COUNT(WorkerFileId) AS Y FROM WorkerFiles WHERE AdmissionDate BETWEEN DATEFROMPARTS(" + firstHalf + ", 4, 1) AND DATEFROMPARTS(" + (firstHalf + 1) + ", 3, 31) UNION SELECT 'Dismissal' as X, COUNT(DismissalId) AS Y FROM Dismissals WHERE Date BETWEEN DATEFROMPARTS(" + firstHalf + ", 4, 1) AND DATEFROMPARTS(" + (firstHalf + 1) + ", 3, 31)", "WorkerFiles", new DtoGraph());
                admissionDismissal.AddRange(mapping.dtoList.Select(renglon => (DtoGraph)renglon.Dto));

                return admissionDismissal;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Total Admissions and Dismissals", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetTotalAdmissionDismissal");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kiFirstHalf"></param>
        /// <returns></returns>
        public List<DtoGraph> GetMonthlyDismissDismissals(string kiFirstHalf)
        {
            try
            {
                int firstHalf = Convert.ToInt32(kiFirstHalf);

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoGraphInt> dismissals = new List<DtoGraphInt>();

                mapping.Load<DtoGraphInt>("SELECT DATEPART(YEAR, Date) Year, DATEPART(MONTH, Date) as X, COUNT(DismissalId) AS Y FROM Dismissals WHERE Type = 'Dismissal' AND (Date BETWEEN DATEFROMPARTS(" + firstHalf + ", 4, 1) AND DATEFROMPARTS(" + (firstHalf + 1) + ", 3, 31)) GROUP BY DATEPART(YEAR, Date), DATEPART(MONTH, Date) ORDER BY YEAR(Date), MONTH(Date)", "Dismissals", new DtoGraphInt());

                dismissals.AddRange(mapping.dtoList.Select(renglon => (DtoGraphInt)renglon.Dto));

                List<DtoGraph> dismissalFinal = new List<DtoGraph>(){
                    new DtoGraph() { X = "April", Y = 0 },
                    new DtoGraph() { X = "May", Y = 0 },
                    new DtoGraph() { X = "June", Y = 0 },
                    new DtoGraph() { X = "July", Y = 0 },
                    new DtoGraph() { X = "August", Y = 0 },
                    new DtoGraph() { X = "September", Y = 0 },
                    new DtoGraph() { X = "October", Y = 0 },
                    new DtoGraph() { X = "November", Y = 0 },
                    new DtoGraph() { X = "December", Y = 0 },
                    new DtoGraph() { X = "January", Y = 0 },
                    new DtoGraph() { X = "February", Y = 0 },
                    new DtoGraph() { X = "March", Y = 0 },
                };

                foreach (var item in dismissalFinal)
                {
                    var itemFound = dismissals.FirstOrDefault(x => x.X == DateTime.ParseExact(item.X, "MMMM", CultureInfo.InvariantCulture).Month);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Y;
                    }
                }

                return dismissalFinal;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Monthly Dismiss Dismissals", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetMonthlyDismissDismissals");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kiFirstHalf"></param>
        /// <returns></returns>
        public List<DtoGraph> GetMonthlyResignationDismissals(string kiFirstHalf)
        {
            try
            {
                int firstHalf = Convert.ToInt32(kiFirstHalf);

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoGraphInt> dismissals = new List<DtoGraphInt>();

                mapping.Load<DtoGraphInt>("SELECT DATEPART(YEAR, Date) Year, DATEPART(MONTH, Date) as X, COUNT(DismissalId) AS Y FROM Dismissals WHERE Type = 'Resignation' AND (Date BETWEEN DATEFROMPARTS(" + firstHalf + ", 4, 1) AND DATEFROMPARTS(" + (firstHalf + 1) + ", 3, 31)) GROUP BY DATEPART(YEAR, Date), DATEPART(MONTH, Date) ORDER BY YEAR(Date), MONTH(Date)", "Dismissals", new DtoGraphInt());

                dismissals.AddRange(mapping.dtoList.Select(renglon => (DtoGraphInt)renglon.Dto));

                List<DtoGraph> dismissalFinal = new List<DtoGraph>(){
                    new DtoGraph() { X = "April", Y = 0 },
                    new DtoGraph() { X = "May", Y = 0 },
                    new DtoGraph() { X = "June", Y = 0 },
                    new DtoGraph() { X = "July", Y = 0 },
                    new DtoGraph() { X = "August", Y = 0 },
                    new DtoGraph() { X = "September", Y = 0 },
                    new DtoGraph() { X = "October", Y = 0 },
                    new DtoGraph() { X = "November", Y = 0 },
                    new DtoGraph() { X = "December", Y = 0 },
                    new DtoGraph() { X = "January", Y = 0 },
                    new DtoGraph() { X = "February", Y = 0 },
                    new DtoGraph() { X = "March", Y = 0 },
                };

                foreach (var item in dismissalFinal)
                {
                    var itemFound = dismissals.FirstOrDefault(x => x.X == DateTime.ParseExact(item.X, "MMMM", CultureInfo.InvariantCulture).Month);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Y;
                    }
                }

                return dismissalFinal;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Monthly Resignation Dismissals", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetMonthlyResignationDismissals");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kiFirstHalf"></param>
        /// <returns></returns>
        public List<DtoGraph> GetMonthlyEndOfContractDismissals(string kiFirstHalf)
        {
            try
            {
                int firstHalf = Convert.ToInt32(kiFirstHalf);

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoGraphInt> dismissals = new List<DtoGraphInt>();

                mapping.Load<DtoGraphInt>("SELECT DATEPART(YEAR, Date) Year, DATEPART(MONTH, Date) as X, COUNT(DismissalId) AS Y FROM Dismissals WHERE Type = 'End of Contract' AND (Date BETWEEN DATEFROMPARTS(" + firstHalf + ", 4, 1) AND DATEFROMPARTS(" + (firstHalf + 1) + ", 3, 31)) GROUP BY DATEPART(YEAR, Date), DATEPART(MONTH, Date) ORDER BY YEAR(Date), MONTH(Date)", "Dismissals", new DtoGraphInt());

                dismissals.AddRange(mapping.dtoList.Select(renglon => (DtoGraphInt)renglon.Dto));

                List<DtoGraph> dismissalFinal = new List<DtoGraph>(){
                    new DtoGraph() { X = "April", Y = 0 },
                    new DtoGraph() { X = "May", Y = 0 },
                    new DtoGraph() { X = "June", Y = 0 },
                    new DtoGraph() { X = "July", Y = 0 },
                    new DtoGraph() { X = "August", Y = 0 },
                    new DtoGraph() { X = "September", Y = 0 },
                    new DtoGraph() { X = "October", Y = 0 },
                    new DtoGraph() { X = "November", Y = 0 },
                    new DtoGraph() { X = "December", Y = 0 },
                    new DtoGraph() { X = "January", Y = 0 },
                    new DtoGraph() { X = "February", Y = 0 },
                    new DtoGraph() { X = "March", Y = 0 },
                };

                foreach (var item in dismissalFinal)
                {
                    var itemFound = dismissals.FirstOrDefault(x => x.X == DateTime.ParseExact(item.X, "MMMM", CultureInfo.InvariantCulture).Month);
                    if (itemFound != null)
                    {
                        item.Y = itemFound.Y;
                    }
                }

                return dismissalFinal;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Monthly End of Contract Dismissals", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetMonthlyEndOfContractDismissals");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kiFirstHalf"></param>
        /// <returns></returns>
        public List<DtoGraph> GetTotalDismissalsByType(string kiFirstHalf)
        {
            try
            {
                int firstHalf = Convert.ToInt32(kiFirstHalf);

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoGraph> dismissals = new List<DtoGraph>();

                mapping.Load<DtoGraph>("SELECT Type AS X, COUNT(DismissalId) AS Y FROM Dismissals WHERE Date BETWEEN DATEFROMPARTS(" + firstHalf + ", 4, 1) AND DATEFROMPARTS(" + (firstHalf + 1) + ", 3, 31) GROUP BY Type", "Dismissals", new DtoGraph());

                dismissals.AddRange(mapping.dtoList.Select(renglon => (DtoGraph)renglon.Dto));

                return dismissals;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Total Dismissals By Type", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetTotalDismissalsByType");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kiFirstHalf"></param>
        /// <returns></returns>
        public List<DtoGraphStringDecimal> GetTurnover(string kiFirstHalf)
        {
            try
            {
                int firstHalf = Convert.ToInt32(kiFirstHalf);
                WorkerFile workerFile = new WorkerFile();

                List<DtoTurnOver> openingClosingWorkers = new List<DtoTurnOver>();
                List<DtoWorkerFile> workerList = workerFile.LoadMultiple();
                List<DtoGraphStringDecimal> turnOver = new List<DtoGraphStringDecimal>(){
                    new DtoGraphStringDecimal() { X = "April", Y = 0.0m },
                    new DtoGraphStringDecimal() { X = "May", Y = 0.0m },
                    new DtoGraphStringDecimal() { X = "June", Y = 0.0m },
                    new DtoGraphStringDecimal() { X = "July", Y = 0.0m },
                    new DtoGraphStringDecimal() { X = "August", Y = 0.0m },
                    new DtoGraphStringDecimal() { X = "September", Y = 0.0m },
                    new DtoGraphStringDecimal() { X = "October", Y = 0.0m },
                    new DtoGraphStringDecimal() { X = "November", Y = 0.0m },
                    new DtoGraphStringDecimal() { X = "December", Y = 0.0m },
                    new DtoGraphStringDecimal() { X = "January", Y = 0.0m },
                    new DtoGraphStringDecimal() { X = "February", Y = 0.0m },
                    new DtoGraphStringDecimal() { X = "March", Y = 0.0m },
                };

                List<int> turnOverInt = new List<int>() { 4, 5, 6, 7, 8, 9, 10, 11, 12, 1, 2, 3 };

                DateTime firstDayOfMonth, lastDayOfMonth;
                int leavers, openingStaff, closingStaff = 0;
                decimal average = 0.0m;

                foreach (var item in turnOverInt)
                {
                    if (item < 4)
                    {
                        firstDayOfMonth = new DateTime((firstHalf + 1), item, 1);
                        lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                    }
                    else
                    {
                        firstDayOfMonth = new DateTime(firstHalf, item, 1);
                        lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                    }

                    openingStaff = workerList.Where(a => a.AdmissionDate < firstDayOfMonth && (a.DismissalDate?.ToString("yyyyMMdd") == "19000101" ? DateTime.Now.Date : a.DismissalDate) >= firstDayOfMonth).Count();
                    closingStaff = workerList.Where(a => a.AdmissionDate <= lastDayOfMonth && (a.DismissalDate?.ToString("yyyyMMdd") == "19000101" ? DateTime.Now.Date : a.DismissalDate) > lastDayOfMonth).Count();
                    leavers = workerList.Where(a => (a.DismissalDate?.ToString("yyyyMMdd") == "19000101" ? DateTime.Now.Date : a.DismissalDate) >= firstDayOfMonth && (a.DismissalDate?.ToString("yyyyMMdd") == "19000101" ? DateTime.Now.Date : a.DismissalDate) <= lastDayOfMonth).Count();
                    openingClosingWorkers.Add(new DtoTurnOver() { Month = CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(item)), OpeningStaff = openingStaff, ClosingStaff = closingStaff, Leavers = leavers });
                }

                foreach (var item in openingClosingWorkers)
                {
                    var itemFound = turnOver.FirstOrDefault(x => x.X == item.Month);
                    if (itemFound != null)
                    {
                        if (item.ClosingStaff != 0)
                        {
                            average = (item.OpeningStaff + item.ClosingStaff) / 2.00m;
                            if (item.Leavers != 0)
                            {
                                itemFound.Y = Math.Round(((item.Leavers / average) * 100.00m), 2);
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            itemFound.Y = 0.00m;
                        }

                    }
                }


                return turnOver;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Turnover", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetTurnover");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kiFirstHalf"></param>
        /// <returns></returns>
        public List<DtoGraphStringDecimal> GetAverageTurnover(string kiFirstHalf)
        {
            try
            {

                List<DtoGraphStringDecimal> turnOver = GetTurnover(kiFirstHalf);

                decimal average = turnOver.Sum(x => x.Y);
                List<DtoGraphStringDecimal> averageTurnOver = new List<DtoGraphStringDecimal>();
                averageTurnOver.Add(new DtoGraphStringDecimal { X = "Average", Y = Math.Round(average / 12.00m, 2) });

                return averageTurnOver;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Average Turnover", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetAverageTurnover");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kiFirstHalf"></param>
        /// <returns></returns>
        public List<DtoHeadCount> GetHeadCount(string kiFirstHalf)
        {
            try
            {
                int firstHalf = Convert.ToInt32(kiFirstHalf);
                WorkerFile workerFile = new WorkerFile();

                List<DtoHeadCount> closingWorkers = new List<DtoHeadCount>();
                List<DtoWorkerFile> workerList = workerFile.LoadMultiple();
                List<DtoHeadCount> headCount = new List<DtoHeadCount>(){
                    new DtoHeadCount() { Month = "April", Men = 0, Women = 0, Total = 0 },
                    new DtoHeadCount() { Month = "May", Men = 0, Women = 0, Total = 0 },
                    new DtoHeadCount() { Month = "June", Men = 0, Women = 0, Total = 0 },
                    new DtoHeadCount() { Month = "July", Men = 0, Women = 0, Total = 0 },
                    new DtoHeadCount() { Month = "August", Men = 0, Women = 0, Total = 0 },
                    new DtoHeadCount() { Month = "September", Men = 0, Women = 0, Total = 0 },
                    new DtoHeadCount() { Month = "October", Men = 0, Women = 0, Total = 0 },
                    new DtoHeadCount() { Month = "November", Men = 0, Women = 0, Total = 0 },
                    new DtoHeadCount() { Month = "December", Men = 0, Women = 0, Total = 0 },
                    new DtoHeadCount() { Month = "January", Men = 0, Women = 0, Total = 0 },
                    new DtoHeadCount() { Month = "February", Men = 0, Women = 0, Total = 0 },
                    new DtoHeadCount() { Month = "March", Men = 0, Women = 0, Total = 0 },
                };

                List<int> calendarMonth = new List<int>() { 4, 5, 6, 7, 8, 9, 10, 11, 12, 1, 2, 3 };

                DateTime firstDayOfMonth, lastDayOfMonth;
                int men, women, total;

                foreach (var item in calendarMonth)
                {
                    if (item < 4)
                    {
                        firstDayOfMonth = new DateTime((firstHalf + 1), item, 1);
                        lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                    }
                    else
                    {
                        firstDayOfMonth = new DateTime(firstHalf, item, 1);
                        lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                    }
                    men = workerList.Where(a => a.AdmissionDate <= lastDayOfMonth && (a.DismissalDate?.ToString("yyyyMMdd") == "19000101" ? DateTime.Now.Date : a.DismissalDate) >= lastDayOfMonth && a.Gender == true).Count();
                    women = workerList.Where(a => a.AdmissionDate <= lastDayOfMonth && (a.DismissalDate?.ToString("yyyyMMdd") == "19000101" ? DateTime.Now.Date : a.DismissalDate) >= lastDayOfMonth && a.Gender == false).Count();
                    total = men + women;
                    closingWorkers.Add(new DtoHeadCount() { Month = CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(item)), Men = men, Women = women, Total = total });
                }

                foreach (var item in closingWorkers)
                {
                    var itemFound = headCount.FirstOrDefault(x => x.Month == item.Month);
                    if (itemFound != null)
                    {
                        itemFound.Men = item.Men;
                        itemFound.Women = item.Women;
                        itemFound.Total = item.Total;
                    }
                }

                return headCount;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Monthly End of Contract Dismissals", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetMonthlyEndOfContractDismissals");
                throw ex;
            }


        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DtoGraph> GetTotalHeadCount()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoGraph> totalHeadCount = new List<DtoGraph>();

                mapping.Load<DtoGraph>("SELECT CASE WHEN Gender = 1 THEN 'Men' ELSE 'Women' END as X, COUNT(WorkerFileId) AS Y FROM WorkerFiles WHERE Status = 1 GROUP BY Gender", "WorkerFiles", new DtoGraph());
                totalHeadCount.AddRange(mapping.dtoList.Select(renglon => (DtoGraph)renglon.Dto));

                return totalHeadCount;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Total Head Count", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetTotalHeadCount");
                throw ex;
            }
        }

    }
}

