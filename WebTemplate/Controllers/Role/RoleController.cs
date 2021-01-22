using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Controllers
{
    [SessionExpireFilter]
    public class RoleController : Controller
    {
        Role role = new Role();
        // GET: Rol
        public ActionResult Index()
        {
            if (Permission.QueryPermission("ROL.VER", long.Parse(HttpContext.Session["UserId"].ToString())))
            {
                HttpContext.Session["CanSaveRole"] = Permission.QueryPermission("ROL.REGISTRAR", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                HttpContext.Session["CanDeleteRole"] = Permission.QueryPermission("ROL.ELIMINAR", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;

                return View(role.LoadMultiple());
            }
            else
            {
                return View("~/Views/Shared/AccessDenied.cshtml");
            }

        }
        /// <summary>
        /// Navega a la vista de nuevo rol
        /// </summary>
        /// <returns></returns>
        public ActionResult AddNewRole()
        {
            return View("EditRole", new DtoRole());
        }
        /// <summary>
        /// Navega a la vista de edicion del rol seleccionado
        /// </summary>
        /// <param name="nMR4z">Es el usuarioId encriptado para que no se visualice en la URL</param>
        /// <returns></returns>
        public ActionResult EditRole(long nMR4z)
        {
            return View("EditRole", role.Load(nMR4z));
        }
        /// <summary>
        /// Registra el arbol de Permisos de Roles.
        /// </summary>
        /// <param name="taskList"></param>
        /// <param name="roleId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns>true o false</returns>
        public ActionResult SaveTreePermissions(List<DtoTask> taskList, string roleId, string name, string description)
        {
            try
            {
                DtoRole roles = new DtoRole();
                roles.RoleId = Convert.ToInt32(roleId);
                roles.Name = name;
                roles.Description = description;

                role.Save(roles);
                return Json(role.SaveRoleTree(taskList, roles.RoleId.ToString()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Elimina el rol que no tenga usuarios asignados
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns>true o false</returns>
        public string DeleteRole(string roleId)
        {
                return role.DeleteRole(roleId);     
        }
    }
}