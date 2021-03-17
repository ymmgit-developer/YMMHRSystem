using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Areas.YMMHRSystem.Controllers
{
    [SessionExpireFilter]
    public class RoomController : Controller
    {
        Room room = new Room();

        // GET: YMMHRSystem/Room
        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("ROOM.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveRoom"] = Permission.QueryPermission("ROOM.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanDeleteRoom"] = Permission.QueryPermission("ROOM.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View("~/Areas/YMMHRSystem/Views/Room/Index.cshtml", room.LoadMultiple());
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
        /// Load view with new/exisiting Room.
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public ActionResult LoadRoom(long roomId)
        {
            try
            {
                Session["LoadedRoomId"] = roomId;

                DtoRoom dtoRoom = room.Load(roomId);

                return View("~/Areas/YMMHRSystem/Views/Room/RoomDetailDialog.cshtml", dtoRoom);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves a Room.
        /// </summary>
        /// <param name="dtoRoom"></param>
        /// <returns></returns>
        public ActionResult SaveRoom(DtoRoom dtoRoom)
        {
            try
            {

                dtoRoom.RoomId = Convert.ToInt64(Session["LoadedRoomId"]);
                if (dtoRoom.RoomId == 0)
                {
                    if (room.GetRoomId(dtoRoom.Name) != 0)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }

                room.Save(dtoRoom);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Removes a Room. 
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public string DeleteRoom(long roomId)
        {
            try
            {
                return room.DeleteRoom(roomId) ? "true" : "false";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
