using DBFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using OfficeOpenXml;

namespace YMMHRSystemLogic
{
    public class BusinessTrip
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        SendEmail sendEmail = new SendEmail();
        User user = new User();
        EmailNotification emailNotification = new EmailNotification();

        #region Standard Methods
        /// <summary>
        /// Loads the BusinessTrip DTO
        /// </summary>
        /// <param name="businessTripId"></param>
        /// <returns>DtoBusinessTrip Loaded</returns>
        public DtoBusinessTrip Load(long businessTripId)
        {
            DtoBusinessTrip businessTrip = new DtoBusinessTrip();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (businessTripId != 0)
                {
                    mapping.Load<DtoBusinessTrip>("BusinessTrips", new DtoBusinessTrip(), "BusinessTripId=" + businessTripId);
                    businessTrip = (DtoBusinessTrip)mapping.dtoList.FirstOrDefault().Dto;
                }

                businessTrip.PassengerList = LoadMultiplePassengers(businessTrip.BusinessTripId);
                businessTrip.TaxiList = LoadMultipleTaxis(businessTrip.BusinessTripId);
                businessTrip.AttachmentList = LoadMultipleAttachments(businessTrip.BusinessTripId);
                businessTrip.FlightList = LoadMultipleFlights(businessTrip.BusinessTripId);
                businessTrip.CarRentalList = LoadMultipleCarRentals(businessTrip.BusinessTripId);
                businessTrip.HotelList = LoadMultipleHotels(businessTrip.BusinessTripId);

                return businessTrip;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load BusinessTrip", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Register BusinessTrip 
        /// </summary>
        /// <param name="businessTrip"></param>
        /// <returns>BusinessTrip registered</returns>
        public void Save(DtoBusinessTrip businessTrip, long userId = 0)
        {
            try
            {
                if (businessTrip.BusinessTripId == 0)
                {
                    businessTrip.CreatedBy = userId;
                    List<string> contacts = emailNotification.GetBusinessTripContacts(1).Split(',').ToList();
                    contacts.Add(user.GetUserEmail(userId));

                    sendEmail.SendEmailTemplate("YMM HR System: Business Trip Request", "TemplateBusinessTripRequest", new[,]
{
                        {"$APPLICANT$", businessTrip.UserCreated},
                        {"$PROCESS$", businessTrip.Process},
                        {"$DATE$", businessTrip.DateAdded.ToString("dd/MM/yyyy")}
                    }, sendEmail.GetAdminEmail(), contacts);
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = businessTrip, TableName = "BusinessTrips" });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save BusinessTrip", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple BusinessTrip with fields.
        /// </summary>
        /// <returns>Load BusinessTrip Dto</returns>
        public List<DtoBusinessTrip> LoadMultiple(string userFilter = "")
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoBusinessTrip> workerfileList = new List<DtoBusinessTrip>();
                if (userFilter == "")
                {
                    mapping.Load<DtoBusinessTrip>("SELECT BusinessTripId, Associate, Process, DateAdded, Status, UserCreated, CreatedBy FROM BusinessTrips ORDER BY BusinessTripId", "BusinessTrips", new DtoBusinessTrip());
                }
                else
                {
                    mapping.Load<DtoBusinessTrip>("SELECT BusinessTripId, Associate, Process, DateAdded, Status, UserCreated, CreatedBy FROM BusinessTrips WHERE Associate = '" + userFilter + "' ORDER BY BusinessTripId", "BusinessTrips", new DtoBusinessTrip());
                }
               
                workerfileList.AddRange(mapping.dtoList.Select(renglon => (DtoBusinessTrip)renglon.Dto));

                return workerfileList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple BusinessTrips", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        #endregion

        #region General Methods    
        /// <summary>
        /// Adds Passenger
        /// </summary>
        /// <param name="dtoPassengerDetail"></param>
        /// <returns></returns>
        public bool AddPassenger(DtoPassengerDetail dtoPassengerDetail)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = dtoPassengerDetail, TableName = "PassengerDetails" });

