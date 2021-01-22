using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class ShiftController : Controller
    {
        Shift shift = new Shift();

        // GET: YMMHRSystem/Shift
        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("SHIFT.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveShift"] = Permission.QueryPermission("SHIFT.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanDeleteShift"] = Permission.QueryPermission("SHIFT.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View("~/Areas/YMMHRSystem/Views/Shift/Index.cshtml", shift.LoadMultiple());
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
        /// Load view with new/exisiting Shift.
        /// </summary>
        /// <param name="shiftId"></param>
        /// <returns></returns>
        public ActionResult LoadShift(long shiftId)
        {
            try
            {
                Session["LoadedShiftId"] = shiftId;

                DtoShift dtoShift = shift.Load(shiftId);

                return View("~/Areas/YMMHRSystem/Views/Shift/ShiftDetailDialog.cshtml", dtoShift);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Shift.
        /// </summary>
        /// <param name="dtoShift"></param>
        /// <returns></returns>
        public ActionResult SaveShift(DtoShift dtoShift)
        {
            DtoShift dtoShifts = new DtoShift();

            try
            {

                dtoShifts.ShiftId = Convert.ToInt64(Session["LoadedShiftId"]);
                if (dtoShifts.ShiftId == 0)
                {
                    if (shift.GetShiftId(dtoShift.Name) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }

                dtoShifts.Name = dtoShift.Name;
                dtoShifts.Symbol = dtoShift.Symbol;
                dtoShifts.UserCreated = dtoShift.UserCreated;
                dtoShifts.DateAdded = dtoShift.DateAdded;

                shift.Save(dtoShifts);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a Shift. 
        /// </summary>
        /// <param name="shiftId"></param>
        /// <returns></returns>
        public string DeleteShift(long shiftId)
        {
            try
            {
                return shift.DeleteShift(shiftId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
