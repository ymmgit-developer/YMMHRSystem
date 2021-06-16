using DBFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace YMMHRSystemLogic
{
    public class Route
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        #region Standard Methods
        /// <summary>
        /// Loads the Route DTO
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns>DtoRoute Loaded</returns>
        public DtoRoute Load(long routeId)
        {
            DtoRoute dtoRoute = new DtoRoute();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (routeId != 0)
                {
                    mapping.Load<DtoRoute>("Routes", new DtoRoute(), "RouteId = " + routeId);
                    dtoRoute = (DtoRoute)mapping.dtoList.FirstOrDefault().Dto;
                }

                dtoRoute.StopList = GetRouteStopList(routeId);

                return dtoRoute;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Route", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Register Route 
        /// </summary>
        /// <param name="dtoRoute"></param>
        /// <returns>Route registered</returns>
        public void Save(DtoRoute dtoRoute)
        {
            try
            {
                User user = new User();

                if (dtoRoute.RouteId == 0)
                {
                    dtoRoute.DateAdded = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = dtoRoute, TableName = "Routes" });

                List<EDependence> dependency = new List<EDependence>();
                dependency.Add(new EDependence() { Index = 0, FKName = "RouteId" });

                dtoRoute.StopList.ForEach(item =>
                {
                    mapping.dtoList.Add(new DBFrameworkDto()
                    {
                        Dto = item,
                        TableName = "RouteStops",
                        DependenceList = dependency
                    });
                });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Route", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Route with fields.
        /// </summary>
        /// <returns>Load Route Dto</returns>
        public List<DtoRoute> LoadMultiple()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoRoute> shiftList = new List<DtoRoute>();

                mapping.Load<DtoRoute>("SELECT RouteId, Name, Cost, UserCreated, DateAdded FROM Routes ORDER BY RouteId", "Routes", new DtoRoute());
                shiftList.AddRange(mapping.dtoList.Select(renglon => (DtoRoute)renglon.Dto));

                return shiftList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Routes", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        #endregion

        #region General Methods    
        /// <summary>
        /// Get Route Id
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public long GetRouteId(string name)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT RouteId FROM Routes WHERE Name = '" + name + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get RouteId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["RouteId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Route Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetRouteId");
                throw ex;
            }

        }
        /// <summary>
        /// Deletes a Route. 
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns></returns>
        public bool DeleteRoute(long routeId)
        {
            try
            {
                string query = "DELETE Routes WHERE RouteId =" + routeId;
                oDatabase.ExecuteNonQuery(query, "Remove Route");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Route", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteRoute");
                return false;
            }

        }
        /// <summary>
        /// Gets a route's stops
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns>List of stops</returns>
        public List<DtoRouteStop> GetRouteStopList(long routeId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoRouteStop> routeStopList = new List<DtoRouteStop>();

                mapping.Load<DtoRouteStop>("SELECT RouteStopId, RouteId, StopId, StopName, Reference FROM RouteStops WHERE RouteId = " + routeId + " ORDER BY RouteId", "Routes", new DtoRouteStop());
                routeStopList.AddRange(mapping.dtoList.Select(renglon => (DtoRouteStop)renglon.Dto));

                return routeStopList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Route Stops", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetRouteStopList");
                throw ex;
            }
        }
        /// <summary>
        /// Deletes a Route Stop. 
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns></returns>
        public bool DeleteRouteStop(long routeId, long stopId)
        {
            try
            {
                string query = "DELETE RouteStops WHERE RouteId = " + routeId + " AND StopId = " +  stopId;
                oDatabase.ExecuteNonQuery(query, "Remove Route");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Route Stop", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteRouteStop");
                return false;
            }

        }
        /// <summary>
        /// Validates Stop existence within the Route Stop table
        /// </summary>
        /// <param name="routeId"></param>
        /// <param name="stopId"></param>
        /// <returns></returns>
        public bool ValidateRouteStopExistence(string stopId)
        {
            try
            {
                DataRow validation = null;
                string sqlString = "SELECT COUNT(*) FROM RouteStops WHERE StopId=" + stopId;

                validation = oDatabase.GetRow(sqlString, "Validate Route Stop Existence");

                return validation[0].ToString() == "0" ? false : true;
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion
    }
}
