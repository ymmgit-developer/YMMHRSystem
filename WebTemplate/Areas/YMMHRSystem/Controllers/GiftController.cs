using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class GiftController : Controller
    {
        // GET: YMMHRSystem/Gift
        Gift gift = new Gift();

        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("GIFT.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveGift"] = Permission.QueryPermission("GIFT.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanDeleteGift"] = Permission.QueryPermission("GIFT.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View("~/Areas/YMMHRSystem/Views/Gift/Index.cshtml", gift.LoadMultiple());
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
        /// Load view with new/exisiting Gift.
        /// </summary>
        /// <param name="giftId"></param>
        /// <returns></returns>
        public ActionResult LoadGift(long giftId)
        {
            try
            {
                Session["LoadedGiftId"] = giftId;

                DtoGift dtoGift = gift.Load(giftId);

                return View("~/Areas/YMMHRSystem/Views/Gift/GiftDetailDialog.cshtml", dtoGift);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="giftId"></param>
        /// <returns></returns>
        public ActionResult LoadGiftLog(long giftId)
        {
            try
            {
                DtoGift dtoGift = gift.Load(giftId);
                ViewBag.Gift = dtoGift.Name;

                return View("~/Areas/YMMHRSystem/Views/Gift/GiftLogList.cshtml", dtoGift.GiftLogList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Gift.
        /// </summary>
        /// <param name="dtoGift"></param>
        /// <returns></returns>
        public ActionResult SaveGift(DtoGift dtoGift)
        {
            try
            {

                dtoGift.GiftId = Convert.ToInt64(Session["LoadedGiftId"]);
                if (dtoGift.GiftId == 0)
                {
                    if (gift.GetGiftId(dtoGift.Name) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }

                dtoGift.Status = true;
                gift.Save(dtoGift);

                DtoGiftLog dtoGiftLog = new DtoGiftLog
                {
                    Associate = Session["UserName"].ToString(),
                    Gift = dtoGift.Name,
                    MovementType = "Entry",
                    GiftId = dtoGift.GiftId,
                    Quantity = dtoGift.Quantity,
                    Date = DateTime.Now
                };

                gift.SaveGiftLog(dtoGiftLog);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a Gift. 
        /// </summary>
        /// <param name="giftId"></param>
        /// <returns></returns>
        public string DeleteGift(long giftId)
        {
            try
            {
                return gift.DeleteGift(giftId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Registers a Gift Movement
        /// </summary>
        /// <param name="giftId"></param>
        /// <param name="quantity"></param>
        /// <param name="movementType"></param>
        /// <returns></returns>
        public ActionResult GiftMovement(long giftId, int quantity, string movementType)
        {
            try
            {
                DtoGift dtoGift = gift.Load(giftId);
                if (movementType == "Entry")
                {
                    dtoGift.Quantity = quantity + dtoGift.Quantity;
                }
                else
                {
                    dtoGift.Quantity = dtoGift.Quantity - quantity;
                }

                if (dtoGift.Quantity >= 0)
                {
                    gift.Save(dtoGift);

                    DtoGiftLog dtoGiftLog = new DtoGiftLog
                    {
                        Associate = Session["UserName"].ToString(),
                        MovementType = movementType,
                        Gift = dtoGift.Name,
                        GiftId = dtoGift.GiftId,
                        Quantity = quantity,
                        Date = DateTime.Now
                    };

                    gift.SaveGiftLog(dtoGiftLog);

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