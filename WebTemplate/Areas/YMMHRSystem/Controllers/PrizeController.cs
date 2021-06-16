using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class PrizeController : Controller
    {
        // GET: YMMHRSystem/Prize
        Prize prize = new Prize();
        SendEmail sendEmail = new SendEmail();

        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("PRIZE.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSavePrize"] = Permission.QueryPermission("PRIZE.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanDeletePrize"] = Permission.QueryPermission("PRIZE.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View("~/Areas/YMMHRSystem/Views/Prize/Index.cshtml", prize.LoadMultiple());
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
        /// Load view with new/exisiting Prize.
        /// </summary>
        /// <param name="prizeId"></param>
        /// <returns></returns>
        public ActionResult LoadPrize(long prizeId)
        {
            try
            {
                Session["LoadedPrizeId"] = prizeId;

                DtoPrize dtoPrize = prize.Load(prizeId);

                return View("~/Areas/YMMHRSystem/Views/Prize/PrizeDetailDialog.cshtml", dtoPrize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prizeId"></param>
        /// <returns></returns>
        public ActionResult LoadPrizeLog(long prizeId)
        {
            try
            {
                DtoPrize dtoPrize = prize.Load(prizeId);
                ViewBag.Prize = dtoPrize.Name;

                return View("~/Areas/YMMHRSystem/Views/Prize/PrizeLogList.cshtml", dtoPrize.PrizeLogList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Prize.
        /// </summary>
        /// <param name="dtoPrize"></param>
        /// <returns></returns>
        public ActionResult SavePrize(DtoPrize dtoPrize)
        {
            try
            {

                dtoPrize.PrizeId = Convert.ToInt64(Session["LoadedPrizeId"]);
                if (dtoPrize.PrizeId == 0)
                {
                    if (prize.GetPrizeId(dtoPrize.Name) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }

                dtoPrize.Status = true;
                prize.Save(dtoPrize, Convert.ToInt64(Session["UserId"]));

                DtoPrizeLog dtoPrizeLog = new DtoPrizeLog
                {
                    Associate = Session["UserName"].ToString(),
                    MovementType = "Entry",
                    PrizeId = dtoPrize.PrizeId,
                    Quantity = dtoPrize.Quantity,
                    Date = DateTime.Now
                };

                prize.SavePrizeLog(dtoPrizeLog);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a Prize. 
        /// </summary>
        /// <param name="prizeId"></param>
        /// <returns></returns>
        public string DeletePrize(long prizeId)
        {
            try
            {
                return prize.DeletePrize(prizeId) ? "true" : "false";
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
        public ActionResult PrizeMovement(long prizeId, int quantity, string movementType)
        {
            try
            {
                DtoPrize dtoPrize = prize.Load(prizeId);
                if (movementType == "Entry")
                {
                    dtoPrize.Quantity = quantity + dtoPrize.Quantity;
                }
                else
                {
                    dtoPrize.Quantity = dtoPrize.Quantity - quantity;
                }

                if (dtoPrize.Quantity >= 0)
                {
                    prize.Save(dtoPrize);

                    DtoPrizeLog dtoPrizeLog = new DtoPrizeLog
                    {
                        Associate = Session["UserName"].ToString(),
                        MovementType = movementType,
                        PrizeId = dtoPrize.PrizeId,
                        Quantity = quantity,
                        Date = DateTime.Now
                    };

                    prize.SavePrizeLog(dtoPrizeLog);
                    try
                    {
                        if (dtoPrize.Quantity <= dtoPrize.Minimum)
                        {
                            User user = new User();
                            List<string> contacts = new List<string> { user.GetUserEmail(dtoPrize.CreatedBy) };
                            sendEmail.SendEmailTemplate("YMM HR System: Prize Minimum Stock Limit", "TemplatePrizeMinimumStock", new[,]
                            {
                                {"$PRIZE$", dtoPrize.Name},
                                {"$QTY$", dtoPrize.Quantity.ToString()},
                                {"$LIMIT$", dtoPrize.Minimum.ToString()}
                            }, sendEmail.GetAdminEmail(), contacts);
                        }
                    }
                    catch (Exception)
                    {

                    }


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