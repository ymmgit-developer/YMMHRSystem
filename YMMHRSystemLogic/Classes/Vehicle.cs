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
                    vehicle.ServiceEmailSent = false;
                    vehicle.VerificationEmailSent = false;
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
                        mapping.Load<DtoVehicle>("SELECT VehicleId, Model, EngineNumber, VIN, Color, Type, Associate, Year, LicensePlate, PolicyNumber, PolicyStart, PolicyEnd, NextService, NextVerification, AnnualFee, InvoiceDate, DateAdded, VerificationStatus, Status, VerificationEmailSent, ServiceEmailSent FROM Vehicles WHERE Type = 'Utility Car' ORDER BY VehicleId", "Vehicle", new DtoVehicle());
                    }
                    else
                    {
                        mapping.Load<DtoVehicle>("SELECT VehicleId, Model, EngineNumber, VIN, Color, Type, Associate, Year, LicensePlate, PolicyNumber, PolicyStart, PolicyEnd, NextService, NextVerification, AnnualFee, InvoiceDate, DateAdded, VerificationStatus, Status, VerificationEmailSent, ServiceEmailSent FROM Vehicles WHERE Type <> 'Utility Car' ORDER BY VehicleId", "Vehicle", new DtoVehicle());
                    }
                }
                else
                {
                    mapping.Load<DtoVehicle>("SELECT VehicleId, Model, EngineNumber, VIN, Color, Type, Associate, Year, LicensePlate, PolicyNumber, PolicyStart, PolicyEnd, NextService, NextVerification, AnnualFee, InvoiceDate, DateAdded, VerificationStatus, Status, ServiceEmailSent, VerificationEmailSent FROM Vehicles ORDER BY VehicleId", "Vehicle", new DtoVehicle());
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
        /// Cancel a Vehicle. 
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public bool CancelVehicle(long vehicleId)
        {
            try
            {
                string query = "UPDATE Vehicles SET Status = 2 WHERE VehicleId =" + vehicleId;
                oDatabase.ExecuteNonQuery(query, "Cancel Vehicle");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Vehicle", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteVehicle");
                return false;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="vehicleId"></param>
        public void UpdateVerificationEmailFlag(bool flag, long vehicleId)
        {
            try
            {
                string query = "UPDATE Vehicles SET VerificationEmailSent = " + (flag ? "1" : "0") + " WHERE VehicleId =" + vehicleId;
                oDatabase.ExecuteNonQuery(query, "Update Vehicle Email Flag");

            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Update Verification Email Flag", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "UpdateVerificationEmailFlag");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="vehicleId"></param>
        public void UpdateServiceEmailFlag(bool flag, long vehicleId)
        {
            try
            {
                string query = "UPDATE Vehicles SET ServiceEmailSent = " + (flag ? "1" : "0") + " WHERE VehicleId =" + vehicleId;
                oDatabase.ExecuteNonQuery(query, "Update Vehicle Email Flag");

            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Update Service Email Flag", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "UpdateServiceEmailFlag");
            }
        }
        #endregion
    }
}
