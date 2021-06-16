using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class PrizeAssociateController : Controller
    {

        PrizeAssociate prizeAssociate = new PrizeAssociate();

        // GET: YMMHRSystem/PrizeAssociate
        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("PRIZEASSOCIATE.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSavePrizeAssociate"] = Permission.QueryPermission("PRIZEASSOCIATE.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    HttpContext.Session["CanDeletePrizeAssociate"] = Permission.QueryPermission("PRIZEASSOCIATE.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View("~/Areas/YMMHRSystem/Views/PrizeAssociate/Index.cshtml", prizeAssociate.LoadMultiple());
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
        /// Saves a PrizeAssociate.
        /// </summary>
        /// <param name="dtoPrizeAssociate"></param>
        /// <returns></returns>
        public ActionResult SavePrizeAssociate(DtoPrizeAssociate dtoPrizeAssociate)
        {
            try
            {

                dtoPrizeAssociate.PrizeAssociateId = Convert.ToInt64(Session["LoadedPrizeAssociateId"]);
                if (dtoPrizeAssociate.PrizeAssociateId == 0)
                {
                    if (prizeAssociate.GetPrizeAssociateId(dtoPrizeAssociate.Associate) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }
                prizeAssociate.Save(dtoPrizeAssociate);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Load view with new/exisiting PrizeAssociate.
        /// </summary>
        /// <param name="nMR4z"></param>
        /// <returns></returns>
        public ActionResult LoadPrizeAssociate(long nMR4z)
        {
            try
            {
                Session["LoadedPrizeAssociateId"] = nMR4z;

                DtoPrizeAssociate dtoPrizeAssociate = prizeAssociate.Load(nMR4z);

                return View("~/Areas/YMMHRSystem/Views/PrizeAssociate/PrizeAssociateDetail.cshtml", dtoPrizeAssociate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Removes a PrizeAssociate. 
        /// </summary>
        /// <param name="prizeAssociateId"></param>
        /// <returns></returns>
        public string DeletePrizeAssociate(long prizeAssociateId)
        {
            try
            {
                return prizeAssociate.DeletePrizeAssociate(prizeAssociateId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportWorkers()
        {
            try
            {
                prizeAssociate.AddPrizeAssociates();
                prizeAssociate.DeleteInactivePrizeAssociates();
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Load attachment view.
        /// </summary>
        /// <returns></returns>
        public ActionResult AttendanceDialog()
        {
            try
            {
                return View("~/Areas/YMMHRSystem/Views/PrizeAssociate/AttendanceDialog.cshtml", new DtoPrizeAssociateAttendance());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Prize Associate Attendance.
        /// </summary>
        /// <param name="dtoAttendance"></param>
        /// <returns></returns>
        public ActionResult SaveAttendance(DtoPrizeAssociateAttendance dtoAttendance)
        {
            try
            {

                dtoAttendance.PrizeAssociateId = Convert.ToInt64(Session["LoadedPrizeAssociateId"]);

                if (dtoAttendance.PrizeAssociateAttendanceId == 0)
                {
                    if (prizeAssociate.GetPrizeAssociateAttendanceId(dtoAttendance.Month, dtoAttendance.Year, dtoAttendance.PrizeAssociateId) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }
                DtoPrizeAssociateConfiguration dtoPrizeAssociateConfiguration = prizeAssociate.LoadPointConfiguration(1);
                DtoPrizeAssociate associate = prizeAssociate.Load(dtoAttendance.PrizeAssociateId);

                dtoAttendance.Points = dtoPrizeAssociateConfiguration.MonthlyAttendance;

                prizeAssociate.SaveAttendance(dtoAttendance);
                prizeAssociate.SaveAssociatePoints(associate.AssociatePoints, dtoPrizeAssociateConfiguration.MonthlyAttendance, associate.PrizeAssociateId, true);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a Prize Associate Attendance. 
        /// </summary>
        /// <param name="prizeAssociateAttendanceId"></param>
        /// <returns></returns>
        public string DeletePrizeAssociateAttendance(long prizeAssociateAttendanceId)
        {
            try
            {
                DtoPrizeAssociateAttendance dtoPrizeAssociateAttendance = prizeAssociate.LoadPrizeAssociateAttendance(prizeAssociateAttendanceId);
                if (prizeAssociate.DeletePrizeAssociateAttendance(prizeAssociateAttendanceId))
                {
                    DtoPrizeAssociate associate = prizeAssociate.Load(Convert.ToInt64(Session["LoadedPrizeAssociateId"]));
                    prizeAssociate.SaveAssociatePoints(associate.AssociatePoints, dtoPrizeAssociateAttendance.Points, associate.PrizeAssociateId, false);
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
        /// Load trajectory view.
        /// </summary>
        /// <returns></returns>
        public ActionResult AssociateMonthDialog()
        {
            try
            {
                return View("~/Areas/YMMHRSystem/Views/PrizeAssociate/AssociateMonthDialog.cshtml", new DtoPrizeAssociateMonth());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Associate Month.
        /// </summary>
        /// <param name="dtoAssociateMonth"></param>
        /// <returns></returns>
        public ActionResult SaveAssociateMonth(DtoPrizeAssociateMonth dtoAssociateMonth)
        {
            try
            {

                dtoAssociateMonth.PrizeAssociateId = Convert.ToInt64(Session["LoadedPrizeAssociateId"]);

                if (dtoAssociateMonth.PrizeAssociateMonthId == 0)
                {
                    if (prizeAssociate.GetPrizeAssociateMonthId(dtoAssociateMonth.Month, dtoAssociateMonth.Year, dtoAssociateMonth.PrizeAssociateId) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }
                DtoPrizeAssociateConfiguration dtoPrizeAssociateConfiguration = prizeAssociate.LoadPointConfiguration(1);
                DtoPrizeAssociate associate = prizeAssociate.Load(dtoAssociateMonth.PrizeAssociateId);

                dtoAssociateMonth.Points = dtoPrizeAssociateConfiguration.AssociateoftheMonth;

                prizeAssociate.SaveAssociateMonth(dtoAssociateMonth);
                prizeAssociate.SaveAssociatePoints(associate.AssociatePoints, dtoPrizeAssociateConfiguration.AssociateoftheMonth, associate.PrizeAssociateId, true);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a Prize Associate Month. 
        /// </summary>
        /// <param name="prizeAssociateYearId"></param>
        /// <returns></returns>
        public string DeletePrizeAssociateMonth(long prizeAssociateMonthId)
        {
            try
            {
                DtoPrizeAssociateMonth dtoPrizeAssociateMonth = prizeAssociate.LoadPrizeAssociateMonth(prizeAssociateMonthId);
                if (prizeAssociate.DeletePrizeAssociateMonth(prizeAssociateMonthId))
                {
                    DtoPrizeAssociate associate = prizeAssociate.Load(Convert.ToInt64(Session["LoadedPrizeAssociateId"]));
                    prizeAssociate.SaveAssociatePoints(associate.AssociatePoints, dtoPrizeAssociateMonth.Points, associate.PrizeAssociateId, false);
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
        /// Load warning view
        /// </summary>
        /// <returns></returns>
        public ActionResult AssociateYearDialog()
        {
            try
            {
                return View("~/Areas/YMMHRSystem/Views/PrizeAssociate/AssociateYearDialog.cshtml", new DtoPrizeAssociateYear());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Associate Year.
        /// </summary>
        /// <param name="dtoAssociateYear"></param>
        /// <returns></returns>
        public ActionResult SaveAssociateYear(DtoPrizeAssociateYear dtoAssociateYear)
        {
            try
            {

                dtoAssociateYear.PrizeAssociateId = Convert.ToInt64(Session["LoadedPrizeAssociateId"]);

                if (dtoAssociateYear.PrizeAssociateYearId == 0)
                {
                    if (prizeAssociate.GetPrizeAssociateYearId(dtoAssociateYear.Year, dtoAssociateYear.PrizeAssociateId) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }
                DtoPrizeAssociateConfiguration dtoPrizeAssociateConfiguration = prizeAssociate.LoadPointConfiguration(1);
                DtoPrizeAssociate associate = prizeAssociate.Load(dtoAssociateYear.PrizeAssociateId);

                dtoAssociateYear.Points = dtoPrizeAssociateConfiguration.AssociateoftheYear;

                prizeAssociate.SaveAssociateYear(dtoAssociateYear);
                prizeAssociate.SaveAssociatePoints(associate.AssociatePoints, dtoPrizeAssociateConfiguration.AssociateoftheYear, associate.PrizeAssociateId, true);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a Prize Associate Year. 
        /// </summary>
        /// <param name="prizeAssociateYearId"></param>
        /// <returns></returns>
        public string DeletePrizeAssociateYear(long prizeAssociateYearId)
        {
            try
            {
                DtoPrizeAssociateYear dtoPrizeAsscoiateYear = prizeAssociate.LoadPrizeAssociateYear(prizeAssociateYearId);
                if (prizeAssociate.DeletePrizeAssociateYear(prizeAssociateYearId))
                {
                    DtoPrizeAssociate associate = prizeAssociate.Load(Convert.ToInt64(Session["LoadedPrizeAssociateId"]));
                    prizeAssociate.SaveAssociatePoints(associate.AssociatePoints, dtoPrizeAsscoiateYear.Points, associate.PrizeAssociateId, false);
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
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult FindingDialog()
        {
            try
            {
                return View("~/Areas/YMMHRSystem/Views/PrizeAssociate/FindingDialog.cshtml", new DtoPrizeAssociateFinding());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Associate Finding.
        /// </summary>
        /// <param name="dtoAssociateFinding"></param>
        /// <returns></returns>
        public ActionResult SaveAssociateFinding(DtoPrizeAssociateFinding dtoAssociateFinding)
        {
            try
            {
                FindingType findingType = new FindingType();
                dtoAssociateFinding.PrizeAssociateId = Convert.ToInt64(Session["LoadedPrizeAssociateId"]);

                if (dtoAssociateFinding.PrizeAssociateFindingId == 0)
                {
                    if (prizeAssociate.GetPrizeAssociateFindingId(dtoAssociateFinding.Month, dtoAssociateFinding.Year, dtoAssociateFinding.Finding, dtoAssociateFinding.PrizeAssociateId) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }

                DtoPrizeAssociate associate = prizeAssociate.Load(dtoAssociateFinding.PrizeAssociateId);
                dtoAssociateFinding.Points = findingType.GetFindingTypePoints(dtoAssociateFinding.Finding);

                prizeAssociate.SaveFinding(dtoAssociateFinding);
                prizeAssociate.SaveAssociatePoints(associate.AssociatePoints, dtoAssociateFinding.Points, associate.PrizeAssociateId, true);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a Prize Associate Finding. 
        /// </summary>
        /// <param name="prizeAssociateFindingId"></param>
        /// <returns></returns>
        public string DeletePrizeAssociateFinding(long prizeAssociateFindingId)
        {
            try
            {
                DtoPrizeAssociateFinding dtoPrizeAssociateFinding = prizeAssociate.LoadPrizeAssociateFinding(prizeAssociateFindingId);
                if (prizeAssociate.DeletePrizeAssociateFinding(prizeAssociateFindingId))
                {
                    DtoPrizeAssociate associate = prizeAssociate.Load(Convert.ToInt64(Session["LoadedPrizeAssociateId"]));
                    prizeAssociate.SaveAssociatePoints(associate.AssociatePoints, dtoPrizeAssociateFinding.Points, associate.PrizeAssociateId, false);
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
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult PrizeDialog()
        {
            try
            {
                return View("~/Areas/YMMHRSystem/Views/PrizeAssociate/PrizeDialog.cshtml", new DtoPrizeAssociatePrize());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Associate Prize.
        /// </summary>
        /// <param name="dtoAssociatePrize"></param>
        /// <returns></returns>
        public ActionResult SavePrizeAssociatePrize(DtoPrizeAssociatePrize dtoAssociatePrize)
        {
            try
            {
                Prize prize = new Prize();
                dtoAssociatePrize.PrizeAssociateId = Convert.ToInt64(Session["LoadedPrizeAssociateId"]);

                if (dtoAssociatePrize.PrizeAssociatePrizeId == 0)
                {
                    if (prizeAssociate.GetPrizeAssociatePrizeId(dtoAssociatePrize.Month, dtoAssociatePrize.Year, dtoAssociatePrize.Prize, dtoAssociatePrize.PrizeAssociateId) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }

                DtoPrizeAssociate associate = prizeAssociate.Load(dtoAssociatePrize.PrizeAssociateId);
                dtoAssociatePrize.Points = prize.GetPrizePoints(dtoAssociatePrize.PrizeId);

                DtoPrize dtoPrize = prize.Load(dtoAssociatePrize.PrizeId);
                dtoPrize.Quantity = dtoPrize.Quantity - 1;
                prize.Save(dtoPrize);

                DtoPrizeLog dtoPrizeLog = new DtoPrizeLog
                {
                    Associate = prizeAssociate.GetPrizeAssociateName(dtoAssociatePrize.PrizeAssociateId),
                    MovementType = "Exit",
                    PrizeId = dtoPrize.PrizeId,
                    Quantity = 1,
                    Date = DateTime.Now
                };

                prize.SavePrizeLog(dtoPrizeLog);
                dtoAssociatePrize.PrizeLogId = dtoPrizeLog.PrizeLogId;
                prizeAssociate.SavePrize(dtoAssociatePrize);
                prizeAssociate.SaveAssociatePoints(associate.AssociatePoints, dtoAssociatePrize.Points, associate.PrizeAssociateId, false);

                return Json("true", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a Prize Associate Prize. 
        /// </summary>
        /// <param name="prizeAssociatePrizeId"></param>
        /// <returns></returns>
        public string DeletePrizeAssociatePrize(long prizeAssociatePrizeId)
        {
            try
            {
                Prize prize = new Prize();
                DtoPrizeAssociatePrize dtoPrizeAssociatePrize = prizeAssociate.LoadPrizeAssociatePrize(prizeAssociatePrizeId);
                if (prize.DeletePrizeLog(dtoPrizeAssociatePrize.PrizeLogId))
                {
                    DtoPrizeAssociate associate = prizeAssociate.Load(Convert.ToInt64(Session["LoadedPrizeAssociateId"]));
                    prizeAssociate.SaveAssociatePoints(associate.AssociatePoints, dtoPrizeAssociatePrize.Points, associate.PrizeAssociateId, true);

                    DtoPrize dtoPrize = prize.Load(dtoPrizeAssociatePrize.PrizeId);
                    dtoPrize.Quantity = dtoPrize.Quantity + 1;
                    prize.Save(dtoPrize);
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
                return View("~/Areas/YMMHRSystem/Views/PrizeAssociate/PointConfigurationDialog.cshtml", prizeAssociate.LoadPointConfiguration(1));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves Point Configuration.
        /// </summary>
        /// <param name="dtoPrizeAssociateConfiguration"></param>
        /// <returns></returns>
        public ActionResult SavePointConfiguration(DtoPrizeAssociateConfiguration dtoPrizeAssociateConfiguration)
        {
            try
            {
                dtoPrizeAssociateConfiguration.PrizeAssociateConfigurationId = 1;
                prizeAssociate.SavePointConfiguration(dtoPrizeAssociateConfiguration);

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
        /// <param name="prizeId"></param>
        /// <returns></returns>
        public ActionResult PrizeAssociatePointLogDialog(long prizeAssociateId)
        {
            try
            {
                List<DtoPrizeAssociatePointLog> dtoPointLogList = prizeAssociate.LoadPrizeAssociatePointLogList(prizeAssociateId);
                return View("~/Areas/YMMHRSystem/Views/PrizeAssociate/PrizeAssociatePointLogList.cshtml", dtoPointLogList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prizeAssociateId"></param>
        /// <returns></returns>
        public ActionResult AdjustmentDialog()
        {
            try
            {
                ViewBag.ActualPoints = prizeAssociate.GetPrizeAssociatePoints(Convert.ToInt64(Session["LoadedPrizeAssociateId"]));
                return View("~/Areas/YMMHRSystem/Views/PrizeAssociate/AdjustmentDialog.cshtml", new DtoPrizeAssociatePointLog());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Registers a Prize Movement
        /// </summary>
        /// <param name="prizeId"></param>
        /// <param name="quantity"></param>
        /// <param name="movementType"></param>
        /// <returns></returns>
        public ActionResult SaveAdjustment(DtoPrizeAssociatePointLog dtoPrizeAssociatePointLog)
        {
            try
            {
                DtoPrizeAssociate dtoPrizeAssociate = prizeAssociate.Load(Convert.ToInt64(Session["LoadedPrizeAssociateId"]));
                if (dtoPrizeAssociatePointLog.MovementType == "Entry")
                {
                    dtoPrizeAssociate.AssociatePoints = dtoPrizeAssociatePointLog.Quantity + dtoPrizeAssociate.AssociatePoints;
                }
                else
                {
                    dtoPrizeAssociate.AssociatePoints = dtoPrizeAssociate.AssociatePoints - dtoPrizeAssociatePointLog.Quantity;
                }

                if (dtoPrizeAssociate.AssociatePoints >= 0)
                {
                    prizeAssociate.Save(dtoPrizeAssociate);
                    dtoPrizeAssociatePointLog.PrizeAssociateId = dtoPrizeAssociate.PrizeAssociateId;
                    dtoPrizeAssociatePointLog.Associate = prizeAssociate.GetPrizeAssociateName(dtoPrizeAssociate.PrizeAssociateId);
                    dtoPrizeAssociatePointLog.Date = DateTime.Now;
                    prizeAssociate.SavePrizeAssociatePointLog(dtoPrizeAssociatePointLog);


                    return Json("true", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("You can not execute this operation", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}