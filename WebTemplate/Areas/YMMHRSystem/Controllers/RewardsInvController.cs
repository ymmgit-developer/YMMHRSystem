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
    public class RewardsInvController : Controller 
    {
        Rewards rewards = new Rewards();
        public ActionResult Index()
        {
            try 
            {
                if (Permission.QueryPermission("REWARDS.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanAddUpdateRecordsRewardsInv"] = Permission.QueryPermission("REWARDSINVENTORY.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    ViewBag.RewardsInv = rewards.GetRewardsItems();
                    return View();
                }
                else
                {
                    return View("~/Views/Shared/AccessDenied.cshtml");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult AddItemDialog()
        {
            try
            {
                return PartialView("~/Areas/YMMHRSystem/Views/RewardsInv/ItemAddDialog.cshtml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult SaveItem(DtoRewardsItem item)
        {
            try
            {
                var userId = long.Parse(HttpContext.Session["UserId"].ToString());

                // Validación mínima (no confíes solo en front)
                if (item == null)
                    return Json(new { success = false, message = "Invalid payload." });

                if (string.IsNullOrWhiteSpace(item.ItemName))
                    return Json(new { success = false, message = "Item name is required." });

                if (item.ValuePoints <= 0)
                    return Json(new { success = false, message = "Value points must be greater than 0." });

                if (item.Stock < 0 || item.StockMin < 0 || item.StockMax < 0)
                    return Json(new { success = false, message = "Stock values must be 0 or more." });

                if (item.StockMax != 0 && item.StockMax < item.StockMin)
                    return Json(new { success = false, message = "Stock Max must be 0 or >= Stock Min." });

                bool ok = rewards.SaveRewardsItem(item, userId);

                return Json(new { success = ok, message = ok ? "Item saved successfully." : "Could not save item." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }

        public ActionResult EditItemDialog(long ItemId)
        {
            try
            {
                var item = rewards.GetRewardsItembyId(ItemId);
                return PartialView("~/Areas/YMMHRSystem/Views/RewardsInv/ItemEditDialog.cshtml", item);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }

        public ActionResult SaveEditItem(DtoRewardsItem item)
        {
            try
            {
                if (item == null)
                    return Json(new { success = false, message = "Invalid payload." });

                if (item.ItemId <= 0)
                    return Json(new { success = false, message = "Invalid ItemId." });

                if (string.IsNullOrWhiteSpace(item.ItemName))
                    return Json(new { success = false, message = "Item name is required." });

                if (item.ValuePoints <= 0)
                    return Json(new { success = false, message = "Value points must be greater than 0." });

                if (item.Stock < 0 || item.StockMin < 0 || item.StockMax < 0)
                    return Json(new { success = false, message = "Stock values must be 0 or more." });

                if (item.StockMax != 0 && item.StockMax < item.StockMin)
                    return Json(new { success = false, message = "Stock Max must be 0 or >= Stock Min." });

                bool ok = rewards.UpdateRewardsItem(item);
                return Json(new { success = ok, message = ok ? "Item updated successfully." : "Could not update item." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }

        public ActionResult DeleteItem(long ItemId)
        {
            try
            {
                bool ok = rewards.DeleteRewardsItem(ItemId);
                return Json(new { success = ok, message = ok ? "Item deleted successfully." : "Could not delete item." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}