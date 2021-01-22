using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class RouteController : Controller
    {
        // GET: YMMHRSystem/Route
        Route route = new Route();

        // GET: YMMHRSystem/Route
        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("ROUTE.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveRoute"] = Permission.QueryPermission("ROUTE.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanDeleteRoute"] = Permission.QueryPermission("ROUTE.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View("~/Areas/YMMHRSystem/Views/Route/Index.cshtml", route.LoadMultiple());
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
        /// Load view with new/exisiting Route.
        /// </summary>
        /// <param name="nMR4z"></param>
        /// <returns></returns>
        public ActionResult LoadRoute(long nMR4z)
        {
            try
            {
                Session["LoadedRouteId"] = nMR4z;

                DtoRoute dtoRoute = route.Load(nMR4z);

                return View("~/Areas/YMMHRSystem/Views/Route/RouteDetail.cshtml", dtoRoute);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Route.
        /// </summary>
        /// <param name="dtoRoute"></param>
        /// <returns></returns>
        public ActionResult SaveRoute(DtoRoute dtoRoute)
        {
            DtoRoute dtoRoutes = new DtoRoute();

            try
            {

                dtoRoutes.RouteId = Convert.ToInt64(Session["LoadedRouteId"]);
                if (dtoRoutes.RouteId == 0)
                {
                    if (route.GetRouteId(dtoRoute.Name) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }

                dtoRoutes.Name = dtoRoute.Name;
                dtoRoutes.Shift = dtoRoute.Shift;
                dtoRoutes.Cost = dtoRoute.Cost;
                dtoRoutes.StopList = dtoRoute.StopList;
                dtoRoutes.Extraordinary = dtoRoute.Extraordinary;
                dtoRoutes.UserCreated = dtoRoute.UserCreated;
                dtoRoutes.DateAdded = dtoRoute.DateAdded;

                route.Save(dtoRoutes);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a Route. 
        /// </summary>
        /// <param name="RouteId"></param>
        /// <returns></returns>
        public string DeleteRoute(long RouteId)
        {
            try
            {
                return route.DeleteRoute(RouteId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Removes a Route Stop. 
        /// </summary>
        /// <param name="RouteId"></param>
        /// <returns></returns>
        public string DeleteRouteStop(long routeId, long stopId)
        {
            try
            {
                return route.DeleteRouteStop(routeId, stopId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Validates if Stop exists within Route Stop Table. 
        /// </summary>
        /// <param name="routeId"></param>
        /// <param name="stopId"></param>
        /// <returns>true or false</returns>
        public string ValidateRouteStopExistence(string stopId)
        {
            try
            {
                return route.ValidateRouteStopExistence(stopId) ? "true" : "false";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// Gets the stop's reference
        /// </summary>
        /// <param name="stopId"></param>
        /// <returns>Reference</returns>
        public ActionResult GetReference(long stopId)
        {
            try
            {
                Stop stop = new Stop();
                string reference = stop.GetReference(stopId);
               
                return Json(reference, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
    }
}