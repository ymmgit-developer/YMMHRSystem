using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class FoodTypeController : Controller
    {
        // GET: YMMHRSystem/FoodType
        FoodType foodType = new FoodType();

        // GET: YMMHRSystem/FoodType
        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("FOODTYPE.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveFoodType"] = Permission.QueryPermission("FOODTYPE.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanDeleteFoodType"] = Permission.QueryPermission("FOODTYPE.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View("~/Areas/YMMHRSystem/Views/FoodType/Index.cshtml", foodType.LoadMultiple());
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
        /// Load view with new/exisiting FoodType.
        /// </summary>
        /// <param name="foodTypeId"></param>
        /// <returns></returns>
        public ActionResult LoadFoodType(long foodTypeId)
        {
            try
            {
                Session["LoadedFoodTypeId"] = foodTypeId;

                DtoFoodType dtoFoodType = foodType.Load(foodTypeId);

                return View("~/Areas/YMMHRSystem/Views/FoodType/FoodTypeDetailDialog.cshtml", dtoFoodType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a FoodType.
        /// </summary>
        /// <param name="dtoFoodType"></param>
        /// <returns></returns>
        public ActionResult SaveFoodType(DtoFoodType dtoFoodType)
        {

            try
            {

                dtoFoodType.FoodTypeId = Convert.ToInt64(Session["LoadedFoodTypeId"]);
                if (dtoFoodType.FoodTypeId == 0)
                {
                    if (foodType.GetFoodTypeId(dtoFoodType.Name) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }

                foodType.Save(dtoFoodType);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a FoodType. 
        /// </summary>
        /// <param name="FoodTypeId"></param>
        /// <returns></returns>
        public string DeleteFoodType(long FoodTypeId)
        {
            try
            {
                return foodType.DeleteFoodType(FoodTypeId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}