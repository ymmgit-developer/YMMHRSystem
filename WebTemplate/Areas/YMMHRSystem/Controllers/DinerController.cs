using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class DinerController : Controller
    {
        // GET: YMMHRSystem/Diner
        Diner diner = new Diner();

        // GET: YMMHRSystem/Diner
        public ActionResult Diner()
        {
            try
            {
                if (Permission.QueryPermission("DINER.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveDiner"] = Permission.QueryPermission("DINER.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanDeleteDiner"] = Permission.QueryPermission("DINER.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View("~/Areas/YMMHRSystem/Views/Diner/Diner.cshtml", diner.LoadMultiple());
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
        /// Load view with new/existing Diner.
        /// </summary>
        /// <param name="dinerId"></param>
        /// <returns></returns>
        public ActionResult LoadDiner(long dinerId)
        {
            try
            {
                Session["LoadedDinerId"] = dinerId;

                DtoDiner dtoDiner = diner.Load(dinerId);

                return View("~/Areas/YMMHRSystem/Views/Diner/DinerDetailDialog.cshtml", dtoDiner);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Diner.
        /// </summary>
        /// <param name="dtoDiner"></param>
        /// <returns></returns>
        public ActionResult SaveDiner(DtoDiner dtoDiner)
        {
            try
            {

                dtoDiner.DinerId = Convert.ToInt64(Session["LoadedDinerId"]);
                if (dtoDiner.DinerId == 0)
                {
                    if (diner.GetDinerId(dtoDiner.Type, dtoDiner.StartDate, dtoDiner.FinishDate) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }

                diner.Save(dtoDiner);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a Diner. 
        /// </summary>
        /// <param name="DinerId"></param>
        /// <returns></returns>
        public string DeleteDiner(long DinerId)
        {
            try
            {
                return diner.DeleteDiner(DinerId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: YMMHRSystem/ExtraordinaryDiner
        public ActionResult ExtraordinaryDiner()
        {
            try
            {
                if (Permission.QueryPermission("EXTRADINER.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveExtraordinaryDiner"] = Permission.QueryPermission("EXTRADINER.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanDeleteExtraordinaryDiner"] = Permission.QueryPermission("EXTRADINER.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View("~/Areas/YMMHRSystem/Views/Diner/ExtraordinaryDiner.cshtml", diner.LoadMultipleExtra());
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
        /// Load view with new/existing Diner.
        /// </summary>
        /// <param name="extraordinaryDinerId"></param>
        /// <returns></returns>
        public ActionResult LoadExtraordinaryDiner(long extraordinaryDinerId)
        {
            try
            {
                Session["LoadedExtraDinerId"] = extraordinaryDinerId;

                DtoExtraordinaryDiner dtoExtraordinaryDiner = diner.LoadExtra(extraordinaryDinerId);

                return View("~/Areas/YMMHRSystem/Views/Diner/ExtraordinaryDinerDetailDialog.cshtml", dtoExtraordinaryDiner);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Diner.
        /// </summary>
        /// <param name="dtoExtraDiner"></param>
        /// <returns></returns>
        public ActionResult SaveExtraordinaryDiner(DtoExtraordinaryDiner dtoExtraDiner)
        {
            try
            {
                DateTime? dateOrdered = dtoExtraDiner.Date?.Add(dtoExtraDiner.Time);
                if (dateOrdered?.Subtract(DateTime.Now).TotalHours < 4)
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }

                dtoExtraDiner.ExtraordinaryDinerId = Convert.ToInt64(Session["LoadedExtraDinerId"]);
                if (dtoExtraDiner.ExtraordinaryDinerId == 0)
                {
                    if (diner.GetExtraDinerId(dtoExtraDiner.AssociateName, dtoExtraDiner.Type, dtoExtraDiner.Date, dtoExtraDiner.Time) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }
                dtoExtraDiner.Lading = dtoExtraDiner.LadingCost > 0.0m ? true : false;
                diner.SaveExtra(dtoExtraDiner);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a Diner. 
        /// </summary>
        /// <param name="extraordinaryDinerId"></param>
        /// <returns></returns>
        public string DeleteExtraordinaryDiner(long extraordinaryDinerId)
        {
            try
            {
                return diner.DeleteExtraDiner(extraordinaryDinerId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
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