                mapping.Save();

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Add Passenger", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "AddPassenger");
                return false;
            }

        }
        /// <summary>
        /// Adds Taxi Reservation 
        /// </summary>
        /// <param name="dtoTaxiReservation"></param>
        /// <returns></returns>
        public bool AddTaxiReservation(DtoTaxiReservation dtoTaxiReservation)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = dtoTaxiReservation, TableName = "TaxiReservations" });

                mapping.Save();

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Adds Taxi Reservation", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "AddTaxiReservation");
                return false;
            }

        }
        /// <summary>
        /// Adds an attachment 
        /// </summary>
        /// <param name="dtoBusinessTripAttachment"></param>
        /// <returns></returns>
        public bool AddAttachment(DtoBusinessTripAttachment dtoBusinessTripAttachment)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = dtoBusinessTripAttachment, TableName = "BusinessTripAttachments" });

                mapping.Save();

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Add Attachment", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "AddAttachment");
                return false;
            }

        }
        /// <summary>
        /// Adds a Flight Reservation
        /// </summary>
        /// <param name="dtoFlightReservation"></param>
        /// <returns></returns>
        public bool AddFlightReservation(DtoFlightReservation dtoFlightReservation)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = dtoFlightReservation, TableName = "FlightReservations" });

                mapping.Save();

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Add Flight Reservation", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "AddFlightReservation");
                return false;
            }

        }
        /// <summary>
        /// Adds a Car Rental Reservation
        /// </summary>
        /// <param name="dtoCarRentalReservation"></param>
        /// <returns></returns>
        public bool AddCarRentalReservation(DtoCarRentalReservation dtoCarRentalReservation)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = dtoCarRentalReservation, TableName = "CarRentalReservations" });

                mapping.Save();

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Add CarRentalReservation", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "AddCarRentalReservation");
                return false;
            }

        }
        /// <summary>
        /// Adds a Hotel Reservation
        /// </summary>
        /// <param name="dtoHotelReservation"></param>
        /// <returns></returns>
        public bool AddHotelReservation(DtoHotelReservation dtoHotelReservation)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = dtoHotelReservation, TableName = "HotelReservations" });

                mapping.Save();

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Add HotelReservation", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "AddHotelReservation");
                return false;
            }

        }

        /// <summary>
        /// Get Business Trip status
        /// </summary>
        /// <param name="businessTripId"></param>
        /// <returns></returns>
        public int GetBusinessTripStatus(long businessTripId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT Status FROM BusinessTrips WHERE BusinessTripId = " + businessTripId;

                dataRow = oDatabase.GetRow(sqlString, "Get BusinessTrip Status");

                if (dataRow == null) return 0;

                return Convert.ToInt32(dataRow["Status"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get BusinessTrip Status", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetBusinessTripStatus");
                throw ex;
            }
        }
        /// <summary>
        /// Get Worker Attachment File 
        /// </summary>
        /// <returns>File name</returns>
        public string GetBusinessTripAttachmentFile(long businessTripAttachmentId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT FileName FROM BusinessTripAttachments WHERE BusinessTripAttachmentId = " + businessTripAttachmentId;

                dataRow = oDatabase.GetRow(sqlString, "Get File Name");

                if (dataRow == null) return "";

                return dataRow["FileName"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Business Trip Attachment File", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetBusinessTripAttachmentFile");
                throw ex;
            }
        }

        /// <summary>
        /// Load multiple BusinessTrip with fields.
        /// </summary>
        /// <returns>Load BusinessTrip Dto</returns>
        public List<DtoPassengerDetail> LoadMultiplePassengers(long businessTripId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoPassengerDetail> passengerList = new List<DtoPassengerDetail>();

                mapping.Load<DtoPassengerDetail>("SELECT PassengerDetailId, BusinessTripId, Passenger, CompanyName, Purpose, Type, Notes FROM PassengerDetails WHERE BusinessTripId = " + businessTripId + " ORDER BY PassengerDetailId", "PassengerDetails", new DtoPassengerDetail());
                passengerList.AddRange(mapping.dtoList.Select(renglon => (DtoPassengerDetail)renglon.Dto));

                return passengerList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Passenger Details", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiplePassengerDetails");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Business Trip with fields.
        /// </summary>
        /// <returns>Load Business Trip Dto</returns>
        public List<DtoTaxiReservation> LoadMultipleTaxis(long businessTripId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoTaxiReservation> taxiList = new List<DtoTaxiReservation>();

                mapping.Load<DtoTaxiReservation>("SELECT TaxiReservationId, Cost, BusinessTripId, QuoteNumber, DriverName, LicensePlate, PickupDate, PickupTime, PickupPlace, FlightNumber, Destination, CompanyPaying, CostCenter FROM TaxiReservations WHERE BusinessTripId = " + businessTripId + " ORDER BY TaxiReservationId", "TaxiReservations", new DtoTaxiReservation());
                taxiList.AddRange(mapping.dtoList.Select(renglon => (DtoTaxiReservation)renglon.Dto));

                return taxiList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Taxis", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleTaxis");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple flights
        /// </summary>
        /// <param name="businessTripId"></param>
        /// <returns></returns>
        public List<DtoFlightReservation> LoadMultipleFlights(long businessTripId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoFlightReservation> flightList = new List<DtoFlightReservation>();

                mapping.Load<DtoFlightReservation>("SELECT FlightReservationId, Cost, FlyerMilesCard, BusinessTripId, PassengerName, PassportNumber, FlightType, DepartureAirport, ArrivalAirport, Departure, Arrival, Origin, Destination, Airline, FlightDate, FlightTime, CompanyPaying, Class, FlightNumber, CostCenter  FROM FlightReservations WHERE BusinessTripId = " + businessTripId + " ORDER BY FlightReservationId", "FlightReservations", new DtoFlightReservation());
                flightList.AddRange(mapping.dtoList.Select(renglon => (DtoFlightReservation)renglon.Dto));

                return flightList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Flight Reservations", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleFlights");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Attachments
        /// </summary>
        /// <param name="businessTripId"></param>
        /// <returns></returns>
        public List<DtoBusinessTripAttachment> LoadMultipleAttachments(long businessTripId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoBusinessTripAttachment> attachmentList = new List<DtoBusinessTripAttachment>();

                mapping.Load<DtoBusinessTripAttachment>("SELECT BusinessTripAttachmentId, BusinessTripId, FileName FROM BusinessTripAttachments WHERE BusinessTripId = " + businessTripId + " ORDER BY BusinessTripAttachmentId", "BusinessTripAttachments", new DtoBusinessTripAttachment());
                attachmentList.AddRange(mapping.dtoList.Select(renglon => (DtoBusinessTripAttachment)renglon.Dto));

                return attachmentList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Business Attachment", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleAttachments");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Car Rental Reservations
        /// </summary>
        /// <param name="businessTripId"></param>
        /// <returns></returns>
        public List<DtoCarRentalReservation> LoadMultipleCarRentals(long businessTripId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoCarRentalReservation> carRentalReservationList = new List<DtoCarRentalReservation>();

                mapping.Load<DtoCarRentalReservation>("SELECT CarRentalReservationId, Cost, BusinessTripId, DriverName, LicenseNumber, PickupDate, PickupTime, PickupPlace, ReturnDate, ReturnTime, ReturnPlace, ReservationNumber, CarType, CompanyPaying, CostCenter FROM CarRentalReservations WHERE BusinessTripId = " + businessTripId + " ORDER BY CarRentalReservationId", "CarRentalReservations", new DtoCarRentalReservation());
                carRentalReservationList.AddRange(mapping.dtoList.Select(renglon => (DtoCarRentalReservation)renglon.Dto));

                return carRentalReservationList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Car Rental Reservations", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleCarRentals");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Hotel Reservations
        /// </summary>
        /// <param name="businessTripId"></param>
        /// <returns></returns>
        public List<DtoHotelReservation> LoadMultipleHotels(long businessTripId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoHotelReservation> hotelList = new List<DtoHotelReservation>();

                mapping.Load<DtoHotelReservation>("SELECT HotelReservationId, Cost, BusinessTripId, CheckIn, CheckOut, Place, Notes, HotelName, ReservationNumber, Address, Telephone, CompanyPaying, CostCenter FROM HotelReservations WHERE BusinessTripId = " + businessTripId + " ORDER BY HotelReservationId", "HotelReservations", new DtoHotelReservation());
                hotelList.AddRange(mapping.dtoList.Select(renglon => (DtoHotelReservation)renglon.Dto));

                return hotelList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Hotel Reservations", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleHotels");
                throw ex;
            }
        }

        /// <summary>
        /// Load Passenger with fields.
        /// </summary>
        /// <returns>Load Passenger Dto</returns>
        public DtoPassengerDetail LoadPassenger(long passengerDetailId)
        {
            try
            {
                DtoPassengerDetail dtoPassengerDetail = new DtoPassengerDetail();
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (passengerDetailId != 0)
                {
                    mapping.Load<DtoPassengerDetail>("PassengerDetails", new DtoPassengerDetail(), "PassengerDetailId=" + passengerDetailId);
                    dtoPassengerDetail = (DtoPassengerDetail)mapping.dtoList.FirstOrDefault().Dto;
                }
                return dtoPassengerDetail;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Passenger", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadPassenger");
                throw ex;
            }
        }
        /// <summary>
        /// Load Taxi with fields.
        /// </summary>
        /// <returns>Load Taxi Dto</returns>
        public DtoTaxiReservation LoadTaxi(long taxiReservationId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                DtoTaxiReservation dtoTaxiReservation = new DtoTaxiReservation();
                if (taxiReservationId != 0)
                {
                    mapping.Load<DtoTaxiReservation>("TaxiReservations", new DtoTaxiReservation(), "TaxiReservationId=" + taxiReservationId);
                    dtoTaxiReservation = (DtoTaxiReservation)mapping.dtoList.FirstOrDefault().Dto;
                }
                return dtoTaxiReservation;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Taxi", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadTaxi");
                throw ex;
            }
        }
        /// <summary>
        /// Load Flight
        /// </summary>
        /// <param name="flightReservationId"></param>
        /// <returns></returns>
        public DtoFlightReservation LoadFlight(long flightReservationId)
        {
            try
            {
                DtoFlightReservation dtoFlightReservation = new DtoFlightReservation();
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (flightReservationId != 0)
                {
                    mapping.Load<DtoFlightReservation>("FlightReservations", new DtoFlightReservation(), "FlightReservationId=" + flightReservationId);
                    dtoFlightReservation = (DtoFlightReservation)mapping.dtoList.FirstOrDefault().Dto;
                }
                return dtoFlightReservation;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Flight", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadFlights");
                throw ex;
            }
        }
        /// <summary>
        /// Load  Car Rental Reservations
        /// </summary>
        /// <param name="businessTripId"></param>
        /// <returns></returns>
        public DtoCarRentalReservation LoadCarRental(long carRentalReservationId)
        {
            try
            {
                DtoCarRentalReservation dtoCarRental = new DtoCarRentalReservation();
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (carRentalReservationId != 0)
                {
                    mapping.Load<DtoCarRentalReservation>("CarRentalReservations", new DtoCarRentalReservation(), "CarRentalReservationId=" + carRentalReservationId);
                    dtoCarRental = (DtoCarRentalReservation)mapping.dtoList.FirstOrDefault().Dto;
                }
                return dtoCarRental;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Car Rental", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadCarRental");
                throw ex;
            }
        }
        /// <summary>
        /// Load Hotel Reservation
        /// </summary>
        /// <param name="hotelReservationId"></param>
        /// <returns></returns>
        public DtoHotelReservation LoadHotel(long hotelReservationId)
        {
            try
            {
                DtoHotelReservation dtoHotelReservation = new DtoHotelReservation();
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (hotelReservationId != 0)
                {
                    mapping.Load<DtoHotelReservation>("HotelReservations", new DtoHotelReservation(), "HotelReservationId=" + hotelReservationId);
                    dtoHotelReservation = (DtoHotelReservation)mapping.dtoList.FirstOrDefault().Dto;
                }
                return dtoHotelReservation;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Hotel Reservation", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadHotel");
                throw ex;
            }
        }

        /// <summary>
        /// Deletes a Business Trip. 
        /// </summary>
        /// <param name="businessTripId"></param>
        /// <returns></returns>
        public bool DeleteBusinessTrip(long businessTripId)
        {
            try
            {
                string query = "DELETE BusinessTrips WHERE BusinessTripId =" + businessTripId;
                oDatabase.ExecuteNonQuery(query, "Remove Business Trips");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Business Trip", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteBusinessTrip");
                return false;
            }
        }
        /// <summary>
        /// Deletes a PassengerDetail. 
        /// </summary>
        /// <param name="passengerDetailId"></param>
        /// <returns></returns>
        public bool DeletePassengerDetail(long passengerDetailId)
        {
            try
            {
                string query = "DELETE PassengerDetails WHERE PassengerDetailId =" + passengerDetailId;
                oDatabase.ExecuteNonQuery(query, "Remove PassengerDetail");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete PassengerDetail", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeletePassengerDetail");
                return false;
            }
        }
        /// <summary>
        /// Deletes a Taxi Reservation. 
        /// </summary>
        /// <param name="taxiReservationId"></param>
        /// <returns></returns>
        public bool DeleteTaxiReservation(long taxiReservationId)
        {
            try
            {
                string query = "DELETE TaxiReservations WHERE TaxiReservationId =" + taxiReservationId;
                oDatabase.ExecuteNonQuery(query, "Remove Taxi Reservation");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Taxi Reservation", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteTaxiReservation");
                return false;
            }
        }
        /// <summary>
        /// Deletes a Flight Reservation. 
        /// </summary>
        /// <param name="flightReservationId"></param>
        /// <returns></returns>
        public bool DeleteFlightReservation(long flightReservationId)
        {
            try
            {
                string query = "DELETE FlightReservations WHERE FlightReservationId =" + flightReservationId;
                oDatabase.ExecuteNonQuery(query, "Remove FlightReservation");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Flight Reservation", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteFlightReservation");
                return false;
            }
        }
        /// <summary>
        /// Deletes a CarRentalReservation. 
        /// </summary>
        /// <param name="carRentalReservationId"></param>
        /// <returns></returns>
        public bool DeleteCarRentalReservation(long carRentalReservationId)
        {
            try
            {
                string query = "DELETE CarRentalReservations WHERE CarRentalReservationId =" + carRentalReservationId;
                oDatabase.ExecuteNonQuery(query, "Remove Car Rental Reservation");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Car Rental Reservation", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteCarRentalReservation");
                return false;
            }
        }
        /// <summary>
        /// Deletes an Attachment. 
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <returns></returns>
        public bool DeleteAttachment(long attachmentId, string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                string query = "DELETE BusinessTripAttachments WHERE BusinessTripAttachmentId =" + attachmentId;
                oDatabase.ExecuteNonQuery(query, "Remove Attachment");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Attachment", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteAttachment");
                return false;
            }
        }
        /// <summary>
        /// Deletes a Hotel Reservation. 
        /// </summary>
        /// <param name="carRentalReservationId"></param>
        /// <returns></returns>
        public bool DeleteHotelReservation(long hotelReservationId)
        {
            try
            {
                string query = "DELETE HotelReservations WHERE HotelReservationId =" + hotelReservationId;
                oDatabase.ExecuteNonQuery(query, "Remove Car Rental Reservation");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Hotel Reservation", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteHotelReservation");
                return false;
            }
        }
        #endregion
    }
}
