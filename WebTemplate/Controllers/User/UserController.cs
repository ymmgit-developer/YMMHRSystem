using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;
using System.Data;

namespace WebTemplate.Controllers
{
    [SessionExpireFilter]
    public class UserController : Controller
    {
        User user = new User();
        Permission permission = new Permission();
        Role role = new Role();

        [HttpGet]
        /// <summary>
        /// Carga la información de los usuarios en la tabla.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {
                if (Permission.QueryPermission("USUARIO.VER", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveUser"] = Permission.QueryPermission("USUARIO.REGISTRAR", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    HttpContext.Session["CanDeleteUser"] = Permission.QueryPermission("USUARIO.ELIMINAR", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                    return View(user.LoadMultiple());
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
        /// Carga la vista para agregar un nuevo usuario.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult LoadUser(long userId)
        {
            try
            {
                Session["LoadedUserId"] = userId;
                return View("AddUserDialog", user.Load(userId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Registra la información que es ingresada en DtoUsuario en la base de datos.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ActionResult SaveUser(DtoUser user)
        {
            User usr = new User();
            DtoUser dtoUsr = new DtoUser();

            try
            {
                dtoUsr.UserId = Convert.ToInt64(Session["LoadedUserId"]);
                dtoUsr.Name = user.Name;
                dtoUsr.FirstSurname = user.FirstSurname;
                dtoUsr.LastSurname = user.LastSurname;
                dtoUsr.WorkerId = user.WorkerId;
                dtoUsr.Email = user.Email;
                dtoUsr.Password = user.Password;
                dtoUsr.RoleId = user.RoleId;
                dtoUsr.Status = user.Status = "1";
                dtoUsr.Process = user.Process;
                this.user.Save(dtoUsr);
                role.SaveUserTasks(role.GetRoleTaskIdList(user.RoleId), dtoUsr.UserId);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Permite la configuración del estatus bloqueado/desbloqueado del usuario. 
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// 
        public string BlockUser(string userId, string status)
        {
            if (user.BlockUser(userId, status) == "1")
            {
                return "true";
            }
            return "false";
        }
    }
}


