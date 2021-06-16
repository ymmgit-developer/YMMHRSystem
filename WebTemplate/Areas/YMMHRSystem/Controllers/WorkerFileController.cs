using OfficeOpenXml;
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
    public class WorkerFileController : Controller
    {
        WorkerFile workerFile = new WorkerFile();

        // GET: YMMHRSystem/WorkerFile
        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("WORKERFILE.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveWorkerFile"] = Permission.QueryPermission("WORKERFILE.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    HttpContext.Session["CanDismissWorkerFile"] = Permission.QueryPermission("WORKERFILE.DISMISS", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    HttpContext.Session["CanRehireWorkerFile"] = Permission.QueryPermission("WORKERFILE.REHIRE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    HttpContext.Session["CanDeleteWorkerFile"] = Permission.QueryPermission("WORKERFILE.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View("~/Areas/YMMHRSystem/Views/WorkerFile/Index.cshtml", workerFile.LoadMultiple());
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
        /// Saves a WorkerFile.
        /// </summary>
        /// <param name="dtoWorkerFile"></param>
        /// <returns></returns>
        public ActionResult SaveWorkerFile(DtoWorkerFile dtoWorkerFile)
        {
            try
            {

                dtoWorkerFile.WorkerFileId = Convert.ToInt64(Session["LoadedWorkerFileId"]);
                if (dtoWorkerFile.WorkerFileId == 0)
                {
                    if (workerFile.GetWorkerFileId(dtoWorkerFile.WorkerId) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }

                workerFile.Save(dtoWorkerFile);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Load view with new/exisiting WorkerFile.
        /// </summary>
        /// <param name="nMR4z"></param>
        /// <returns></returns>
        public ActionResult LoadWorkerFile(long nMR4z)
        {
            try
            {
                Session["LoadedWorkerFileId"] = nMR4z;

                DtoWorkerFile dtoWorkerFile = workerFile.Load(nMR4z);

                return View("~/Areas/YMMHRSystem/Views/WorkerFile/FileDetail.cshtml", dtoWorkerFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Removes a WorkerFile. 
        /// </summary>
        /// <param name="workerFileId"></param>
        /// <returns></returns>
        public string DeleteWorkerFile(long workerFileId)
        {
            try
            {
                string fileName = workerFile.GetWorkerPhotoFile(workerFileId);
                return workerFile.DeleteWorkerFile(workerFileId, fileName) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Load view with new/exisiting WorkerFile.
        /// </summary>
        /// <param name="workerFileId"></param>
        /// <returns></returns>
        public ActionResult DismissWorkerFile(long workerFileId)
        {
            try
            {
                Session["LoadedWorkerFileId"] = workerFileId;

                return View("~/Areas/YMMHRSystem/Views/WorkerFile/DismissalDialog.cshtml", new DtoDismissal());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Removes a WorkerFile. 
        /// </summary>
        /// <param name="WorkerFileId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DismissWorker(DtoDismissal dtoDismissal, HttpPostedFileBase file)
        {
            try
            {
                dtoDismissal.WorkerFileId = Convert.ToInt64(Session["LoadedWorkerFileId"]);

                if (file != null)
                {
                    string checkextension = Path.GetExtension(file.FileName).ToLower();
                    if (checkextension == ".pdf")
                    {
                        string path = Server.MapPath("~/WorkerFiles/Dismissals/" + workerFile.GetWorkerId(dtoDismissal.WorkerFileId) + "/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        dtoDismissal.FileName = path + file.FileName;
                        file.SaveAs(path + file.FileName);
                    }
                    else
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }

                }
                return workerFile.DismissWorker(dtoDismissal) ? Json("true", JsonRequestBehavior.AllowGet) : Json("false", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Load view with new/exisiting WorkerFile.
        /// </summary>
        /// <param name="workerFileId"></param>
        /// <returns></returns>
        public ActionResult AdmitWorkerFile(long workerFileId)
        {
            try
            {
                Session["LoadedWorkerFileId"] = workerFileId;

                return View("~/Areas/YMMHRSystem/Views/WorkerFile/AdmissionDialog.cshtml", new DtoAdmission());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Admits a Worker. 
        /// </summary>
        /// <param name="WorkerFileId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AdmitWorker(DtoAdmission dtoAdmission)
        {
            try
            {
                dtoAdmission.WorkerFileId = Convert.ToInt64(Session["LoadedWorkerFileId"]);
                return workerFile.AdmitWorker(dtoAdmission) ? Json("true", JsonRequestBehavior.AllowGet) : Json("false", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Removes a dismissal file. 
        /// </summary>
        /// <param name="dismissalId"></param>
        /// <returns></returns>
        public string DeleteDismissalFile(long dismissalId)
        {
            try
            {
                string fileName = workerFile.GetWorkerDismissalFile(dismissalId);
                return workerFile.DeleteDismissal(dismissalId, Convert.ToInt64(Session["LoadedWorkerFileId"]), fileName) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Removes a trajectory file. 
        /// </summary>
        /// <param name="trajectoryId"></param>
        /// <returns></returns>
        public string DeleteTrajectoryFile(long trajectoryId)
        {
            try
            {
                string fileName = workerFile.GetWorkerTrajectoryFile(trajectoryId);
                return workerFile.DeleteTrajectory(trajectoryId, fileName) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Removes a warning file. 
        /// </summary>
        /// <param name="warningId"></param>
        /// <returns></returns>
        public string DeleteWarningFile(long warningId)
        {
            try
            {
                string fileName = workerFile.GetWorkerWarningFile(warningId);
                return workerFile.DeleteWarning(warningId, fileName) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Removes a attachment file. 
        /// </summary>
        /// <param name="workerAttachmentId"></param>
        /// <returns></returns>
        public string DeleteAttachmentFile(long workerAttachmentId)
        {
            try
            {
                string fileName = workerFile.GetWorkerAttachmentFile(workerAttachmentId);
                return workerFile.DeleteAttachment(workerAttachmentId, fileName) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets dismissal file
        /// </summary>
        /// <param name="dismissalId"></param>
        /// <returns>file</returns>
        public ActionResult GetDismissalPDF(long dismissalId)
        {
            try
            {
                string file = workerFile.GetWorkerDismissalFile(dismissalId);
                return File(file, "application/pdf");
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        /// <summary>
        /// Get attachment file
        /// </summary>
        /// <param name="workerAttachmentId"></param>
        /// <returns>file</returns>
        public ActionResult GetAttachmentPDF(long workerAttachmentId)
        {
            try
            {
                string file = workerFile.GetWorkerAttachmentFile(workerAttachmentId);
                return File(file, "application/pdf");
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        /// <summary>
        /// Get trajectory file
        /// </summary>
        /// <param name="trajectoryId"></param>
        /// <returns>file</returns>
        public ActionResult GetTrajectoryPDF(long trajectoryId)
        {
            try
            {
                string file = workerFile.GetWorkerTrajectoryFile(trajectoryId);
                return File(file, "application/pdf");
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        /// <summary>
        /// Get warning file
        /// </summary>
        /// <param name="warningId"></param>
        /// <returns>file</returns>
        public ActionResult GetWarningPDF(long warningId)
        {
            try
            {
                string file = workerFile.GetWorkerWarningFile(warningId);
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
                return View("~/Areas/YMMHRSystem/Views/WorkerFile/AttachmentDialog.cshtml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Adds a Worker Attachment. 
        /// </summary>
        /// <param name="WorkerFileId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddAttachment(HttpPostedFileBase file)
        {
            try
            {
                DtoWorkerAttachment dtoWorkerAttachment = new DtoWorkerAttachment();
                dtoWorkerAttachment.WorkerFileId = Convert.ToInt64(Session["LoadedWorkerFileId"]);

                if (file != null)
                {
                    string checkextension = Path.GetExtension(file.FileName).ToLower();
                    if (checkextension == ".pdf")
                    {
                        string path = Server.MapPath("~/WorkerFiles/Attachments/" + workerFile.GetWorkerId(dtoWorkerAttachment.WorkerFileId) + "/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        dtoWorkerAttachment.FileName = path + file.FileName;
                        file.SaveAs(path + file.FileName);
                    }
                    else
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }

                }
                return workerFile.AddAttachment(dtoWorkerAttachment) ? Json("true", JsonRequestBehavior.AllowGet) : Json("false", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Load trajectory view.
        /// </summary>
        /// <returns></returns>
        public ActionResult TrajectoryDialog()
        {
            try
            {
                return View("~/Areas/YMMHRSystem/Views/WorkerFile/TrajectoryDialog.cshtml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Adds a Worker Trajectory. 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddTrajectory(HttpPostedFileBase file)
        {
            try
            {
                DtoTrajectory dtoTrajectory = new DtoTrajectory();
                dtoTrajectory.WorkerFileId = Convert.ToInt64(Session["LoadedWorkerFileId"]);

                if (file != null)
                {
                    string checkextension = Path.GetExtension(file.FileName).ToLower();
                    if (checkextension == ".pdf")
                    {
                        string path = Server.MapPath("~/WorkerFiles/Trajectory/" + workerFile.GetWorkerId(dtoTrajectory.WorkerFileId) + "/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        dtoTrajectory.FileName = path + file.FileName;
                        file.SaveAs(path + file.FileName);
                    }
                    else
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }

                }
                return workerFile.AddTrajectory(dtoTrajectory) ? Json("true", JsonRequestBehavior.AllowGet) : Json("false", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Load warning view
        /// </summary>
        /// <returns></returns>
        public ActionResult WarningDialog()
        {
            try
            {
                return View("~/Areas/YMMHRSystem/Views/WorkerFile/WarningDialog.cshtml", new DtoWarning());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Adds a Warning
        /// </summary>
        /// <param name="dtoWarning"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddWarning(DtoWarning dtoWarning, HttpPostedFileBase file)
        {
            try
            {
                dtoWarning.WorkerFileId = Convert.ToInt64(Session["LoadedWorkerFileId"]);

                if (file != null)
                {
                    string checkextension = Path.GetExtension(file.FileName).ToLower();
                    if (checkextension == ".pdf")
                    {
                        string path = Server.MapPath("~/WorkerFiles/Warnings/" + workerFile.GetWorkerId(dtoWarning.WorkerFileId) + "/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        dtoWarning.FileName = path + file.FileName;
                        file.SaveAs(path + file.FileName);
                    }
                    else
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }

                }
                return workerFile.AddWarning(dtoWarning) ? Json("true", JsonRequestBehavior.AllowGet) : Json("false", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves Worker Photo
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public ActionResult SavePhoto(HttpPostedFileBase Url)
        {
            try
            {
                if (Url == null) return Content("false", "text/plain");

                long workerFileId = Convert.ToInt64(Session["LoadedWorkerFileId"]);
                string fileName = workerFile.GetWorkerPhotoFile(workerFileId);

                workerFile.DeleteWorkerPhoto(fileName);

                string file = Path.GetFileName(Url.FileName);
                string path = "";
                string virtualFolder, folder = "";

                virtualFolder = "~/WorkerFiles/WorkerPhotos/" + workerFile.GetWorkerId(workerFileId) + "/";
                folder = Server.MapPath("~/WorkerFiles/WorkerPhotos/" + workerFile.GetWorkerId(workerFileId) + "/");
                if (file != null) path = folder + file;


                //Verifys if the Image folder exists, if not it is created
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                Url.SaveAs(path);
                return Content(virtualFolder + file, "text/plain");
            }
            catch (Exception ex)
            {
                return Content("false", "text/plain");
            }
        }
        /// <summary>
        /// Creates Worker Template and downloads it.
        /// </summary>
        public void CreateDocument()
        {

            ExcelPackage excelPackage = new ExcelPackage();
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("WorkerTemplate");

            worksheet.Cells["A1"].Value = "YMMHRSystem";

            worksheet.Cells["A3"].Value = "Worker ID";
            worksheet.Cells["B3"].Value = "Names";
            worksheet.Cells["C3"].Value = "Admission Date";
            worksheet.Cells["D3"].Value = "SSN";
            worksheet.Cells["E3"].Value = "RFC";
            worksheet.Cells["F3"].Value = "CURP";
            worksheet.Cells["G3"].Value = "Job Title";
            worksheet.Cells["H3"].Value = "Process";
            worksheet.Cells["I3"].Value = "Date of Birth";
            worksheet.Cells["J3"].Value = "Email";
            worksheet.Cells["K3"].Value = "Telephone";
            worksheet.Cells["L3"].Value = "Civil Status";
            worksheet.Cells["M3"].Value = "Education Level";
            worksheet.Cells["N3"].Value = "Worker Type";

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            //Save the workbook to disk in xlsx format

            //convert the excel package to a byte array
            byte[] bin = excelPackage.GetAsByteArray();
            //clear the buffer stream
            Response.ClearHeaders();
            Response.Clear();
            Response.Buffer = true;

            //set the correct contenttype
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            //set the correct length of the data being send
            Response.AddHeader("content-length", bin.Length.ToString());

            //set the filename for the excel package
            Response.AddHeader("content-disposition", "attachment; filename=\"WorkerTemplate.xlsx\"");

            //send the byte array to the browser
            Response.OutputStream.Write(bin, 0, bin.Length);

            //cleanup
            Response.Flush();
        }
        /// <summary>
        /// Imports Workers from Excel
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportWorkers()
        {
           
            if (Request.Files.Count > 0)
            {
                try
                {
                    List<DtoWorkerFile> excelData = new List<DtoWorkerFile>();
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        var newName = fname.Split('.');
                        fname = newName[0] + "_" + DateTime.Now.Ticks.ToString() + "." + newName[1];
                        var uploadRootFolderInput = AppDomain.CurrentDomain.BaseDirectory + "\\ExcelUploads";
                        Directory.CreateDirectory(uploadRootFolderInput);
                        var directoryFullPathInput = uploadRootFolderInput;
                        fname = Path.Combine(directoryFullPathInput, fname);
                        file.SaveAs(fname);
                        string xlsFile = fname;
                        excelData = ReadExcel(fname);
                    }
                    if (excelData != null)
                    {
                        List<DtoWorkerFile> newWorkers = new List<DtoWorkerFile>();
                        List<DtoWorkerFile> actualWorkers = workerFile.LoadMultiple();
                        if (actualWorkers.Count > 0)
                        {
                            newWorkers = excelData.Where(x => !actualWorkers.Any(y => y.WorkerId == x.WorkerId)).ToList();
                        }
                        else
                        {
                           newWorkers = excelData;
                        }
                       
                        workerFile.SaveMultiple(newWorkers);

                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public List<DtoWorkerFile> ReadExcel(string filePath)
        {
            try
            {
                List<DtoWorkerFile> workers = new List<DtoWorkerFile>();
                FileInfo existingFile = new FileInfo(filePath);
                using (ExcelPackage package = new ExcelPackage(existingFile))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets["WorkerTemplate"];
                    int rowCount = worksheet.Dimension.End.Row;

                    if (worksheet.Cells[1, 1].Value.ToString().Trim() != "YMMHRSystem")
                    {
                        return null;
                    }

                    if (worksheet.Cells[4, 1].Value != null && worksheet.Cells[4, 2].Value != null && worksheet.Cells[4, 3].Value != null && worksheet.Cells[4, 4].Value != null && worksheet.Cells[4, 5].Value != null && worksheet.Cells[4, 6].Value != null && worksheet.Cells[4, 7].Value != null && worksheet.Cells[4, 8].Value != null && worksheet.Cells[4, 9].Value != null && worksheet.Cells[4, 10].Value != null && worksheet.Cells[4, 11].Value != null && worksheet.Cells[4, 12].Value != null && worksheet.Cells[4, 13].Value != null && worksheet.Cells[4, 14].Value != null)
                    {
                        for (int row = 4; row <= rowCount; row++)
                        {
                            if (worksheet.Cells[row, 1].Value != null && worksheet.Cells[row, 2].Value != null && worksheet.Cells[row, 3].Value != null && worksheet.Cells[row, 4].Value != null && worksheet.Cells[row, 5].Value != null && worksheet.Cells[row, 6].Value != null && worksheet.Cells[row, 7].Value != null && worksheet.Cells[row, 8].Value != null && worksheet.Cells[row, 9].Value != null && worksheet.Cells[row, 12].Value != null && worksheet.Cells[row, 13].Value != null && worksheet.Cells[row, 14].Value != null)
                            {
                                workers.Add(new DtoWorkerFile()
                                {
                                    WorkerId = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                    Names = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(worksheet.Cells[row, 2].Value.ToString().Trim().ToLower()),
                                    AdmissionDate = Convert.ToDateTime(worksheet.Cells[row, 3].Value.ToString().Trim()),
                                    NSS = worksheet.Cells[row, 4].Value.ToString().Trim(),
                                    RFC = worksheet.Cells[row, 5].Value.ToString().Trim(),
                                    CURP = worksheet.Cells[row, 6].Value.ToString().Trim(),
                                    Job = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(worksheet.Cells[row, 7].Value.ToString().Trim().ToLower()),
                                    Process = worksheet.Cells[row, 8].Value.ToString().Trim(),
                                    DoB = Convert.ToDateTime(worksheet.Cells[row, 9].Value.ToString().Trim()),
                                    Email = worksheet.Cells[row, 10].Value == null ? "" : worksheet.Cells[row, 10].Value.ToString().Trim(),
                                    Telephone = worksheet.Cells[row, 11].Value == null ? "" : worksheet.Cells[row, 11].Value.ToString().Trim(),
                                    CivilStatus = worksheet.Cells[row, 12].Value.ToString().Trim(),
                                    Education = worksheet.Cells[row, 13].Value.ToString().Trim(),
                                    Type = worksheet.Cells[row, 14].Value.ToString().Trim(),
                                });
                            }
                            else
                            {
                                row = rowCount;
                            }
                        }
                        return workers;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// Loads Route's stop list
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns></returns>
        public ActionResult LoadStops(long routeId)
        {
            try
            {
                Route route = new Route();
                var stopList = route.GetRouteStopList(routeId);

                return Json(stopList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}