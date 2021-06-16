using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class VehicleController : Controller
    {
        Vehicle vehicle = new Vehicle();

        // GET: YMMHRSystem/Vehicle
        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("VEHICLE.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveVehicle"] = Permission.QueryPermission("VEHICLE.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanCancelVehicle"] = Permission.QueryPermission("VEHICLE.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View("~/Areas/YMMHRSystem/Views/Vehicle/Index.cshtml", vehicle.LoadMultiple());
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
        /// Load view with new/exisiting Vehicle.
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public ActionResult LoadVehicle(long vehicleId)
        {
            try
            {
                Session["LoadedVehicleId"] = vehicleId;

                DtoVehicle dtoVehicle = vehicle.Load(vehicleId);

                return View("~/Areas/YMMHRSystem/Views/Vehicle/VehicleDetailDialog.cshtml", dtoVehicle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Vehicle.
        /// </summary>
        /// <param name="dtoVehicle"></param>
        /// <returns></returns>
        public ActionResult SaveVehicle(DtoVehicle dtoVehicle)
        {
            try
            {

                dtoVehicle.VehicleId = Convert.ToInt64(Session["LoadedVehicleId"]);
                if (dtoVehicle.VehicleId == 0)
                {
                    if (vehicle.GetVehicleId(dtoVehicle.EngineNumber) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                    dtoVehicle.Status = 1;
                }
                vehicle.Save(dtoVehicle);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a Vehicle. 
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public string CancelVehicle(long vehicleId)
        {
            try
            {
                return vehicle.CancelVehicle(vehicleId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
