using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class BusinessTripController : Controller
    {
        BusinessTrip businessTrip = new BusinessTrip();
        SendEmail sendEmail = new SendEmail();
        EmailNotification emailNotification = new EmailNotification();

        // GET: YMMHRSystem/BusinessTrip
        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("BUSINESS.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveBusinessTrip"] = Permission.QueryPermission("BUSINESS.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    HttpContext.Session["CanDeleteBusinessTrip"] = Permission.QueryPermission("BUSINESS.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    Session["LoadedBusinessTripId"] = null;
                    return View("~/Areas/YMMHRSystem/Views/BusinessTrip/Index.cshtml", businessTrip.LoadMultiple());
                }
                else
                {
                    return View("~/Views/Shared/AccessDenied.cshtml");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a BusinessTrip.
        /// </summary>
        /// <param name="dtoBusinessTrip"></param>
        /// <returns></returns>
        public ActionResult SaveBusinessTrip(DtoBusinessTrip dtoBusinessTrip)
        {
            try
            {

                dtoBusinessTrip.BusinessTripId = Convert.ToInt64(Session["LoadedBusinessTripId"]);
                businessTrip.Save(dtoBusinessTrip);
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult NewBusinessTrip()
        {
            try
            {
                DtoBusinessTrip dtoBusinessTrip = new DtoBusinessTrip();
                User user = new User();
                dtoBusinessTrip.Associate = Session["UserName"].ToString();
                dtoBusinessTrip.DateAdded = DateTime.Now;
                dtoBusinessTrip.Status = 1;
                dtoBusinessTrip.UserCreated = Session["UserName"].ToString();
                businessTrip.Save(dtoBusinessTrip);
                Session["LoadedBusinessTripId"] = dtoBusinessTrip.BusinessTripId;

                return RedirectToAction("LoadBusinessTrip", new { nMR4z = dtoBusinessTrip.BusinessTripId });
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// Load view with new/exisiting BusinessTrip.
        /// </summary>
        /// <param name="nMR4z"></param>
        /// <returns></returns>
        public ActionResult LoadBusinessTrip(long nMR4z)
        {
            try
            {
                Session["LoadedBusinessTripId"] = nMR4z;

                DtoBusinessTrip dtoBusinessTrip = businessTrip.Load(nMR4z);

                return View("~/Areas/YMMHRSystem/Views/BusinessTrip/BusinessTripDetail.cshtml", dtoBusinessTrip);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Removes a BusinessTrip. 
        /// </summary>
        /// <param name="businessTripId"></param>
        /// <returns></returns>
        public string DeleteBusinessTrip(long businessTripId)
        {
            try
            {
                return businessTrip.DeleteBusinessTrip(businessTripId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Removes a attachment file. 
        /// </summary>
        /// <param name="businessTripAttachmentId"></param>
        /// <returns></returns>
        public string DeleteAttachmentFile(long businessTripAttachmentId)
        {
            try
            {
                if (businessTrip.GetBusinessTripStatus(Convert.ToInt64(Session["LoadedBusinessTripId"])) != 2)
                {
                    string fileName = businessTrip.GetBusinessTripAttachmentFile(businessTripAttachmentId);
                    return businessTrip.DeleteAttachment(businessTripAttachmentId, fileName) ? "true" : "false";
                }
                else
                {
                    return "false";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Get attachment file
        /// </summary>
        /// <param name="businessTripAttachmentId"></param>
        /// <returns>file</returns>
        public ActionResult GetAttachmentPDF(long businessTripAttachmentId)
        {
            try
            {
                string file = businessTrip.GetBusinessTripAttachmentFile(businessTripAttachmentId);
                return File(file, "application/pdf");
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        /// <summary>
        /// Load attachment view.
        /// </summary>
        /// <returns></returns>
        public ActionResult AttachmentDialog()
        {
            try
            {
                return View("~/Areas/YMMHRSystem/Views/BusinessTrip/AttachmentDialog.cshtml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Adds a Worker Attachment. 
        /// </summary>
        /// <param name="BusinessTripId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddAttachment(HttpPostedFileBase file)
        {
            try
            {
                if (Session["LoadedBusinessTripId"] != null)
                {
                    if (businessTrip.GetBusinessTripStatus(Convert.ToInt64(Session["LoadedBusinessTripId"])) != 2)
                    {
                        DtoBusinessTripAttachment dtoBusinessTripAttachment = new DtoBusinessTripAttachment();
                        dtoBusinessTripAttachment.BusinessTripId = Convert.ToInt64(Session["LoadedBusinessTripId"]);

                        if (file != null)
                        {
                            string checkextension = Path.GetExtension(file.FileName).ToLower();
                            if (checkextension == ".pdf")
                            {
                                string path = Server.MapPath("~/BusinessTrips/Attachments/" + dtoBusinessTripAttachment.BusinessTripId + "/");
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                dtoBusinessTripAttachment.FileName = path + file.FileName;
                                file.SaveAs(path + file.FileName);
                            }
                            else
                            {
                                return Json("false", JsonRequestBehavior.AllowGet);
                            }

                        }
                        return businessTrip.AddAttachment(dtoBusinessTripAttachment) ? Json("true", JsonRequestBehavior.AllowGet) : Json("false", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Load Passenger view
        /// </summary>
        /// <returns></returns>
        public ActionResult PassengerDialog(long passengerDetailId)
        {
            try
            {
                DtoPassengerDetail dtoPassengerDetail = businessTrip.LoadPassenger(passengerDetailId);
                return View("~/Areas/YMMHRSystem/Views/BusinessTrip/PassengerDialog.cshtml", dtoPassengerDetail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Adds a Passenger
        /// </summary>
        /// <param name="dtoPassenger"></param>
        /// <returns></returns>
        public ActionResult AddPassenger(DtoPassengerDetail dtoPassenger)
        {
            try
            {
                if (Session["LoadedBusinessTripId"] != null)
                {
                    if (businessTrip.GetBusinessTripStatus(Convert.ToInt64(Session["LoadedBusinessTripId"])) != 2)
                    {
                        dtoPassenger.BusinessTripId = Convert.ToInt64(Session["LoadedBusinessTripId"]);
                        businessTrip.AddPassenger(dtoPassenger);
                    }
                    else
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                    return Json("true", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Removes a Passenger. 
        /// </summary>
        /// <param name="passengerDetailId"></param>
        /// <returns></returns>
        public string DeletePassenger(long passengerDetailId)
        {
            try
            {
                if (businessTrip.GetBusinessTripStatus(Convert.ToInt64(Session["LoadedBusinessTripId"])) != 2)
                {
                    return businessTrip.DeletePassengerDetail(passengerDetailId) ? "true" : "false";
                }
                else
                {
                    return "false";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Load Taxi Reservation view
        /// </summary>
        /// <returns></returns>
        public ActionResult TaxiDialog(long taxiReservationId)
        {
            try
            {
                DtoTaxiReservation dtoTaxiReservation = businessTrip.LoadTaxi(taxiReservationId);
                return View("~/Areas/YMMHRSystem/Views/BusinessTrip/TaxiDialog.cshtml", dtoTaxiReservation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Adds a Taxi Reservation
        /// </summary>
        /// <param name="dtoTaxiReservation"></param>
        /// <returns></returns>
        public ActionResult AddTaxi(DtoTaxiReservation dtoTaxiReservation)
        {
            try
            {
                if (Session["LoadedBusinessTripId"] != null)
                {
                    if (businessTrip.GetBusinessTripStatus(Convert.ToInt64(Session["LoadedBusinessTripId"])) != 2)
                    {
                        dtoTaxiReservation.BusinessTripId = Convert.ToInt64(Session["LoadedBusinessTripId"]);
                        businessTrip.AddTaxiReservation(dtoTaxiReservation);

                        return Json("true", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Removes a Taxi Reservation. 
        /// </summary>
        /// <param name="taxiReservationId"></param>
        /// <returns></returns>
        public string DeleteTaxi(long taxiReservationId)
        {
            try
            {
                if (businessTrip.GetBusinessTripStatus(Convert.ToInt64(Session["LoadedBusinessTripId"])) != 2)
                {
                    return businessTrip.DeleteTaxiReservation(taxiReservationId) ? "true" : "false";
                }
                else
                {
                    return "false";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Load Hotel Reservation view
        /// </summary>
        /// <returns></returns>
        public ActionResult HotelDialog(long hotelReservationId)
        {
            try
            {
                DtoHotelReservation dtoHotelReservation = businessTrip.LoadHotel(hotelReservationId);
                return View("~/Areas/YMMHRSystem/Views/BusinessTrip/HotelDialog.cshtml", dtoHotelReservation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Adds a Hotel Reservation
        /// </summary>
        /// <param name="dtoHotelReservation"></param>
        /// <returns></returns>
        public ActionResult AddHotel(DtoHotelReservation dtoHotelReservation)
        {
            try
            {
                if (Session["LoadedBusinessTripId"] != null)
                {
                    if (businessTrip.GetBusinessTripStatus(Convert.ToInt64(Session["LoadedBusinessTripId"])) != 2)
                    {
                        dtoHotelReservation.BusinessTripId = Convert.ToInt64(Session["LoadedBusinessTripId"]);
                        businessTrip.AddHotelReservation(dtoHotelReservation);

                        return Json("true", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Removes a Hotel Reservation. 
        /// </summary>
        /// <param name="hotelReservationId"></param>
        /// <returns></returns>
        public string DeleteHotel(long hotelReservationId)
        {
            try
            {
                if (businessTrip.GetBusinessTripStatus(Convert.ToInt64(Session["LoadedBusinessTripId"])) != 2)
                {
                    return businessTrip.DeleteHotelReservation(hotelReservationId) ? "true" : "false";
                }
                else
                {
                    return "false";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Load Car Rental view
        /// </summary>
        /// <returns></returns>
        public ActionResult CarRentalDialog(long carRentalReservationId)
        {
            try
            {
                DtoCarRentalReservation dtoCarRentalReservation = businessTrip.LoadCarRental(carRentalReservationId);
                return View("~/Areas/YMMHRSystem/Views/BusinessTrip/CarRentalDialog.cshtml", dtoCarRentalReservation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Adds a Car Rental
        /// </summary>
        /// <param name="dtoCarRentalReservation"></param>
        /// <returns></returns>
        public ActionResult AddCarRental(DtoCarRentalReservation dtoCarRentalReservation)
        {
            try
            {
                if (Session["LoadedBusinessTripId"] != null)
                {
                    if (businessTrip.GetBusinessTripStatus(Convert.ToInt64(Session["LoadedBusinessTripId"])) != 2)
                    {
                        dtoCarRentalReservation.BusinessTripId = Convert.ToInt64(Session["LoadedBusinessTripId"]);
                        businessTrip.AddCarRentalReservation(dtoCarRentalReservation);

                        return Json("true", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Removes a Car Rental Reservation. 
        /// </summary>
        /// <param name="carRentalReservationId"></param>
        /// <returns></returns>
        public string DeleteCarRental(long carRentalReservationId)
        {
            try
            {
                if (businessTrip.GetBusinessTripStatus(Convert.ToInt64(Session["LoadedBusinessTripId"])) != 2)
                {
                    return businessTrip.DeleteCarRentalReservation(carRentalReservationId) ? "true" : "false";
                }
                else
                {
                    return "false";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Load Flight Reservation view
        /// </summary>
        /// <returns></returns>
        public ActionResult FlightDialog(long flightReservationId)
        {
            try
            {
                DtoFlightReservation dtoFlightReservation = businessTrip.LoadFlight(flightReservationId);
                return View("~/Areas/YMMHRSystem/Views/BusinessTrip/FlightDialog.cshtml", dtoFlightReservation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Adds a Flight Reservation
        /// </summary>
        /// <param name="dtoFlightReservation"></param>
        /// <returns></returns>
        public ActionResult AddFlight(DtoFlightReservation dtoFlightReservation)
        {
            try
            {
                if (Session["LoadedBusinessTripId"] != null)
                {
                    if (businessTrip.GetBusinessTripStatus(Convert.ToInt64(Session["LoadedBusinessTripId"])) != 2)
                    {
                        dtoFlightReservation.BusinessTripId = Convert.ToInt64(Session["LoadedBusinessTripId"]);
                        businessTrip.AddFlightReservation(dtoFlightReservation);

                        return Json("true", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Removes a Flight Reservation. 
        /// </summary>
        /// <param name="flightReservationId"></param>
        /// <returns></returns>
        public string DeleteFlight(long flightReservationId)
        {
            try
            {
                if (businessTrip.GetBusinessTripStatus(Convert.ToInt64(Session["LoadedBusinessTripId"])) != 2)
                {
                    return businessTrip.DeleteFlightReservation(flightReservationId) ? "true" : "false";
                }
                else
                {
                    return "false";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Responds a Business Trip.
        /// </summary>
        /// <returns></returns>
        public ActionResult RespondBusinessTrip(DtoBusinessTrip dtoBusinessTrip)
        {
            try
            {
                dtoBusinessTrip.Status = 2;
                businessTrip.Save(dtoBusinessTrip);
                sendEmail.SendEmailTemplate("YMM HR System: Business Trip Response", "TemplateBusinessTripResponse", new[,]
{
                        {"$APPLICANT$", dtoBusinessTrip.UserCreated},
                        {"$PROCESS$", dtoBusinessTrip.Process},
                        {"$DATE$", dtoBusinessTrip.DateAdded.ToString("dd/MM/yyyy")}
                    }, sendEmail.GetAdminEmail(), emailNotification.GetBusinessTripContacts(1).Split(',').ToList());

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Creates Worker Template and downloads it.
        /// </summary>
        public void CreateDocument(long nMR4z)
        {
            DtoBusinessTrip dtoBusinessTrip = businessTrip.Load(nMR4z);

            string path = Server.MapPath("~/ExcelTemplates/") + "HTFForm.xlsx";
            FileInfo existingFile = new FileInfo(path);
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                //get the first worksheet in the workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                //GENERAL INFORMATION
                worksheet.Cells["B5"].Value = "Applied Date";
                worksheet.Cells["C5"].Value = "Applicator";
                worksheet.Cells["D5"].Value = "Department";
                worksheet.Cells["B6"].Value = dtoBusinessTrip.DateAdded.ToShortDateString();
                worksheet.Cells["C6"].Value = dtoBusinessTrip.Associate;
                worksheet.Cells["D6"].Value = dtoBusinessTrip.Process;
                var range = worksheet.Cells[5, 2, 5, 4];
                range.Style.Font.Bold = true;
                range = worksheet.Cells[5, 2, 6, 4];
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                //PASSENGER LIST
                worksheet.Cells["B8"].Value = "User Name";
                worksheet.Cells["C8"].Value = "Company Name";
                worksheet.Cells["D8"].Value = "Purpose of Business Trip";
                worksheet.Cells["E8"].Value = "Class";
                worksheet.Cells["F8"].Value = "Notes";
                worksheet.Cells[8, 6, 8, 8].Merge = true;
                range = worksheet.Cells[8, 2, 8, 8];
                range.Style.Font.Bold = true;
                int row = 9;
                foreach (var item in dtoBusinessTrip.PassengerList)
                {
                    worksheet.Cells[row, 2].Value = item.Passenger;
                    worksheet.Cells[row, 3].Value = item.CompanyName;
                    worksheet.Cells[row, 4].Value = item.Purpose;
                    worksheet.Cells[row, 5].Value = item.Type;
                    worksheet.Cells[row, 6].Value = item.Notes;
                    worksheet.Cells[row, 6].Style.WrapText = true;
                    worksheet.Cells[row, 6, row, 8].Merge = true;
                    row++;
                }
                row--;
                range = worksheet.Cells[8, 2, row, 8];
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                //TAXI LIST
                row += 2;
                worksheet.Cells[row, 2].Value = "Taxi";
                worksheet.Cells[row, 2].Style.Font.Bold = true;

                row++;
                int checkpointRow = row;
                worksheet.Cells[row, 2].Value = "Quote Number";
                worksheet.Cells[row, 3].Value = "Date";
                worksheet.Cells[row, 4].Value = "Time";
                worksheet.Cells[row, 5].Value = "Place";
                worksheet.Cells[row, 6].Value = "Destination";
                worksheet.Cells[row, 7].Value = "Cost Center";
                worksheet.Cells[row, 8].Value = "Driver Name";
                worksheet.Cells[row, 9].Value = "License Plate";
                worksheet.Cells[row, 10].Value = "Cost";
                range = worksheet.Cells[checkpointRow, 2, row, 10];
                range.Style.Font.Bold = true;
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                row++;
                checkpointRow = row;
                foreach (var item in dtoBusinessTrip.TaxiList)
                {
                    worksheet.Cells[row, 2].Value = item.QuoteNumber;
                    worksheet.Cells[row, 3].Value = item.PickupDate?.ToShortDateString();
                    worksheet.Cells[row, 4].Value = item.PickupTime.ToString(@"hh\:mm");
                    worksheet.Cells[row, 5].Value = item.PickupPlace;
                    worksheet.Cells[row, 6].Value = item.Destination;
                    worksheet.Cells[row, 7].Value = item.CostCenter;
                    worksheet.Cells[row, 8].Value = item.DriverName;
                    worksheet.Cells[row, 9].Value = item.LicensePlate;
                    worksheet.Cells[row, 10].Value = item.Cost;
                    worksheet.Cells[row, 10].Style.Numberformat.Format = "$ #,###,###.00";
                    row++;
                }
                row--;
                range = worksheet.Cells[checkpointRow, 2, row, 10];
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                //HOTEL LIST
                row += 2;
                worksheet.Cells[row, 2].Value = "Hotel";
                worksheet.Cells[row, 2].Style.Font.Bold = true;

                row++;
                checkpointRow = row;
                worksheet.Cells[row, 2].Value = "Hotel";
                worksheet.Cells[row, 3].Value = "Reservation No.";
                worksheet.Cells[row, 4].Value = "Check In";
                worksheet.Cells[row, 5].Value = "Check Out";
                worksheet.Cells[row, 6].Value = "Address";
                worksheet.Cells[row, 7].Value = "Telephone";
                worksheet.Cells[row, 8].Value = "Cost Center";
                worksheet.Cells[row, 9].Value = "Cost";
                worksheet.Cells[row, 10].Value = "Notes";
                worksheet.Cells[row, 10, row, 11].Merge = true;
                range = worksheet.Cells[checkpointRow, 2, row, 11];
                range.Style.Font.Bold = true;
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                row++;
                checkpointRow = row;
                foreach (var item in dtoBusinessTrip.HotelList)
                {
                    worksheet.Cells[row, 2].Value = item.HotelName;
                    worksheet.Cells[row, 3].Value = item.ReservationNumber;
                    worksheet.Cells[row, 4].Value = item.CheckIn?.ToShortDateString();
                    worksheet.Cells[row, 5].Value = item.CheckOut?.ToShortDateString();
                    worksheet.Cells[row, 6].Value = item.Address;
                    worksheet.Cells[row, 7].Value = item.Telephone;
                    worksheet.Cells[row, 8].Value = item.CostCenter;
                    worksheet.Cells[row, 9].Value = item.Cost;
                    worksheet.Cells[row, 10].Value = item.Notes;
                    worksheet.Cells[row, 9].Style.Numberformat.Format = "$ #,###,###.00";
                    worksheet.Cells[row, 10, row, 11].Merge = true;
                    worksheet.Cells[row, 10].Style.WrapText = true;
                    row++;
                }
                row--;
                range = worksheet.Cells[checkpointRow, 2, row, 11];
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                //FLIGHT LIST
                row += 2;
                worksheet.Cells[row, 2].Value = "Flight";
                worksheet.Cells[row, 2].Style.Font.Bold = true;

                row++;
                checkpointRow = row;
                worksheet.Cells[row, 2].Value = "Passenger";
                worksheet.Cells[row, 3].Value = "Passport No.";
                worksheet.Cells[row, 4].Value = "Flight No.";
                worksheet.Cells[row, 5].Value = "Departure Airport";
                worksheet.Cells[row, 6].Value = "Arrival Airport";
                worksheet.Cells[row, 7].Value = "Date";
                worksheet.Cells[row, 8].Value = "Time";
                worksheet.Cells[row, 9].Value = "Airline";
                worksheet.Cells[row, 10].Value = "Class";
                worksheet.Cells[row, 11].Value = "Cost Center";
                worksheet.Cells[row, 12].Value = "Cost";
                range = worksheet.Cells[checkpointRow, 2, row, 12];
                range.Style.Font.Bold = true;
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                row++;
                checkpointRow = row;
                foreach (var item in dtoBusinessTrip.FlightList)
                {
                    worksheet.Cells[row, 2].Value = item.PassengerName;
                    worksheet.Cells[row, 3].Value = item.PassportNumber;
                    worksheet.Cells[row, 4].Value = item.FlightNumber;
                    worksheet.Cells[row, 5].Value = item.DepartureAirport;
                    worksheet.Cells[row, 6].Value = item.ArrivalAirport;
                    worksheet.Cells[row, 7].Value = item.FlightDate?.ToShortDateString();
                    worksheet.Cells[row, 8].Value = item.FlightTime.ToString(@"hh\:mm");
                    worksheet.Cells[row, 9].Value = item.Airline;
                    worksheet.Cells[row, 10].Value = item.Class;
                    worksheet.Cells[row, 11].Value = item.CostCenter;
                    worksheet.Cells[row, 12].Value = item.Cost;
                    worksheet.Cells[row, 12].Style.Numberformat.Format = "$ #,###,###.00";
                    row++;
                }
                row--;
                range = worksheet.Cells[checkpointRow, 2, row, 12];
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                //CAR RENTAL LIST
                row += 2;
                worksheet.Cells[row, 2].Value = "Car Rental";
                worksheet.Cells[row, 2].Style.Font.Bold = true;

                row++;
                checkpointRow = row;
                worksheet.Cells[row, 2].Value = "Reservation No.";
                worksheet.Cells[row, 3].Value = "Car Type";
                worksheet.Cells[row, 4].Value = "Pickup Date";
                worksheet.Cells[row, 5].Value = "Pickup Time";
                worksheet.Cells[row, 6].Value = "Pickup Place";
                worksheet.Cells[row, 7].Value = "Return Date";
                worksheet.Cells[row, 8].Value = "Return Time";
                worksheet.Cells[row, 9].Value = "Return Place";
                worksheet.Cells[row, 10].Value = "Cost Center";
                worksheet.Cells[row, 11].Value = "Cost";
                range = worksheet.Cells[checkpointRow, 2, row, 11];
                range.Style.Font.Bold = true;
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                row++;
                checkpointRow = row;
                foreach (var item in dtoBusinessTrip.CarRentalList)
                {
                    worksheet.Cells[row, 2].Value = item.ReservationNumber;
                    worksheet.Cells[row, 3].Value = item.CarType;
                    worksheet.Cells[row, 4].Value = item.PickupDate?.ToShortDateString();
                    worksheet.Cells[row, 5].Value = item.PickupTime.ToString(@"hh\:mm");
                    worksheet.Cells[row, 6].Value = item.PickupPlace;
                    worksheet.Cells[row, 7].Value = item.ReturnDate?.ToShortDateString();
                    worksheet.Cells[row, 8].Value = item.ReturnTime.ToString(@"hh\:mm");
                    worksheet.Cells[row, 9].Value = item.ReturnPlace;
                    worksheet.Cells[row, 10].Value = item.CostCenter;
                    worksheet.Cells[row, 11].Value = item.Cost;
                    worksheet.Cells[row, 11].Style.Numberformat.Format = "$ #,###,###.00";
                    row++;
                }
                row--;
                range = worksheet.Cells[checkpointRow, 2, row, 11];
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                range = worksheet.Cells[2, 2, row, 12];
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.AutoFitColumns();
                //optional use this to make all columms just a bit wider, text would sometimes still overflow after AutoFitColumns().
                for (int col = 2; col <= worksheet.Dimension.End.Column; col++)
                {
                    worksheet.Column(col).Width = worksheet.Column(col).Width + 10;
                }
                //Save the workbook to disk in xlsx format

                //convert the excel package to a byte array
                byte[] bin = package.GetAsByteArray();
                //clear the buffer stream
                Response.ClearHeaders();
                Response.Clear();
                Response.Buffer = true;

                //set the correct contenttype
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                //set the correct length of the data being send
                Response.AddHeader("content-length", bin.Length.ToString());

                //set the filename for the excel package
                Response.AddHeader("content-disposition", "attachment; filename=\"BusinessTrip" + dtoBusinessTrip.DateAdded.ToString("yyyyMMdd") + "_" + dtoBusinessTrip.BusinessTripId + ".xlsx\"");

                //send the byte array to the browser
                Response.OutputStream.Write(bin, 0, bin.Length);

                //cleanup
                Response.Flush();
            }
        }
    }
}