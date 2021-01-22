using DBFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class Transport
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        #region Standard Methods
        /// <summary>
        /// Loads the Transport DTO
        /// </summary>
        /// <param name="transportId"></param>
        /// <returns>DtoTransport Loaded</returns>
        public DtoTransport Load(long transportId)
        {
            DtoTransport Transport = new DtoTransport();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (transportId != 0)
                {
                    mapping.Load<DtoTransport>("Transports", new DtoTransport(), "TransportId=" + transportId);
                    Transport = (DtoTransport)mapping.dtoList.FirstOrDefault().Dto;
                }

                return Transport;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Transport", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Register Transport 
        /// </summary>
        /// <param name="transport"></param>
        /// <returns>Transport registered</returns>
        public void Save(DtoTransport transport)
        {
            try
            {
                User user = new User();

                if (transport.TransportId == 0)
                {
                    transport.UserCreated = user.GetUserName(SQLTools.userId.ToString());
                    transport.Date = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = transport, TableName = "Transports" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Transport", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Transport with fields.
        /// </summary>
        /// <returns>Load Transport Dto</returns>
        public List<DtoTransport> LoadMultiple()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoTransport> shiftList = new List<DtoTransport>();

                mapping.Load<DtoTransport>("SELECT TransportId, Route, Shift, Date, StartDate, FinishDate, Cost, UserCreated, Type FROM Transports ORDER BY TransportId", "Transports", new DtoTransport());
                shiftList.AddRange(mapping.dtoList.Select(renglon => (DtoTransport)renglon.Dto));

                return shiftList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Transports", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }

        /// <summary>
        /// Loads the Extra Transport DTO
        /// </summary>
        /// <param name="transportId"></param>
        /// <returns>DtoTransport Loaded</returns>
        public DtoExtraordinaryTransport LoadExtra(long transportId)
        {
            DtoExtraordinaryTransport extraTransport = new DtoExtraordinaryTransport();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (transportId != 0)
                {
                    mapping.Load<DtoExtraordinaryTransport>("ExtraordinaryTransports", new DtoExtraordinaryTransport(), "ExtraordinaryTransportId=" + transportId);
                    extraTransport = (DtoExtraordinaryTransport)mapping.dtoList.FirstOrDefault().Dto;
                }

                return extraTransport;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Extraordinary Transport", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadExtra");
                throw ex;
            }
        }
        /// <summary>
        /// Register Transport 
        /// </summary>
        /// <param name="extraTransport"></param>
        /// <returns>Transport registered</returns>
        public void SaveExtra(DtoExtraordinaryTransport extraTransport)
        {
            try
            {
                User user = new User();

                if (extraTransport.ExtraordinaryTransportId == 0)
                {
                    extraTransport.UserCreated = user.GetUserName(SQLTools.userId.ToString());
                    extraTransport.Date = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = extraTransport, TableName = "ExtraordinaryTransports" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Extra Transport", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveExtra");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Transport with fields.
        /// </summary>
        /// <returns>Load Transport Dto</returns>
        public List<DtoExtraordinaryTransport> LoadMultipleExtra()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoExtraordinaryTransport> extraTransportList = new List<DtoExtraordinaryTransport>();

                mapping.Load<DtoExtraordinaryTransport>("SELECT ExtraordinaryTransportId, AssociateName, Process, Route, Shift, Stop, StartTime, StartDate, FinishTime, FinishDate, Motive, Cost, Date, UserCreated FROM ExtraordinaryTransports ORDER BY ExtraordinaryTransportId", "ExtraordinaryTransports", new DtoExtraordinaryTransport());
                extraTransportList.AddRange(mapping.dtoList.Select(renglon => (DtoExtraordinaryTransport)renglon.Dto));

                return extraTransportList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Extra Transports", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleExtra");
                throw ex;
            }
        }
        #endregion

        #region General Methods    
        /// <summary>
        /// Get Transport Id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public long GetTransportId(string route, string shift, DateTime startDate, DateTime finishDate)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT TransportId FROM Transports WHERE Route = '" + route + "' AND Shift = '" + shift + "' AND StartDate = '" + startDate.ToString("yyyyMMdd") + "' AND FinishDate = '" + finishDate.ToString("yyyyMMdd") + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get TransportId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["TransportId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Transport Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetTransportId");
                throw ex;
            }

        }
        /// <summary>
        /// Deletes a Transport. 
        /// </summary>
        /// <param name="transportId"></param>
        /// <returns></returns>
        public bool DeleteTransport(long transportId)
        {
            try
            {
                string query = "DELETE Transports WHERE TransportId =" + transportId;
                oDatabase.ExecuteNonQuery(query, "Remove Transport");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Transport", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteTransport");
                return false;
            }

        }

        /// <summary>
        /// Get Extraordinary Transport Id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public long GetExtraTransportId(string associateName, string route, string shift, DateTime? startDate, DateTime? finishDate)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT ExtraordinaryTransportId FROM ExtraordinaryTransports WHERE AssociateName = '" + associateName + "' AND Route = '" + route + "' AND Shift = '" + shift + "' AND StartDate = '" + startDate?.ToString("yyyyMMdd") + "' AND FinishDate = '" + finishDate?.ToString("yyyyMMdd") + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get Extra Transport Id");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["ExtraordinaryTransportId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Extraordinary Transport Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetExtraTransportId");
                throw ex;
            }

        }
        /// <summary>
        /// Deletes a Extra Transport. 
        /// </summary>
        /// <param name="extraTransportId"></param>
        /// <returns></returns>
        public bool DeleteExtraTransport(long extraTransportId)
        {
            try
            {
                string query = "DELETE ExtraordinaryTransports WHERE ExtraordinaryTransportId =" + extraTransportId;
                oDatabase.ExecuteNonQuery(query, "Remove Extra Transport");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Extraordinary Transport", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteExtraTransport");
                return false;
            }

        }
        #endregion
    }
}
