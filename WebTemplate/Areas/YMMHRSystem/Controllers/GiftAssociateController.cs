using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class GiftAssociateController : Controller
    {

        GiftAssociate giftAssociate = new GiftAssociate();

        // GET: YMMHRSystem/GiftAssociate
        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("GIFTASSOCIATE.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveGiftAssociate"] = Permission.QueryPermission("GIFTASSOCIATE.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    HttpContext.Session["CanDeleteGiftAssociate"] = Permission.QueryPermission("GIFTASSOCIATE.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View("~/Areas/YMMHRSystem/Views/GiftAssociate/Index.cshtml", giftAssociate.LoadMultiple());
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
        /// Saves a GiftAssociate.
        /// </summary>
        /// <param name="dtoGiftLog"></param>
        /// <returns></returns>
        public ActionResult SaveAssociateGift(DtoGiftLog dtoGiftLog)
        {
            try
            {
                Gift gift = new Gift();

                if (dtoGiftLog.GiftLogId == 0)
                {
                    if (gift.GetGiftLogId(dtoGiftLog.Gift, Session["Associate"].ToString(), DateTime.Now) != 0)
                    {
                        return Json("Operation already exists", JsonRequestBehavior.AllowGet);
                    }
                }

                DtoGift dtoGift = gift.Load(dtoGiftLog.GiftId);
                dtoGift.Quantity = dtoGift.Quantity - 1;

                if (dtoGift.Quantity >= 0)
                {
                    gift.Save(dtoGift);

                    dtoGiftLog.Associate = Session["Associate"].ToString();
                    dtoGiftLog.MovementType = "Exit";
                    dtoGiftLog.Quantity = 1;
                    dtoGiftLog.Date = DateTime.Now;

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
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a GiftAssociate. 
        /// </summary>
        /// <param name="giftAssociateId"></param>
        /// <returns></returns>
        public string DeleteGiftAssociate(long giftAssociateId)
        {
            try
            {
                return giftAssociate.DeleteGiftAssociate(giftAssociateId) ? "true" : "false";
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
                giftAssociate.AddGiftAssociates();
                giftAssociate.DeleteInactiveGiftAssociates();
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult OpenGiftDialog(long giftAssociateId, string associate)
        {
            try
            {
                Session["LoadedGiftAssociateId"] = giftAssociateId;
                Session["Associate"] = associate;
                return View("~/Areas/YMMHRSystem/Views/GiftAssociate/GiftDialog.cshtml", new DtoGiftLog());
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
        public ActionResult OpenGiftLogDialog(string associate)
        {
            try
            {
                Gift gift = new Gift();
                List<DtoGiftLog> dtoGiftLogList = gift.LoadMultipleGiftLogByAssociate(associate);

                return View("~/Areas/YMMHRSystem/Views/GiftAssociate/GiftAssociateLogList.cshtml", dtoGiftLogList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}