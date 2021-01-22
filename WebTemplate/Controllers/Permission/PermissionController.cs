using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Controllers
{
    [SessionExpireFilter]
    public class PermissionController : Controller
    {
        User user = new User();
        Permission permission = new Permission();
        Role role = new Role();
        // GET: Permiso
        [HttpGet]
        public ActionResult Index()
        {
            if (Permission.QueryPermission("PERMISO.VER", long.Parse(HttpContext.Session["UserId"].ToString())))
            {
                HttpContext.Session["CanSavePermission"] = Permission.QueryPermission("PERMISO.REGISTRAR", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                return View(user.LoadMultiple());
            }
            else
            {
                return View("~/Views/Shared/AccessDenied.cshtml");
            }
           
        }
        /// <summary>
        /// Carga los permisos en un Treeview de Checkbox por usuario.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Regresa una vista parcial con el Treeview cargado</returns>
        public ActionResult LoadTreeByUser(string userId)
        {
            try
            {
               List<DtoPermission> permissionList = permission.Load(Convert.ToInt64(userId));
                return View("PermissionTree", permissionList);
            }           
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves the user's permissions
        /// </summary>
        /// <param name="taskList"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string SaveTreePermissons(List<DtoTask> taskList, string userId)
        {
            try
            {
                return permission.SavePermissionTree(taskList, userId);
            }
            catch (Exception ex)
            {
                return "false";
            }      
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public ActionResult ReturnDefaultRole(string roleId)
        {
            try
            {
                List<DtoPermission> listaPermisos = role.LoadRolePermissions(Convert.ToInt64(roleId));
                return View("PermissionTree", listaPermisos);
            }           
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}