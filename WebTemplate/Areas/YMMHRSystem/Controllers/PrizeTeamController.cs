using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class PrizeTeamController : Controller
    {
        PrizeTeam prizeTeam = new PrizeTeam();
        PrizeAssociate prizeAssociate = new PrizeAssociate();

        // GET: YMMHRSystem/PrizeTeam
        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("PRIZETEAM.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSavePrizeTeam"] = Permission.QueryPermission("PRIZETEAM.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    HttpContext.Session["CanDeletePrizeTeam"] = Permission.QueryPermission("PRIZETEAM.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View("~/Areas/YMMHRSystem/Views/PrizeTeam/Index.cshtml", prizeTeam.LoadMultiple());
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
        /// Saves a PrizeTeam.
        /// </summary>
        /// <param name="dtoPrizeTeam"></param>
        /// <returns></returns>
        public ActionResult SavePrizeTeam(DtoPrizeTeam dtoPrizeTeam)
        {
            try
            {
                bool isNew = true;
                dtoPrizeTeam.PrizeTeamId = Convert.ToInt64(Session["LoadedPrizeTeamId"]);
                if (dtoPrizeTeam.PrizeTeamId == 0)
                {
                    if (prizeTeam.GetPrizeTeamId(dtoPrizeTeam.Name) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    isNew = false;
                }

                dtoPrizeTeam.CreatedBy = Session["UserName"].ToString();
                prizeTeam.Save(dtoPrizeTeam);
                if (isNew)
                {
                    return Json(new { redirectToUrl = Url.Action("LoadPrizeTeam", "PrizeTeam", new { Area = "YMMHRSystem", nMR4z = dtoPrizeTeam.PrizeTeamId }) });
                }
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Saves a PrizeTeam.
        /// </summary>
        /// <param name="dtoPrizeTeam"></param>
        /// <returns></returns>
        public ActionResult SavePrizeTeamAssociate(DtoPrizeTeamAssociate dtoPrizeTeamAssociate)
        {
            try
            {
                dtoPrizeTeamAssociate.PrizeTeamId = Convert.ToInt64(Session["LoadedPrizeTeamId"]);
                DtoPrizeTeam dtoPrizeTeam = prizeTeam.Load(dtoPrizeTeamAssociate.PrizeTeamId); ;
                dtoPrizeTeamAssociate.PrizeAssociateId = prizeAssociate.GetPrizeAssociateId(dtoPrizeTeamAssociate.Associate);
                if (dtoPrizeTeamAssociate.PrizeAssociateId == 0)
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }
                if (prizeTeam.ValidatePrizeTeamAssociateRelation(dtoPrizeTeamAssociate.PrizeAssociateId, dtoPrizeTeamAssociate.PrizeTeamId))
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }

                prizeTeam.SavePrizeTeamAssociate(dtoPrizeTeamAssociate);
                prizeTeam.UpdateAssociateTeam(dtoPrizeTeamAssociate.PrizeAssociateId, dtoPrizeTeam.Name);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Removes a Prize Team Associate. 
        /// </summary>
        /// <param name="prizeTeamId"></param>
        /// <returns></returns>
        public string DeletePrizeTeamAssociate(long prizeTeamAssociateId)
        {
            try
            {
                long prizeAssociateId = prizeTeam.GetPrizeAssociateId(prizeTeamAssociateId);
                return prizeTeam.DeletePrizeTeamAssociate(prizeTeamAssociateId, prizeAssociateId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Load New Team Associate Dialog.
        /// </summary>
        /// <returns></returns>
        public ActionResult NewTeamAssociateDialog()
        {
            try
            {
                return View("~/Areas/YMMHRSystem/Views/PrizeTeam/NewTeamAssociateDialog.cshtml", new DtoPrizeTeamAssociate());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Load attachment view.
        /// </summary>
        /// <returns></returns>
        public ActionResult PrizeTeamDialog(long prizeTeamId)
        {
            try
            {
                Session["LoadedPrizeTeamId"] = prizeTeamId;

                DtoPrizeTeam dtoPrizeTeam = prizeTeam.Load(prizeTeamId);
                return View("~/Areas/YMMHRSystem/Views/PrizeTeam/NewTeamDialog.cshtml", dtoPrizeTeam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Load view with new/exisiting PrizeTeam.
        /// </summary>
        /// <param name="nMR4z"></param>
        /// <returns></returns>
        public ActionResult LoadPrizeTeam(long nMR4z)
        {
            try
            {
                Session["LoadedPrizeTeamId"] = nMR4z;

                DtoPrizeTeam dtoPrizeTeam = prizeTeam.Load(nMR4z);

                return View("~/Areas/YMMHRSystem/Views/PrizeTeam/PrizeTeamDetail.cshtml", dtoPrizeTeam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Removes a PrizeTeam. 
        /// </summary>
        /// <param name="prizeTeamId"></param>
        /// <returns></returns>
        public string DeletePrizeTeam(long prizeTeamId)
        {
            try
            {
                return prizeTeam.DeletePrizeTeam(prizeTeamId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Load attachment view.
        /// </summary>
        /// <returns></returns>
        public ActionResult PrizeTeamAttendanceDialog()
        {
            try
            {
                return View("~/Areas/YMMHRSystem/Views/PrizeTeam/AttendanceDialog.cshtml", new DtoPrizeTeamAttendance());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Prize Team Attendance.
        /// </summary>
        /// <param name="dtoAttendance"></param>
        /// <returns></returns>
        public ActionResult SavePrizeTeamAttendance(DtoPrizeTeamAttendance dtoAttendance)
        {
            try
            {
                dtoAttendance.PrizeTeamId = Convert.ToInt64(Session["LoadedPrizeTeamId"]);
                List<DtoPrizeTeamAssociate> associateList = prizeTeam.LoadMultipleTeamAssociates(dtoAttendance.PrizeTeamId);
                if (associateList.Count == 0)
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }

                if (dtoAttendance.PrizeTeamAttendanceId == 0)
                {
                    if (prizeTeam.GetPrizeTeamAttendanceId(dtoAttendance.Month, dtoAttendance.Year, dtoAttendance.PrizeTeamId) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }
                DtoPrizeTeamConfiguration dtoPrizeTeamConfiguration = prizeTeam.LoadPointConfiguration(1);
                DtoPrizeTeam team = prizeTeam.Load(dtoAttendance.PrizeTeamId);

                dtoAttendance.Points = dtoPrizeTeamConfiguration.MonthlyAttendance;

                prizeTeam.SavePrizeTeamAttendance(dtoAttendance);
                prizeTeam.SaveTeamPoints(team.TeamPoints, dtoPrizeTeamConfiguration.MonthlyAttendance, team.PrizeTeamId, true);
                prizeTeam.SaveMultipleAssociateTeamPoints(associateList, dtoPrizeTeamConfiguration.MonthlyAttendance, true);

                DtoPrizeTeamPointLog dtoPrizeTeamPointLog = new DtoPrizeTeamPointLog
                {
                    MovementType = "Entry",
                    PrizeTeamId = team.PrizeTeamId,
                    Quantity = dtoPrizeTeamConfiguration.MonthlyAttendance,
                    Date = DateTime.Now
                };

                prizeTeam.SavePrizeTeamPointLog(dtoPrizeTeamPointLog);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a Prize Team Attendance. 
        /// </summary>
        /// <param name="prizeTeamAttendanceId"></param>
        /// <returns></returns>
        public string DeletePrizeTeamAttendance(long prizeTeamAttendanceId)
        {
            try
            {
                DtoPrizeTeamAttendance dtoPrizeTeamAttendance = prizeTeam.LoadPrizeTeamAttendance(prizeTeamAttendanceId);
                List<DtoPrizeTeamAssociate> associateList = prizeTeam.LoadMultipleTeamAssociates(dtoPrizeTeamAttendance.PrizeTeamId);
                if (associateList.Count == 0)
                {
                    return "false";
                }
                if (prizeTeam.DeletePrizeTeamAttendance(prizeTeamAttendanceId))
                {
                    DtoPrizeTeam team = prizeTeam.Load(Convert.ToInt64(Session["LoadedPrizeTeamId"]));
                    prizeTeam.SaveTeamPoints(team.TeamPoints, dtoPrizeTeamAttendance.Points, team.PrizeTeamId, false);
                    prizeTeam.SaveMultipleAssociateTeamPoints(associateList, dtoPrizeTeamAttendance.Points, false);
                    DtoPrizeTeamPointLog dtoPrizeTeamPointLog = new DtoPrizeTeamPointLog
                    {
                        MovementType = "Exit",
                        PrizeTeamId = team.PrizeTeamId,
                        Quantity = dtoPrizeTeamAttendance.Points,
                        Date = DateTime.Now
                    };

                    prizeTeam.SavePrizeTeamPointLog(dtoPrizeTeamPointLog);


                    return "true";
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
        /// Load Prize Team Productivity Dialog.
        /// </summary>
        /// <returns></returns>
        public ActionResult PrizeTeamProductivityDialog()
        {
            try
            {
                return View("~/Areas/YMMHRSystem/Views/PrizeTeam/ProductivityDialog.cshtml", new DtoPrizeTeamProductivity());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Prize Team Productivity.
        /// </summary>
        /// <param name="dtoPrizeTeamProductivity"></param>
        /// <returns></returns>
        public ActionResult SavePrizeTeamProductivity(DtoPrizeTeamProductivity dtoPrizeTeamProductivity)
        {
            try
            {

                dtoPrizeTeamProductivity.PrizeTeamId = Convert.ToInt64(Session["LoadedPrizeTeamId"]);
                List<DtoPrizeTeamAssociate> associateList = prizeTeam.LoadMultipleTeamAssociates(dtoPrizeTeamProductivity.PrizeTeamId);
                if (associateList.Count == 0)
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }

                if (dtoPrizeTeamProductivity.PrizeTeamProductivityId == 0)
                {
                    if (prizeTeam.GetPrizeTeamProductivityId(dtoPrizeTeamProductivity.Month, dtoPrizeTeamProductivity.Year, dtoPrizeTeamProductivity.PrizeTeamId) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }
                DtoPrizeTeamConfiguration dtoPrizeTeamConfiguration = prizeTeam.LoadPointConfiguration(1);
                DtoPrizeTeam team = prizeTeam.Load(dtoPrizeTeamProductivity.PrizeTeamId);

                dtoPrizeTeamProductivity.Points = dtoPrizeTeamConfiguration.Productivity;

                prizeTeam.SavePrizeTeamProductivity(dtoPrizeTeamProductivity);
                prizeTeam.SaveTeamPoints(team.TeamPoints, dtoPrizeTeamConfiguration.Productivity, team.PrizeTeamId, true);
                prizeTeam.SaveMultipleAssociateTeamPoints(associateList, dtoPrizeTeamConfiguration.Productivity, true);
                DtoPrizeTeamPointLog dtoPrizeTeamPointLog = new DtoPrizeTeamPointLog
                {
                    MovementType = "Entry",
                    PrizeTeamId = team.PrizeTeamId,
                    Quantity = dtoPrizeTeamConfiguration.Productivity,
                    Date = DateTime.Now
                };

                prizeTeam.SavePrizeTeamPointLog(dtoPrizeTeamPointLog);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a Prize Team Productivity. 
        /// </summary>
        /// <param name="prizeTeamYearId"></param>
        /// <returns></returns>
        public string DeletePrizeTeamProductivity(long prizeTeamMonthId)
        {
            try
            {
                DtoPrizeTeamProductivity dtoPrizeTeamProductivity = prizeTeam.LoadPrizeTeamProductivity(prizeTeamMonthId);
                List<DtoPrizeTeamAssociate> associateList = prizeTeam.LoadMultipleTeamAssociates(dtoPrizeTeamProductivity.PrizeTeamId);
                if (associateList.Count == 0)
                {
                    return "false";
                }

                if (prizeTeam.DeletePrizeTeamProductivity(prizeTeamMonthId))
                {
                    DtoPrizeTeam team = prizeTeam.Load(Convert.ToInt64(Session["LoadedPrizeTeamId"]));
                    prizeTeam.SaveTeamPoints(team.TeamPoints, dtoPrizeTeamProductivity.Points, team.PrizeTeamId, false);
                    prizeTeam.SaveMultipleAssociateTeamPoints(associateList, dtoPrizeTeamProductivity.Points, false);
                    DtoPrizeTeamPointLog dtoPrizeTeamPointLog = new DtoPrizeTeamPointLog
                    {
                        MovementType = "Exit",
                        PrizeTeamId = team.PrizeTeamId,
                        Quantity = dtoPrizeTeamProductivity.Points,
                        Date = DateTime.Now
                    };

                    prizeTeam.SavePrizeTeamPointLog(dtoPrizeTeamPointLog);

                    return "true";
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
        /// Load Prize Team Scrap Dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult PrizeTeamScrapDialog()
        {
            try
            {
                return View("~/Areas/YMMHRSystem/Views/PrizeTeam/ScrapDialog.cshtml", new DtoPrizeTeamScrap());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Prize Team Scrap.
        /// </summary>
        /// <param name="dtoPrizeTeamScrap"></param>
        /// <returns></returns>
        public ActionResult SavePrizeTeamScrap(DtoPrizeTeamScrap dtoPrizeTeamScrap)
        {
            try
            {
                dtoPrizeTeamScrap.PrizeTeamId = Convert.ToInt64(Session["LoadedPrizeTeamId"]);
                List<DtoPrizeTeamAssociate> associateList = prizeTeam.LoadMultipleTeamAssociates(dtoPrizeTeamScrap.PrizeTeamId);
                if (associateList.Count == 0)
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }

                if (dtoPrizeTeamScrap.PrizeTeamScrapId == 0)
                {
                    if (prizeTeam.GetPrizeTeamScrapId(dtoPrizeTeamScrap.Month, dtoPrizeTeamScrap.Year, dtoPrizeTeamScrap.PrizeTeamId) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }
                DtoPrizeTeamConfiguration dtoPrizeTeamConfiguration = prizeTeam.LoadPointConfiguration(1);
                DtoPrizeTeam team = prizeTeam.Load(dtoPrizeTeamScrap.PrizeTeamId);

                dtoPrizeTeamScrap.Points = dtoPrizeTeamConfiguration.Scrap;

                prizeTeam.SavePrizeTeamScrap(dtoPrizeTeamScrap);
                prizeTeam.SaveTeamPoints(team.TeamPoints, dtoPrizeTeamConfiguration.Scrap, team.PrizeTeamId, true);
                prizeTeam.SaveMultipleAssociateTeamPoints(associateList, dtoPrizeTeamConfiguration.Scrap, true);
                DtoPrizeTeamPointLog dtoPrizeTeamPointLog = new DtoPrizeTeamPointLog
                {
                    MovementType = "Entry",
                    PrizeTeamId = team.PrizeTeamId,
                    Quantity = dtoPrizeTeamConfiguration.Scrap,
                    Date = DateTime.Now
                };

                prizeTeam.SavePrizeTeamPointLog(dtoPrizeTeamPointLog);


                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a Prize Team Scrap. 
        /// </summary>
        /// <param name="prizeTeamScrapId"></param>
        /// <returns></returns>
        public string DeletePrizeTeamScrap(long prizeTeamScrapId)
        {
            try
            {
                DtoPrizeTeamScrap dtoPrizeTeamScrap = prizeTeam.LoadPrizeTeamScrap(prizeTeamScrapId);
                List<DtoPrizeTeamAssociate> associateList = prizeTeam.LoadMultipleTeamAssociates(dtoPrizeTeamScrap.PrizeTeamId);
                if (associateList.Count == 0)
                {
                    return "false";
                }


                if (prizeTeam.DeletePrizeTeamScrap(prizeTeamScrapId))
                {
                    DtoPrizeTeam team = prizeTeam.Load(Convert.ToInt64(Session["LoadedPrizeTeamId"]));
                    prizeTeam.SaveTeamPoints(team.TeamPoints, dtoPrizeTeamScrap.Points, team.PrizeTeamId, false);
                    prizeTeam.SaveMultipleAssociateTeamPoints(associateList, dtoPrizeTeamScrap.Points, false);
                    DtoPrizeTeamPointLog dtoPrizeTeamPointLog = new DtoPrizeTeamPointLog
                    {
                        MovementType = "Exit",
                        PrizeTeamId = team.PrizeTeamId,
                        Quantity = dtoPrizeTeamScrap.Points,
                        Date = DateTime.Now
                    };

                    prizeTeam.SavePrizeTeamPointLog(dtoPrizeTeamPointLog);

                    return "true";
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
        /// Opens Prize Team Dock Audit Dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult PrizeTeamDockAuditDialog()
        {
            try
            {
                return View("~/Areas/YMMHRSystem/Views/PrizeTeam/DockAuditDialog.cshtml", new DtoPrizeTeamDockAudit());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Team DockAudit.
        /// </summary>
        /// <param name="dtoPrizeTeamDockAudit"></param>
        /// <returns></returns>
        public ActionResult SavePrizeTeamDockAudit(DtoPrizeTeamDockAudit dtoPrizeTeamDockAudit)
        {
            try
            {

                dtoPrizeTeamDockAudit.PrizeTeamId = Convert.ToInt64(Session["LoadedPrizeTeamId"]);
                List<DtoPrizeTeamAssociate> associateList = prizeTeam.LoadMultipleTeamAssociates(dtoPrizeTeamDockAudit.PrizeTeamId);
                if (associateList.Count == 0)
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }

                if (dtoPrizeTeamDockAudit.PrizeTeamDockAuditId == 0)
                {
                    if (prizeTeam.GetPrizeTeamDockAuditId(dtoPrizeTeamDockAudit.Month, dtoPrizeTeamDockAudit.Year, dtoPrizeTeamDockAudit.PrizeTeamId) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }
                DtoPrizeTeamConfiguration dtoPrizeTeamConfiguration = prizeTeam.LoadPointConfiguration(1);
                DtoPrizeTeam team = prizeTeam.Load(dtoPrizeTeamDockAudit.PrizeTeamId);

                dtoPrizeTeamDockAudit.Points = dtoPrizeTeamConfiguration.DockAudit;

                prizeTeam.SavePrizeTeamDockAudit(dtoPrizeTeamDockAudit);
                prizeTeam.SaveTeamPoints(team.TeamPoints, dtoPrizeTeamConfiguration.DockAudit, team.PrizeTeamId, true);
                prizeTeam.SaveMultipleAssociateTeamPoints(associateList, dtoPrizeTeamConfiguration.DockAudit, true);
                DtoPrizeTeamPointLog dtoPrizeTeamPointLog = new DtoPrizeTeamPointLog
                {
                    MovementType = "Entry",
                    PrizeTeamId = team.PrizeTeamId,
                    Quantity = dtoPrizeTeamConfiguration.DockAudit,
                    Date = DateTime.Now
                };

                prizeTeam.SavePrizeTeamPointLog(dtoPrizeTeamPointLog);


                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Removes a Prize Team DockAudit. 
        /// </summary>
        /// <param name="prizeTeamDockAuditId"></param>
        /// <returns></returns>
        public string DeletePrizeTeamDockAudit(long prizeTeamDockAuditId)
        {
            try
            {
                DtoPrizeTeamDockAudit dtoPrizeTeamDockAudit = prizeTeam.LoadPrizeTeamDockAudit(prizeTeamDockAuditId);
                List<DtoPrizeTeamAssociate> associateList = prizeTeam.LoadMultipleTeamAssociates(dtoPrizeTeamDockAudit.PrizeTeamId);
                if (associateList.Count == 0)
                {
                    return "false";
                }

                if (prizeTeam.DeletePrizeTeamDockAudit(prizeTeamDockAuditId))
                {
                    DtoPrizeTeam team = prizeTeam.Load(Convert.ToInt64(Session["LoadedPrizeTeamId"]));
                    prizeTeam.SaveTeamPoints(team.TeamPoints, dtoPrizeTeamDockAudit.Points, team.PrizeTeamId, false);
                    prizeTeam.SaveMultipleAssociateTeamPoints(associateList, dtoPrizeTeamDockAudit.Points, false);
                    DtoPrizeTeamPointLog dtoPrizeTeamPointLog = new DtoPrizeTeamPointLog
                    {
                        MovementType = "Exit",
                        PrizeTeamId = team.PrizeTeamId,
                        Quantity = dtoPrizeTeamDockAudit.Points,
                        Date = DateTime.Now
                    };

                    prizeTeam.SavePrizeTeamPointLog(dtoPrizeTeamPointLog);

                    return "true";
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
        /// Open Prize Team Security Cross Dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult PrizeTeamSecurityCrossDialog()
        {
            try
            {
                return View("~/Areas/YMMHRSystem/Views/PrizeTeam/SecurityCrossDialog.cshtml", new DtoPrizeTeamSecurityCross());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Team Security Cross.
        /// </summary>
        /// <param name="dtoTeamPrizeSecurityCross"></param>
        /// <returns></returns>
        public ActionResult SavePrizeTeamSecurityCross(DtoPrizeTeamSecurityCross dtoTeamPrizeSecurityCross)
        {
            try
            {

                dtoTeamPrizeSecurityCross.PrizeTeamId = Convert.ToInt64(Session["LoadedPrizeTeamId"]);
                List<DtoPrizeTeamAssociate> associateList = prizeTeam.LoadMultipleTeamAssociates(dtoTeamPrizeSecurityCross.PrizeTeamId);
                if (associateList.Count == 0)
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }

                if (dtoTeamPrizeSecurityCross.PrizeTeamSecurityCrossId == 0)
                {
                    if (prizeTeam.GetPrizeTeamSecurityCrossId(dtoTeamPrizeSecurityCross.Month, dtoTeamPrizeSecurityCross.Year, dtoTeamPrizeSecurityCross.PrizeTeamId) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }
                DtoPrizeTeamConfiguration dtoPrizeTeamConfiguration = prizeTeam.LoadPointConfiguration(1);
                DtoPrizeTeam team = prizeTeam.Load(dtoTeamPrizeSecurityCross.PrizeTeamId);

                dtoTeamPrizeSecurityCross.Points = dtoPrizeTeamConfiguration.SecurityCross;

                prizeTeam.SavePrizeTeamSecurityCross(dtoTeamPrizeSecurityCross);
                prizeTeam.SaveTeamPoints(team.TeamPoints, dtoPrizeTeamConfiguration.SecurityCross, team.PrizeTeamId, true);
                prizeTeam.SaveMultipleAssociateTeamPoints(associateList, dtoPrizeTeamConfiguration.SecurityCross, true);
                DtoPrizeTeamPointLog dtoPrizeTeamPointLog = new DtoPrizeTeamPointLog
                {
                    MovementType = "Entry",
                    PrizeTeamId = team.PrizeTeamId,
                    Quantity = dtoPrizeTeamConfiguration.SecurityCross,
                    Date = DateTime.Now
                };

                prizeTeam.SavePrizeTeamPointLog(dtoPrizeTeamPointLog);


                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Removes a Prize Team Security Cross. 
        /// </summary>
        /// <param name="prizeTeamSecurityCrossId"></param>
        /// <returns></returns>
        public string DeletePrizeTeamSecurityCross(long prizeTeamSecurityCrossId)
        {
            try
            {
                DtoPrizeTeamSecurityCross dtoPrizeTeamSecurityCross = prizeTeam.LoadPrizeTeamSecurityCross(prizeTeamSecurityCrossId);
                List<DtoPrizeTeamAssociate> associateList = prizeTeam.LoadMultipleTeamAssociates(dtoPrizeTeamSecurityCross.PrizeTeamId);
                if (associateList.Count == 0)
                {
                    return "false";
                }

                if (prizeTeam.DeletePrizeTeamSecurityCross(prizeTeamSecurityCrossId))
                {
                    DtoPrizeTeam team = prizeTeam.Load(Convert.ToInt64(Session["LoadedPrizeTeamId"]));
                    prizeTeam.SaveTeamPoints(team.TeamPoints, dtoPrizeTeamSecurityCross.Points, team.PrizeTeamId, false);
                    prizeTeam.SaveMultipleAssociateTeamPoints(associateList, dtoPrizeTeamSecurityCross.Points, false);
                    DtoPrizeTeamPointLog dtoPrizeTeamPointLog = new DtoPrizeTeamPointLog
                    {
                        MovementType = "Exit",
                        PrizeTeamId = team.PrizeTeamId,
                        Quantity = dtoPrizeTeamSecurityCross.Points,
                        Date = DateTime.Now
                    };

                    prizeTeam.SavePrizeTeamPointLog(dtoPrizeTeamPointLog);


                    return "true";
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
        /// Opens Point Configuration Dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult PointConfigurationDialog()
        {
            try
            {
                return View("~/Areas/YMMHRSystem/Views/PrizeTeam/PointConfigurationDialog.cshtml", prizeTeam.LoadPointConfiguration(1));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves Point Configuration.
        /// </summary>
        /// <param name="dtoPrizeTeamConfiguration"></param>
        /// <returns></returns>
        public ActionResult SavePointConfiguration(DtoPrizeTeamConfiguration dtoPrizeTeamConfiguration)
        {
            try
            {
                dtoPrizeTeamConfiguration.PrizeTeamConfigurationId = 1;
                prizeTeam.SavePointConfiguration(dtoPrizeTeamConfiguration);

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
        /// <param name="prizeTeamId"></param>
        /// <returns></returns>
        public ActionResult PrizeTeamPointLogDialog(long prizeTeamId)
        {
            try
            {
                List<DtoPrizeTeamPointLog> dtoPointLogList = prizeTeam.LoadPrizeTeamPointLogList(prizeTeamId);
                return View("~/Areas/YMMHRSystem/Views/PrizeTeam/PrizeTeamPointLogList.cshtml", dtoPointLogList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}