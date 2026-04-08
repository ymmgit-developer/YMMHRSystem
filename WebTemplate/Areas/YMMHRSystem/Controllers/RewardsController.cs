using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class RewardsController : Controller
    {
        Rewards rewards = new Rewards();
        WorkerFile workerFile = new WorkerFile();
        public ActionResult Index()
        {
            try 
            {
                if (Permission.QueryPermission("REWARDS.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanAddUpdateRecordsRewards"] = Permission.QueryPermission("REWARDS.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    ViewBag.ImprovementCredits = rewards.GetWorkerRewards();
                    return View();
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

        public ActionResult AddRecordsRewards(string[] SelectedWorkerIds)
        {
            try
            {
                ViewBag.Improvements = rewards.GetImprovementsbySelection(SelectedWorkerIds);
                ViewBag.Workers = workerFile.LoadMultipleWorkersbyWorkerId(SelectedWorkerIds);

                return PartialView("~/Areas/YMMHRSystem/Views/Rewards/ScoreAddDialog.cshtml");
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        public ActionResult SaveRegisterRecords(string[] TrustedWorkerIds, string[] AssociateWorkerIds, long[] ImprovementIdsTrusted, long[] ImprovementIdsAssociates, long[] ImprovementIdsContingencies) 
        {
            try 
            {
                long userId = long.Parse(HttpContext.Session["UserId"].ToString());

                rewards.ApplyScores(
                    TrustedWorkerIds ?? Array.Empty<string>(),
                    AssociateWorkerIds ?? Array.Empty<string>(),
                    ImprovementIdsTrusted ?? Array.Empty<long>(),
                    ImprovementIdsAssociates ?? Array.Empty<long>(),
                    ImprovementIdsContingencies ?? Array.Empty<long>(),
                    userId
                );

                return Json(new { success = true, message = "Scores applied successfully." });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetRecordsRewards(string WorkerId) 
        {
            try 
            {
                ViewBag.RewardsHistory = rewards.GetHistorybyWorkerId(WorkerId);

                return PartialView("~/Areas/YMMHRSystem/Views/Rewards/ScoreHistoryViewDialog.cshtml");
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        public ActionResult AddRedeemRewards(string WorkerId) 
        {
            try
            {
                var score = rewards.GetScoreByWorkerId(WorkerId);
                var items = rewards.GetRedeemableItems(score);

                ViewBag.WorkerId = WorkerId;
                ViewBag.Score = score;
                ViewBag.Items = items;

                return PartialView("~/Areas/YMMHRSystem/Views/Rewards/RedeemRewardsDialog.cshtml");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult SaveRedeemRewards(string WorkerId, long[] ItemIds, int[] Quantities)
        {
            try
            {
                var userId = long.Parse(HttpContext.Session["UserId"].ToString());

                var result = rewards.SaveRedeemRewards(WorkerId, ItemIds, Quantities, userId);

                return Json(new
                {
                    success = result.Success == 1,
                    message = result.Message,
                    redeemId = result.RedeemId,
                    totalPoints = result.TotalPoints,
                    newBalance = result.NewBalance
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }

        public ActionResult ResetPoints(string[] WorkerIds)
        {
            try
            {
                var userId = long.Parse(HttpContext.Session["UserId"].ToString());
                bool ok = rewards.ResetPoints(WorkerIds, userId);
                return Json(new { success = ok, message = ok ? "Points reset successfully." : "No workers were updated." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}