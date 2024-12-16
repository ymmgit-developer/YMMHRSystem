using DBFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMMHRSystemLogic
{
    public class WorkerFile
    {
        Log log = new Log();
        SQLTools oDatabase = new SQLTools();
        #region Standard Methods
        /// <summary>
        /// Loads the WorkerFile DTO
        /// </summary>
        /// <param name="workerFileId"></param>
        /// <returns>DtoWorkerFile Loaded</returns>
        public DtoWorkerFile Load(long workerFileId)
        {
            DtoWorkerFile workerFile = new DtoWorkerFile();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (workerFileId != 0)
                {
                    mapping.Load<DtoWorkerFile>("WorkerFiles", new DtoWorkerFile(), "WorkerFileId=" + workerFileId);
                    workerFile = (DtoWorkerFile)mapping.dtoList.FirstOrDefault().Dto;
                }

                workerFile.DismissalList = LoadMultipleDismissals(workerFile.WorkerFileId);
                workerFile.AdmissionList = LoadMultipleAdmissions(workerFile.WorkerFileId);
                workerFile.AttachmentList = LoadMultipleAttachments(workerFile.WorkerFileId);
                workerFile.TrajectoryList = LoadMultipleTrajectories(workerFile.WorkerFileId);
                workerFile.WarningList = LoadMultipleWarnings(workerFile.WorkerFileId);

                return workerFile;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load WorkerFile", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Register WorkerFile 
        /// </summary>
        /// <param name="workerFile"></param>
        /// <returns>WorkerFile registered</returns>
        public void Save(DtoWorkerFile workerFile)
        {
            try
            {
                User user = new User();
                DtoAdmission dtoAdmission = new DtoAdmission();
                if (workerFile.WorkerFileId == 0)
                {
                    workerFile.UserCreated = user.GetUserName(SQLTools.userId.ToString());
                    workerFile.DateAdded = DateTime.Now;
                    dtoAdmission.Admission = workerFile.AdmissionDate;
                }
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = workerFile, TableName = "WorkerFiles" });

                mapping.Save();

                if (dtoAdmission.Admission != null)
                {
                    dtoAdmission.WorkerFileId = workerFile.WorkerFileId;
                    AdmitWorker(dtoAdmission);
                }
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save WorkerFile", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple WorkerFile with fields.
        /// </summary>
        /// <returns>Load WorkerFile Dto</returns>
        public List<DtoWorkerFile> LoadMultiple(int status = 2)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoWorkerFile> workerfileList = new List<DtoWorkerFile>();
                switch (status)
                {
                    case 0:
                        mapping.Load<DtoWorkerFile>("SELECT WorkerFileId, Names, AdmissionDate, DismissalDate, Process, WorkerId, Status, Rehirable, DismissalNumber, AdmissionNumber, Gender, Route, Stop FROM WorkerFiles WHERE Status = 0 ORDER BY WorkerId * 1", "WorkerFiles", new DtoWorkerFile());
                        break;
                    case 1:
                        mapping.Load<DtoWorkerFile>("SELECT WorkerFileId, Names, AdmissionDate, DismissalDate, Process, WorkerId, Status, Rehirable, DismissalNumber, AdmissionNumber, Gender, Route, Stop FROM WorkerFiles WHERE Status = 1 ORDER BY WorkerId * 1", "WorkerFiles", new DtoWorkerFile());
                        break;
                    default:
                        mapping.Load<DtoWorkerFile>("SELECT WorkerFileId, Names, AdmissionDate, DismissalDate, Process, WorkerId, Status, Rehirable, DismissalNumber, AdmissionNumber, Gender, Route, Stop FROM WorkerFiles ORDER BY WorkerId * 1", "WorkerFiles", new DtoWorkerFile());
                        break;
                }
                workerfileList.AddRange(mapping.dtoList.Select(renglon => (DtoWorkerFile)renglon.Dto));

                return workerfileList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple WorkerFiles", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Union Workers with fields.
        /// </summary>
        /// <returns>Load WorkerFile Dto</returns>
        public List<DtoWorkerFile> LoadMultipleUnionWorkers(bool status)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoWorkerFile> workerfileList = new List<DtoWorkerFile>();

                mapping.Load<DtoWorkerFile>("SELECT WorkerFileId, Names, AdmissionDate, Process, WorkerId, Status, Rehirable, DismissalNumber, AdmissionNumber, Route, Stop FROM WorkerFiles WHERE Type <> 'Staff' AND Status = " + (status ? 1 : 0) + " ORDER BY WorkerId * 1", "WorkerFiles", new DtoWorkerFile());
                workerfileList.AddRange(mapping.dtoList.Select(renglon => (DtoWorkerFile)renglon.Dto));

                return workerfileList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple WorkerFiles", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }
        }
        /// <summary>
        /// Saves multiple workers
        /// </summary>
        /// <param name="workerFiles"></param>
        public void SaveMultiple(List<DtoWorkerFile> workerFiles)
        {
            try
            {
                User user = new User();
                string userCreated = user.GetUserName(SQLTools.userId.ToString());
                List<DtoAdmission> admissionsList = new List<DtoAdmission>();


                DBFrameworkMapping mapping = new DBFrameworkMapping();
                workerFiles.ForEach(item =>
                {
                    item.DateAdded = DateTime.Now;
                    item.Status = true;
                    item.UserCreated = userCreated;
                    mapping.dtoList.Add(new DBFrameworkDto() { Dto = item, TableName = "WorkerFiles" });
                });

                mapping.Save();

                mapping = new DBFrameworkMapping();

                foreach (var item in workerFiles)
                {
                    admissionsList.Add(new DtoAdmission()
                    {
                        WorkerFileId = item.WorkerFileId,
                        Admission = item.AdmissionDate
                    });
                }

                admissionsList.ForEach(item =>
                {
                    mapping.dtoList.Add(new DBFrameworkDto() { Dto = item, TableName = "Admissions" });
                });

                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Multiple Worker File", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveMultiple");
                throw ex;
            }
        }
        #endregion

        #region General Methods    
        /// <summary>
        /// Get WorkerFile Id
        /// </summary>
        /// <param name="workerId"></param>
        /// <returns></returns>
        public string GetWorkerFileName(long workerId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT Names FROM WorkerFiles WHERE WorkerId = " + workerId;

                dataRow = oDatabase.GetRow(sqlString, "Get WorkerFile Names");

                if (dataRow == null) return "";

                return dataRow["Names"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get WorkerFile Names", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetWorkerFileName");
                throw ex;
            }

        }
        /// <summary>
        /// Get WorkerFile Id
        /// </summary>
        /// <param name="workerId"></param>
        /// <returns></returns>
        public long GetWorkerFileId(string workerId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT WorkerFileId FROM WorkerFiles WHERE WorkerId = '" + workerId + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get WorkerFileId");

                if (dataRow == null) return 0;

                return Convert.ToInt64(dataRow["WorkerFileId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get WorkerFile Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetWorkerFileId");
                throw ex;
            }

        }
        /// <summary>
        /// Get WorkerFile Id by name
        /// </summary>
        /// <param name="workerId"></param>
        /// <returns></returns>
        public string GetWorkerFileIdByName(string Names)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT WorkerFileId FROM WorkerFiles WHERE Names = '" + Names + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get GetWorkerFileIdByName");

                if (dataRow == null) return "";

                return dataRow["WorkerFileId"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get WorkerFile Id By Name", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetWorkerFileIdByName");
                throw ex;
            }

        }
        /// <summary>
        /// Get WorkerFile Route
        /// </summary>
        /// <param name="workerId"></param>
        /// <returns></returns>
        public string GetWorkerFileRoute(string names)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT Route FROM WorkerFiles WHERE Names = '" + names + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get WorkerFile Route");

                if (dataRow == null) return "";

                return dataRow["Route"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get WorkerFile Route", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetWorkerFileRoute");
                throw ex;
            }

        }
        /// <summary>
        /// Get WorkerFile Stop
        /// </summary>
        /// <param name="workerId"></param>
        /// <returns></returns>
        public string GetWorkerFileStop(string names)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT Stop FROM WorkerFiles WHERE Names = '" + names + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get WorkerFile Stop");

                if (dataRow == null) return "";

                return dataRow["Stop"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get WorkerFile Stop", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetWorkerFileStop");
                throw ex;
            }

        }
        /// <summary>
        /// Get Worker Photo File 
        /// </summary>
        /// <returns>File name</returns>
        public string GetWorkerPhotoFile(long workerFileId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT WorkerPhoto FROM WorkerFiles WHERE WorkerFileId = " + workerFileId;

                dataRow = oDatabase.GetRow(sqlString, "Get File Name");

                if (dataRow == null) return "";

                return dataRow["WorkerPhoto"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Worker Photo File", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetWorkerPhotoFile");
                throw ex;
            }
        }
        /// <summary>
        /// Dismisses a Worker. 
        /// </summary>
        /// <param name="dtoDismissal"></param>
        /// <returns></returns>
        public bool DismissWorker(DtoDismissal dtoDismissal)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = dtoDismissal, TableName = "Dismissals" });

                mapping.Save();

                ChangeWorkerStatus(false, dtoDismissal.Rehirable, dtoDismissal.WorkerFileId, dtoDismissal.Date);

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Dismiss Worker", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DismissWorker");
                return false;
            }

        }

        /// <summary>
        /// Dismisses a Worker (New Process) Ago,2024 
        /// </summary>
        /// <param name="WorkerFileId"></param>
        /// <returns>true, false</returns>
        public bool DismissWorkerWithoutWorkerFiles(int WorkerId) {
            try
            {
                DateTime date = DateTime.Now.Date;
                string query = "";
                query = "UPDATE WorkerFiles SET Status = 0, Rehirable = 0, DismissalNumber = " + (GetWorkerDismissals(WorkerId) + 1) + ", DismissalDate = '" + date.ToString("yyyyMMdd") + "' WHERE WorkerId = " + WorkerId;
                oDatabase.ExecuteNonQuery(query, "Worker dismiss made");
                return true;
            }
            catch (Exception ex) 
            {
                log.WriteToErrorLog("HR System", "Dismiss Worker Without Requiring Documentation", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DismissWorker");
                return false;
            }
        }

        /// <summary>
        /// Admit a Worker. 
        /// </summary>
        /// <param name="dtoAdmission"></param>
        /// <returns></returns>
        public bool AdmitWorker(DtoAdmission dtoAdmission)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = dtoAdmission, TableName = "Admissions" });

                mapping.Save();

                ChangeWorkerStatus(true, false, dtoAdmission.WorkerFileId, dtoAdmission.Admission);

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Admit Worker", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "AdmitWorker");
                return false;
            }

        }
        /// <summary>
        /// Adds an attachment 
        /// </summary>
        /// <param name="dtoWorkerAttachment"></param>
        /// <returns></returns>
        public bool AddAttachment(DtoWorkerAttachment dtoWorkerAttachment)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = dtoWorkerAttachment, TableName = "WorkerAttachments" });

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
        /// Adds a trajectory
        /// </summary>
        /// <param name="dtoTrajectory"></param>
        /// <returns></returns>
        public bool AddTrajectory(DtoTrajectory dtoTrajectory)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = dtoTrajectory, TableName = "Trajectories" });

                mapping.Save();

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Add Trajectory", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "AddTrajectory");
                return false;
            }

        }
        /// <summary>
        /// Adds a trajectory
        /// </summary>
        /// <param name="dtoWarning"></param>
        /// <returns></returns>
        public bool AddWarning(DtoWarning dtoWarning)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = dtoWarning, TableName = "Warnings" });

                mapping.Save();

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Add Warning", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "AddWarning");
                return false;
            }

        }

        /// <summary>
        /// Changes worker status and rehirable fields
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="isRehirable"></param>
        /// <param name="workerFileId"></param>
        public void ChangeWorkerStatus(bool isActive, bool isRehirable, long workerFileId, DateTime? date = null)
        {
            try
            {
                string query = "";
                if (isActive)
                {
                    query = "UPDATE WorkerFiles SET AdmissionNumber = " + (GetWorkerAdmissions(workerFileId) + 1) + " WHERE WorkerFileId=" + workerFileId;
                    oDatabase.ExecuteNonQuery(query, "Increment Admission Number");

                    if (date != null)
                    {
                        query = "UPDATE WorkerFiles SET AdmissionDate = '" + date?.ToString("yyyyMMdd") + "' WHERE WorkerFileId = " + workerFileId;
                        oDatabase.ExecuteNonQuery(query, "Updates worker Admission Date");
                    }
                }
                else
                {
                    query = "UPDATE WorkerFiles SET DismissalNumber = " + (GetWorkerDismissals(workerFileId) + 1) + " WHERE WorkerFileId=" + workerFileId;
                    oDatabase.ExecuteNonQuery(query, "Increment Dismissal Number");

                    query = "UPDATE WorkerFiles SET DismissalDate = '" + date?.ToString("yyyyMMdd") + "' WHERE WorkerFileId = " + workerFileId;
                    oDatabase.ExecuteNonQuery(query, "Updates worker Dismissal Date");
                }

                query = "UPDATE WorkerFiles SET Status = " + (isActive ? 1 : 0) + ", Rehirable = " + (isRehirable ? 1 : 0) + " WHERE WorkerFileId=" + workerFileId;
                oDatabase.ExecuteNonQuery(query, "Worker status update");

            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Change Worker Status", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "ChangeWorkerStatus");
                throw ex;
            }
        }
        /// <summary>
        /// Gets Worker Id
        /// </summary>
        /// <returns>ID</returns>
        public string GetWorkerId(long workerFileId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT WorkerId FROM WorkerFiles WHERE WorkerFileId = '" + workerFileId + "'";

                dataRow = oDatabase.GetRow(sqlString, "Get WorkerFileId");

                if (dataRow == null) return "";

                return dataRow["WorkerId"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Worker Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetWorkerId");
                throw ex;
            }
        }
        /// <summary>
        /// Get Worker Dismissal File 
        /// </summary>
        /// <returns>File name</returns>
        public string GetWorkerDismissalFile(long dismissalId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT FileName FROM Dismissals WHERE DismissalId = " + dismissalId;

                dataRow = oDatabase.GetRow(sqlString, "Get File Name");

                if (dataRow == null) return "";

                return dataRow["FileName"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Worker Dismissal File", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetWorkerDismissalFile");
                throw ex;
            }
        }
        /// <summary>
        /// Get Worker Trajectory File 
        /// </summary>
        /// <returns>File name</returns>
        public string GetWorkerTrajectoryFile(long trajectoryId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT FileName FROM Trajectories WHERE TrajectoryId = " + trajectoryId;

                dataRow = oDatabase.GetRow(sqlString, "Get File Name");

                if (dataRow == null) return "";

                return dataRow["FileName"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Worker Trajectory File", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetWorkerTrajectoryFile");
                throw ex;
            }
        }
        /// <summary>
        /// Get Worker Attachment File 
        /// </summary>
        /// <returns>File name</returns>
        public string GetWorkerAttachmentFile(long workerAttachmentId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT FileName FROM WorkerAttachments WHERE WorkerAttachmentId = " + workerAttachmentId;

                dataRow = oDatabase.GetRow(sqlString, "Get File Name");

                if (dataRow == null) return "";

                return dataRow["FileName"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Worker Attachment File", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetWorkerAttachmentFile");
                throw ex;
            }
        }
        /// <summary>
        /// Get Worker Warning File 
        /// </summary>
        /// <returns>File name</returns>
        public string GetWorkerWarningFile(long warningId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT FileName FROM Warnings WHERE WarningId = " + warningId;

                dataRow = oDatabase.GetRow(sqlString, "Get File Name");

                if (dataRow == null) return "";

                return dataRow["FileName"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Worker Warning File", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetWorkerWarningFile");
                throw ex;
            }
        }
        /// <summary>
        /// Gets Worker admission number
        /// </summary>
        /// <returns>Number</returns>
        public int GetWorkerAdmissions(long workerFileId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT AdmissionNumber FROM WorkerFiles WHERE WorkerFileId = " + workerFileId;

                dataRow = oDatabase.GetRow(sqlString, "Get Admissions");

                if (dataRow == null) return 0;

                return Convert.ToInt32(dataRow[0]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Worker Admissions", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetWorkerAdmissions");
                throw ex;
            }
        }
        /// <summary>
        /// Gets Worker dismissal number
        /// </summary>
        /// <returns>Number</returns>
        public int GetWorkerDismissals(long workerFileId)
        {
            try
            {
                DataRow dataRow = null;
                string sqlString = "SELECT DismissalNumber FROM WorkerFiles WHERE WorkerFileId = " + workerFileId;

                dataRow = oDatabase.GetRow(sqlString, "Get Dismissals");

                if (dataRow == null) return 0;

                return Convert.ToInt32(dataRow[0]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Worker Dismissals", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetWorkerDismissals");
                throw ex;
            }
        }

        /// <summary>
        /// Load multiple WorkerFile with fields.
        /// </summary>
        /// <returns>Load WorkerFile Dto</returns>
        public List<DtoDismissal> LoadMultipleDismissals(long workerFileId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoDismissal> dismissalList = new List<DtoDismissal>();

                mapping.Load<DtoDismissal>("SELECT DismissalId, WorkerFileId, FileName, Motive, Date, Type, Rehirable FROM Dismissals WHERE WorkerFileId = " + workerFileId + " ORDER BY Date", "Dismissals", new DtoDismissal());
                dismissalList.AddRange(mapping.dtoList.Select(renglon => (DtoDismissal)renglon.Dto));

                return dismissalList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Dismissals", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleDismissals");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple WorkerFile with fields.
        /// </summary>
        /// <returns>Load WorkerFile Dto</returns>
        public List<DtoAdmission> LoadMultipleAdmissions(long workerFileId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoAdmission> admissionList = new List<DtoAdmission>();

                mapping.Load<DtoAdmission>("SELECT AdmissionId, WorkerFileId, Admission FROM Admissions WHERE WorkerFileId = " + workerFileId + " ORDER BY Admission", "Admissions", new DtoAdmission());
                admissionList.AddRange(mapping.dtoList.Select(renglon => (DtoAdmission)renglon.Dto));

                return admissionList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Admissions", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleAdmissions");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple trajectories
        /// </summary>
        /// <param name="workerFileId"></param>
        /// <returns></returns>
        public List<DtoTrajectory> LoadMultipleTrajectories(long workerFileId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoTrajectory> trajectoryList = new List<DtoTrajectory>();

                mapping.Load<DtoTrajectory>("SELECT TrajectoryId, WorkerFileId, FileName FROM Trajectories WHERE WorkerFileId = " + workerFileId + " ORDER BY TrajectoryId", "Trajectories", new DtoTrajectory());
                trajectoryList.AddRange(mapping.dtoList.Select(renglon => (DtoTrajectory)renglon.Dto));

                return trajectoryList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Trajectories", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleTrajectories");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple Attachments
        /// </summary>
        /// <param name="workerFileId"></param>
        /// <returns></returns>
        public List<DtoWorkerAttachment> LoadMultipleAttachments(long workerFileId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoWorkerAttachment> attachmentList = new List<DtoWorkerAttachment>();

                mapping.Load<DtoWorkerAttachment>("SELECT WorkerAttachmentId, WorkerFileId, FileName FROM WorkerAttachments WHERE WorkerFileId = " + workerFileId + " ORDER BY WorkerAttachmentId", "WorkerAttachments", new DtoWorkerAttachment());
                attachmentList.AddRange(mapping.dtoList.Select(renglon => (DtoWorkerAttachment)renglon.Dto));

                return attachmentList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Worker Attachment", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleAttachments");
                throw ex;
            }
        }
        /// <summary>
        /// Load multiple warnings
        /// </summary>
        /// <param name="workerFileId"></param>
        /// <returns></returns>
        public List<DtoWarning> LoadMultipleWarnings(long workerFileId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoWarning> warningList = new List<DtoWarning>();

                mapping.Load<DtoWarning>("SELECT WarningId, WorkerFileId, FileName, Motive, Date FROM Warnings WHERE WorkerFileId = " + workerFileId + " ORDER BY WarningId", "Warnings", new DtoWarning());
                warningList.AddRange(mapping.dtoList.Select(renglon => (DtoWarning)renglon.Dto));

                return warningList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Warnings", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultipleWarnings");
                throw ex;
            }
        }

        /// <summary>
        /// Deletes a Worker. 
        /// </summary>
        /// <param name="workerFileId"></param>
        /// <returns></returns>
        public bool DeleteWorkerFile(long workerFileId, string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                string query = "DELETE WorkerFiles WHERE WorkerFileId =" + workerFileId;
                oDatabase.ExecuteNonQuery(query, "Remove Worker File");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Worker File", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteWorkerFile");
                return false;
            }
        }
        /// <summary>
        /// Deletes a Dismissal. 
        /// </summary>
        /// <param name="dismissalId"></param>
        /// <returns></returns>
        public bool DeleteDismissal(long dismissalId, long workerFileId, string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                string query = "DELETE Dismissals WHERE DismissalId =" + dismissalId;
                oDatabase.ExecuteNonQuery(query, "Remove Dismissal");

                ChangeWorkerStatus(true, false, workerFileId);

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Dismissal", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteDismissal");
                return false;
            }
        }
        /// <summary>
        /// Deletes a Trajectory. 
        /// </summary>
        /// <param name="trajectoryId"></param>
        /// <returns></returns>
        public bool DeleteTrajectory(long trajectoryId, string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                string query = "DELETE Trajectories WHERE TrajectoryId =" + trajectoryId;
                oDatabase.ExecuteNonQuery(query, "Remove Trajectory");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Trajectory", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteTrajectory");
                return false;
            }
        }
        /// <summary>
        /// Deletes a Warning. 
        /// </summary>
        /// <param name="warningId"></param>
        /// <returns></returns>
        public bool DeleteWarning(long warningId, string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                string query = "DELETE Warnings WHERE WarningId =" + warningId;
                oDatabase.ExecuteNonQuery(query, "Remove Warning");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Warning", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteWarning");
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

                string query = "DELETE WorkerAttachments WHERE WorkerAttachmentId =" + attachmentId;
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
        /// Deletes a photo. 
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <returns></returns>
        public bool DeleteWorkerPhoto(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                return true;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Worker Photo", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteWorkerPhoto");
                return false;
            }
        }
        /// <summary>
        /// Get First half of year
        /// </summary>
        public string SetKiFirstHalfYear()
        {
            if (DateTime.Now.Month >= 4 && DateTime.Now.Month <= 12)
            {
                return DateTime.Now.Year.ToString();
            }
            else
            {
                return DateTime.Now.AddYears(-1).Year.ToString();
            }
        }
        #endregion
    }
}
