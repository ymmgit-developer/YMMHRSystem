using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class EntryExitAuthorizationController : Controller
    {
        EntryExitAuthorization checkinout = new EntryExitAuthorization();
        User user = new User();
        EmailNotification emailNotification = new EmailNotification();
        Motive motive = new Motive();
        Types types = new Types();
        WorkerFile workerFile = new WorkerFile();

        public ActionResult CheckInOut()
        {
            try 
            {
                HttpContext.Session["CanCreateChekInOut"] = Permission.QueryPermission("CHECKINOUT.CREATE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                HttpContext.Session["CanApproveChekInOut"] =
                Permission.QueryPermission("CHECKINOUT.FIRSTAPPROVAL", long.Parse(HttpContext.Session["UserId"].ToString())) ||
                Permission.QueryPermission("CHECKINOUT.SECONDAPPROVAL", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;


                if ((Role.QueryRole("HR", Convert.ToInt64(Session["UserId"])) || Role.QueryRole("Admin", Convert.ToInt64(Session["UserId"])) ) && !(Permission.QueryPermission("WATCHMEN", long.Parse(HttpContext.Session["UserId"].ToString()))) )
                {
                    return View("~/Areas/YMMHRSystem/Views/EntryExitAuthorization/CheckInOut.cshtml", checkinout.GetRecords(4));
                }
                else 
                {
                    if (Permission.QueryPermission("CHECKINOUT.FIRSTAPPROVAL", long.Parse(HttpContext.Session["UserId"].ToString())))
                    {
                        return View("~/Areas/YMMHRSystem/Views/EntryExitAuthorization/CheckInOut.cshtml", checkinout.GetRecords(0));
                    }
                    else
                    {
                        if (Permission.QueryPermission("CHECKINOUT.SECONDAPPROVAL", long.Parse(HttpContext.Session["UserId"].ToString())))
                        {
                            return View("~/Areas/YMMHRSystem/Views/EntryExitAuthorization/CheckInOut.cshtml", checkinout.GetRecords(1));
                        }
                        else
                        {
                            if (Permission.QueryPermission("CHECKINOUT.CREATE", long.Parse(HttpContext.Session["UserId"].ToString())))
                            {
                                return View("~/Areas/YMMHRSystem/Views/EntryExitAuthorization/CheckInOut.cshtml", checkinout.GetRecords(2, long.Parse(HttpContext.Session["UserId"].ToString())));
                            }
                            else
                            {
                                if (Permission.QueryPermission("WATCHMEN", long.Parse(HttpContext.Session["UserId"].ToString())))
                                {
                                    return View("~/Areas/YMMHRSystem/Views/EntryExitAuthorization/CheckInOut.cshtml", checkinout.GetRecords(3));
                                }
                                else
                                {
                                    return View("~/Views/Shared/AccessDenied.cshtml");
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult AddCheckInOut()
        {
            try
            {
                return PartialView("~/Areas/YMMHRSystem/Views/EntryExitAuthorization/CheckInOutAddDialog.cshtml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult SaveCheckInOut(DtoEntryExitAuthorization dtoEntryExitAuthorization)
        {
            try
            {
                if (dtoEntryExitAuthorization.WorkerList == null || !dtoEntryExitAuthorization.WorkerList.Any())
                {
                    return Json(new { success = false, message = "Please select an associate." }, JsonRequestBehavior.AllowGet);
                }

                var workers = dtoEntryExitAuthorization.WorkerList
                    .Where(worker => worker != null && !string.IsNullOrWhiteSpace(worker.Names))
                    .GroupBy(worker => new
                    {
                        Names = worker.Names.Trim().ToUpperInvariant(),
                        Process = (worker.Process ?? string.Empty).Trim().ToUpperInvariant()
                    })
                    .Select(group => group.First())
                    .ToList();

                if (!workers.Any())
                {
                    return Json(new { success = false, message = "Please select an associate." }, JsonRequestBehavior.AllowGet);
                }

                var entries = workers.Select(worker => new DtoEntryExitAuthorization
                {
                    Associate = worker.Names,
                    Process = worker.Process,
                    DateFor = dtoEntryExitAuthorization.DateFor,
                    TimeFor = dtoEntryExitAuthorization.TimeFor,
                    IdType = dtoEntryExitAuthorization.IdType,
                    IdSalary = dtoEntryExitAuthorization.IdSalary,
                    IdMotive = dtoEntryExitAuthorization.IdMotive,
                    CurrentState = dtoEntryExitAuthorization.CurrentState,
                    CreateBy = HttpContext.Session["UserName"].ToString(),
                    CreateDate = DateTime.Now
                }).ToList();

                foreach (var entry in entries)
                {
                    if (checkinout.Exists(entry))
                    {
                        return Json(new
                        {
                            success = false,
                            message = "An identical access request already exists for " + entry.Associate + "."
                        }, JsonRequestBehavior.AllowGet);
                    }
                }

                foreach (var entry in entries)
                {
                    if (!checkinout.Save(entry))
                    {
                        return Json(new { success = false, message = "The access request could not be saved." }, JsonRequestBehavior.AllowGet);
                    }

                    emailNotification.FirstApprovalNotification(
                        workerFile.GetWorkerFileIdByName(entry.Associate),
                        entry.Associate,
                        types.GetTypeDescription(entry.IdType),
                        motive.GetMotiveDescription(entry.IdMotive),
                        entry.DateFor,
                        entry.TimeFor.ToString());
                }

                return Json(new { success = true, message = "Successful registration." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "The access request could not be saved." }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateCheckInOut(DtoEntryExitAuthorization dtoEntryExitAuthorization) 
        {
            EntryExitAuthorization checkInOut = new EntryExitAuthorization();
            var entryExit = new DtoEntryExitAuthorization
            {
                IdRecordsInOut = dtoEntryExitAuthorization.IdRecordsInOut,
                Associate = dtoEntryExitAuthorization.Associate,
                Process = dtoEntryExitAuthorization.Process,
                DateFor = dtoEntryExitAuthorization.DateFor.AddMilliseconds(1),
                TimeFor = dtoEntryExitAuthorization.TimeFor,
                IdType = dtoEntryExitAuthorization.IdType,
                IdSalary = dtoEntryExitAuthorization.IdSalary,
                IdMotive = dtoEntryExitAuthorization.IdMotive,
                CurrentState = 0,
                CreateBy = dtoEntryExitAuthorization.CreateBy.ToString(),
                CreateDate = dtoEntryExitAuthorization.CreateDate
            };
            if (checkInOut.Update(entryExit))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DetailCheckInOut(int IdRecordsInOut)
        {
            try
            {
                DtoEntryExitAuthorization dtoEntryExitAuthorization = checkinout.DetailCheckInOut(IdRecordsInOut);
                return PartialView("~/Areas/YMMHRSystem/Views/EntryExitAuthorization/CheckInOutDetailDialog.cshtml", dtoEntryExitAuthorization);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult DeleteCheckInOut(int IdRecordsInOut)
        {
            try
            {
                EntryExitAuthorization EntryExitAuthorization = new EntryExitAuthorization();
                if (EntryExitAuthorization.Delete(IdRecordsInOut))
                {
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
        public ActionResult ApproveCheckInOut(int IdRecordsInOut, int approvalLevel)
        {
            try
            {
                EntryExitAuthorization entryExitAuthorization = new EntryExitAuthorization();

                long userId = Convert.ToInt64(HttpContext.Session["UserId"]);
                string userName = HttpContext.Session["UserName"]?.ToString();
                string workerFileName = HttpContext.Session["WorkerFileName"]?.ToString();

                var record = entryExitAuthorization.GetRecord(IdRecordsInOut);

                if (record == null)
                {
                    return Json(new { success = false, message = "Record not found." });
                }

                if (record.CurrentState == 2)
                {
                    return Json(new { success = false, message = "This record is already fully approved." });
                }

                if (string.Equals(record.Associate?.Trim(), workerFileName?.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return Json(new { success = false, message = "You cannot approve your own record." });
                }

                if (approvalLevel == 1)
                {
                    if (!Permission.QueryPermission("CHECKINOUT.FIRSTAPPROVAL", userId))
                    {
                        return Json(new { success = false, message = "You do not have permission for first approval." });
                    }

                    if (record.CurrentState != 0 || record.FirtsAuthorization == true)
                    {
                        return Json(new { success = false, message = "This record is no longer pending first approval." });
                    }

                    bool approved = entryExitAuthorization.ApproveRecords(1, userName, IdRecordsInOut);

                    if (approved)
                    {
                        entryExitAuthorization.ChangeCurrentStatus(1, IdRecordsInOut);
                        emailNotification.SecondApprovalNotification(IdRecordsInOut);

                        return Json(new { success = true, message = "First approval completed." });
                    }

                    return Json(new { success = false, message = "Error approving record." });
                }

                if (approvalLevel == 2)
                {
                    if (!Permission.QueryPermission("CHECKINOUT.SECONDAPPROVAL", userId))
                    {
                        return Json(new { success = false, message = "You do not have permission for second approval." });
                    }

                    if (record.CurrentState != 1 || record.FirtsAuthorization != true)
                    {
                        return Json(new { success = false, message = "This record is not pending second approval." });
                    }

                    bool approved = entryExitAuthorization.ApproveRecords(2, userName, IdRecordsInOut);

                    if (approved)
                    {
                        entryExitAuthorization.ChangeCurrentStatus(2, IdRecordsInOut);
                        emailNotification.CompleteApprovalNotification(IdRecordsInOut);

                        return Json(new { success = true, message = "Second approval completed." });
                    }

                    return Json(new { success = false, message = "Error approving record." });
                }

                return Json(new { success = false, message = "Invalid approval level." });
            }
            catch (Exception ex)
            {
                // Aquí conviene registrar el error real en log.
                return Json(new { success = false, message = "Unexpected error approving record." });
            }
        }
    }
}
