using DBFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMMHRSystemLogic;

namespace YMMHRSystemLogic
{
    public class UtilityCar
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        SendEmail sendEmail = new SendEmail();
        EmailNotification emailNotification = new EmailNotification();

        #region Standard Methods
        /// <summary>
        /// Loads the Utility Car DTO
        /// </summary>
        /// <param name="utilityCarId"></param>
        /// <returns>DtoUtilityCar Loaded</returns>
        public DtoUtilityCar Load(long utilityCarId)
        {
            DtoUtilityCar utilityCar = new DtoUtilityCar();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (utilityCarId != 0)
                {
                    mapping.Load<DtoUtilityCar>("UtilityCars", new DtoUtilityCar(), "UtilityCarId=" + utilityCarId);
                    utilityCar = (DtoUtilityCar)mapping.dtoList.FirstOrDefault().Dto;
                }

                return utilityCar;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Utility Car", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Register UtilityCar 
        /// </summary>
        /// <param name="utilityCar"></param>
        /// <returns>UtilityCar registered</returns>
        public void Save(DtoUtilityCar utilityCar, long userId = 0)
        {
            try
            {
                User user = new User();
                if (utilityCar.UtilityCarId == 0)
                {
                    utilityCar.DateAdded = DateTime.Now;
                    utilityCar.Status = 1;
                    utilityCar.CreatedBy = userId;

                    List<string> contacts = emailNotification.GetUtilityCarContacts(1).Split(',').ToList();
                    contacts.Add(user.GetUserEmail(userId));

                    sendEmail.SendEmailTemplate("YMM HR System: Utility Car Request", "TemplateUtilityCarRequest", new[,]
                    {
                        {"$APPLICANT$", utilityCar.Associate},
                        {"$PASSENGERS$", utilityCar.Passengers.ToString()},
                        {"$DESTINATION$", utilityCar.Destination },
                        {"$DATE$", utilityCar.DepartureDate?.ToString("dd/MM/yyyy")}
                    }, sendEmail.GetAdminEmail(), contacts);
                }

                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = utilityCar, TableName = "UtilityCars" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Utility Car", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary> 
        /// Load multiple Utility Car with fields.
        /// </summary>
        /// <returns>Load Utility Car Dto</returns>
        public List<DtoUtilityCar> LoadMultiple(string userFilter = "")
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoUtilityCar> utilityCarList = new List<DtoUtilityCar>();
                if (userFilter == "")
                {
                    mapping.Load<DtoUtilityCar>("SELECT UtilityCarId, UtilityCar, LicensePlate, Associate, Destination, Passengers, Motive, Observations, DepartureDate, ArrivalDate, DepartureTime, ArrivalTime, LicenseExpiration, Card, KMDeparture, KMArrival, TankLevelDeparture, TankLevelArrival, Status, DateAdded FROM UtilityCars ORDER BY UtilityCarId", "UtilityCars", new DtoUtilityCar());
                }
                else
                {
                    mapping.Load<DtoUtilityCar>("SELECT UtilityCarId, UtilityCar, LicensePlate, Associate, Destination, Passengers, Motive, Observations, DepartureDate, ArrivalDate, DepartureTime, ArrivalTime, LicenseExpiration, Card, KMDeparture, KMArrival, TankLevelDeparture, TankLevelArrival, Status, DateAdded FROM UtilityCars WHERE Associate = '" + userFilter + "' ORDER BY UtilityCarId", "UtilityCars", new DtoUtilityCar());
                }
               
                utilityCarList.AddRange(mapping.dtoList.Select(renglon => (DtoUtilityCar)renglon.Dto));

                return utilityCarList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple UtilityCar", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        /// <summary> 
        /// Load multiple Utility Car with fields.
        /// </summary>
        /// <returns>Load Utility Car Dto</returns>
        public List<DtoVehicle> LoadMultipleAvailableCars(DateTime? date)
        {
            try
            {
                Vehicle vehicle = new Vehicle();
                var vehicles = vehicle.LoadMultiple(true);
                var utilityCars = LoadMultiple();
                List<DtoVehicle> carList = new List<DtoVehicle>();
                List<DtoUtilityCar> busyUtilityCars = utilityCars.Where(x => utilityCars.Any(y => (date >= y.DepartureDate && date <= y.ArrivalDate) && x.Status != 3)).ToList();
                if (busyUtilityCars.Count > 0)
                {
                     carList = vehicles.Where(x => !busyUtilityCars.Any(y => y.LicensePlate == x.LicensePlate)).ToList();
                }
                else
                {
                    carList = vehicles;
                }


                return carList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple UtilityCar", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        #endregion

        #region General Methods    
        /// <summary>
        /// Deletes a UtilityCar. 
        /// </summary>
        /// <param name="utilityCarId"></param>
        /// <returns></returns>
        public bool DeleteUtilityCar(long utilityCarId)
        {
            try
            {
                string query = "DELETE UtilityCars WHERE UtilityCarId =" + utilityCarId;
                oDatabase.ExecuteNonQuery(query, "Remove UtilityCar");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete UtilityCar", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteUtilityCar");
                return false;
            }

        }
        /// <summary>
        /// Get UtilityCar status
        /// </summary>
        /// <param name="utilityCarId"></param>
        /// <returns></returns>
        public int GetUtilityCarStatus(long utilityCarId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT Status FROM UtilityCars WHERE UtilityCarId = " + utilityCarId;

                dataRow = oDatabase.GetRow(sqlString, "Get UtilityCar Status");

                if (dataRow == null) return 0;

                return Convert.ToInt32(dataRow["Status"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get UtilityCar Status", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetUtilityCarStatus");
                throw ex;
            }
        }
        #endregion
    }
}
