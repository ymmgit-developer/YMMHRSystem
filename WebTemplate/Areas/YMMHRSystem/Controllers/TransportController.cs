using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class TransportController : Controller
    {
        // GET: YMMHRSystem/Transport
        Transport transport = new Transport();

        // GET: YMMHRSystem/Transport
        public ActionResult Transport()
        {
            try
            {
                if (Permission.QueryPermission("TRANSPORT.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveTransport"] = Permission.QueryPermission("TRANSPORT.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanDeleteTransport"] = Permission.QueryPermission("TRANSPORT.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View("~/Areas/YMMHRSystem/Views/Transport/Transport.cshtml", transport.LoadMultiple());
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
        /// Load view with new/existing Transport.
        /// </summary>
        /// <param name="transportId"></param>
        /// <returns></returns>
        public ActionResult LoadTransport(long transportId)
        {
            try
            {
                Session["LoadedTransportId"] = transportId;

                DtoTransport dtoTransport = transport.Load(transportId);

                return View("~/Areas/YMMHRSystem/Views/Transport/TransportDetailDialog.cshtml", dtoTransport);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Transport.
        /// </summary>
        /// <param name="dtoTransport"></param>
        /// <returns></returns>
        public ActionResult SaveTransport(DtoTransport dtoTransport)
        {
            try
            {

                dtoTransport.TransportId = Convert.ToInt64(Session["LoadedTransportId"]);
                if (dtoTransport.TransportId == 0)
                {
                    if (transport.GetTransportId(dtoTransport.Route, dtoTransport.Shift, dtoTransport.StartDate, dtoTransport.FinishDate) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }

                int totalDays = (dtoTransport.FinishDate.AddDays(1) - dtoTransport.StartDate).Days;

                if (!dtoTransport.Type)
                {
                    dtoTransport.Cost = (dtoTransport.Cost * totalDays) * 2;
                }
                else
                {
                    dtoTransport.Cost = dtoTransport.Cost * totalDays;
                }

                transport.Save(dtoTransport);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a Transport. 
        /// </summary>
        /// <param name="TransportId"></param>
        /// <returns></returns>
        public string DeleteTransport(long TransportId)
        {
            try
            {
                return transport.DeleteTransport(TransportId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // GET: YMMHRSystem/ExtraordinaryTransport
        public ActionResult ExtraordinaryTransport()
        {
            try
            {
                if (Permission.QueryPermission("EXTRATRANSPORT.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveExtraordinaryTransport"] = Permission.QueryPermission("EXTRATRANSPORT.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanDeleteExtraordinaryTransport"] = Permission.QueryPermission("EXTRATRANSPORT.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View("~/Areas/YMMHRSystem/Views/Transport/ExtraordinaryTransport.cshtml", transport.LoadMultipleExtra());
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
        /// Load view with new/existing Transport.
        /// </summary>
        /// <param name="extraordinaryTransportId"></param>
        /// <returns></returns>
        public ActionResult LoadExtraordinaryTransport(long extraordinaryTransportId)
        {
            try
            {
                Session["LoadedExtraTransportId"] = extraordinaryTransportId;

                DtoExtraordinaryTransport dtoExtraordinaryTransport = transport.LoadExtra(extraordinaryTransportId);

                return View("~/Areas/YMMHRSystem/Views/Transport/ExtraordinaryTransportDetailDialog.cshtml", dtoExtraordinaryTransport);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Transport.
        /// </summary>
        /// <param name="dtoExtraTransport"></param>
        /// <returns></returns>
        public ActionResult SaveExtraordinaryTransport(DtoExtraordinaryTransport dtoExtraTransport)
        {
            try
            {
                DateTime startDateTime = Convert.ToDateTime(dtoExtraTransport.StartDate + dtoExtraTransport.StartTime);
                if (startDateTime.Subtract(DateTime.Now).TotalHours < 4)
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }

                dtoExtraTransport.ExtraordinaryTransportId = Convert.ToInt64(Session["LoadedExtraTransportId"]);
                if (dtoExtraTransport.ExtraordinaryTransportId == 0)
                {
                    if (transport.GetExtraTransportId(dtoExtraTransport.AssociateName, dtoExtraTransport.Route, dtoExtraTransport.Shift, dtoExtraTransport.StartDate, dtoExtraTransport.FinishDate) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }

                transport.SaveExtra(dtoExtraTransport);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a Transport. 
        /// </summary>
        /// <param name="extraordinaryTransportId"></param>
        /// <returns></returns>
        public string DeleteExtraordinaryTransport(long extraordinaryTransportId)
        {
            try
            {
                return transport.DeleteExtraTransport(extraordinaryTransportId) ? "true" : "false";
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