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
    public class Vehicle
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        #region Standard Methods
        /// <summary>
        /// Loads the Vehicle DTO
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns>DtoVehicle Loaded</returns>
        public DtoVehicle Load(long vehicleId)
        {
            DtoVehicle vehicle = new DtoVehicle();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (vehicleId != 0)
                {
                    mapping.Load<DtoVehicle>("Vehicles", new DtoVehicle(), "VehicleId=" + vehicleId);
                    vehicle = (DtoVehicle)mapping.dtoList.FirstOrDefault().Dto;
                }

                return vehicle;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Vehicle", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Register Vehicle 
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns>Vehicle registered</returns>
        public void Save(DtoVehicle vehicle)
        {
            try
            {
                User user = new User();

                if (vehicle.VehicleId == 0)
                {
                    vehicle.DateAdded = DateTime.Now;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = vehicle, TableName = "Vehicles" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Vehicle", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Vehicle with fields.
        /// </summary>
        /// <returns>Load Vehicle Dto</returns>
        public List<DtoVehicle> LoadMultiple(bool withFilter = false, bool isUtility = true)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoVehicle> vehicleList = new List<DtoVehicle>();
                if (withFilter)
                {
                    if (isUtility)
                    {
                        mapping.Load<DtoVehicle>("SELECT VehicleId, Model, EngineNumber, VIN, Color, Type, Associate, Year, LicensePlate, PolicyNumber, PolicyStart, PolicyEnd, NextService, NextVerification, AnnualFee, InvoiceDate, DateAdded, Status FROM Vehicles WHERE Type = 'Utility Car' ORDER BY VehicleId", "Vehicle", new DtoVehicle());
                    }
                    else
                    {
                        mapping.Load<DtoVehicle>("SELECT VehicleId, Model, EngineNumber, VIN, Color, Type, Associate, Year, LicensePlate, PolicyNumber, PolicyStart, PolicyEnd, NextService, NextVerification, AnnualFee, InvoiceDate, DateAdded, Status FROM Vehicles WHERE Type <> 'Utility Car' ORDER BY VehicleId", "Vehicle", new DtoVehicle());
                    }
                }
                else
                {
                    mapping.Load<DtoVehicle>("SELECT VehicleId, Model, EngineNumber, VIN, Color, Type, Associate, Year, LicensePlate, PolicyNumber, PolicyStart, PolicyEnd, NextService, NextVerification, AnnualFee, InvoiceDate, DateAdded, Status FROM Vehicles ORDER BY VehicleId", "Vehicle", new DtoVehicle());
                }
               
                vehicleList.AddRange(mapping.dtoList.Select(renglon => (DtoVehicle)renglon.Dto));

                return vehicleList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Vehicle", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        #endregion

        #region General Methods    
        /// <summary>
        /// Get Vehicle Id
        /// </summary>
        /// <param name="engineNumber"></param>
        /// <returns></returns>
        public long GetVehicleId(string engineNumber)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT VehicleId FROM Vehicles WHERE EngineNumber = '" + engineNumber + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get VehicleId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["VehicleId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Vehicle Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetVehicleId");
                throw ex;
            }

        }
        /// <summary>
        /// Deletes a Vehicle. 
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public bool DeleteVehicle(long vehicleId)
        {
            try
            {
                string query = "DELETE Vehicles WHERE VehicleId =" + vehicleId;
                oDatabase.ExecuteNonQuery(query, "Remove Vehicle");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Vehicle", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteVehicle");
                return false;
            }

        }
        #endregion
    }
}
