using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMMHRSystemLogic;
using System.Data;
using DBFramework;

namespace YMMHRSystemLogic
{
    public class Role
    {
        Log log = new Log();
        SQLTools oBD = new SQLTools();
        #region Standard Methods
        /// <summary>
        /// Genera una lista con las categorias y las tareas de un rol, llenando la propiedad tieneCheck para su futuro uso en el TreeView
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns>Lista de Permisos</returns>
        public DtoRole Load(long roleId)
        {
            DtoRole role = new DtoRole();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (roleId != 0)
                {
                    mapping.Load<DtoRole>("Roles", new DtoRole(), "RoleId=" + roleId);
                    role = (DtoRole)mapping.dtoList.FirstOrDefault().Dto;
                }
                role.UserList = GetUsersByRole(roleId);

                return role;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Role", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Carga multiples roles con sus campos respectivos.
        /// </summary>
        /// <returns>multiples roles cargados</returns>
        public List<DtoRole> LoadMultiple()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoRole> roleList = new List<DtoRole>();

                mapping.Load<DtoRole>("Roles", new DtoRole());
                roleList.AddRange(mapping.dtoList.Select(renglon => (DtoRole)renglon.Dto));

                return roleList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Roles", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }

        }
        /// <summary>
        /// Registra un rol con el Mapping de DBTools
        /// </summary>
        /// <param name="role"></param>
        public void Save(DtoRole role)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = role, TableName = "Roles" });
                mapping.Save();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Role", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        #endregion

        #region General Methods

        /// <summary>
        /// Recupera el id de las tareas asignadas al rol 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<DtoTask> GetRoleTaskIdList(int roleId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoTask> taskIdList = new List<DtoTask>();

                mapping.Load<DtoTask>("RoleTasks", new DtoTask(), "RoleId=" + roleId);
                taskIdList.AddRange(mapping.dtoList.Select(row => (DtoTask)row.Dto));

                return taskIdList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Role Task Id List", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetRoleTaskIdList");
                throw ex;
            }

        }
        /// <summary>
        /// Registra las tareas asignadas al  usuarioId
        /// </summary>
        /// <param name="taskIdList"></param>
        /// <param name="userId"></param>
        public string SaveUserTasks(List<DtoTask> taskIdList, long userId)
        {
            try
            {
                List<string> taskList = new List<string>();
                string sqlString = "DELETE UserTasks WHERE UserId = " + userId;
                oBD.ExecuteNonQuery(sqlString, "Elimina tareas del usuario");

                foreach (var tarea in taskIdList)
                {
                    taskList.Add("INSERT INTO UserTasks VALUES (" + userId + "," + tarea.TaskId + ")");
                }

                oBD.ExecuteMultipleNonQueryList(taskList, "Inserta las nuevas tareas al usuario");

                return "true";
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save User Tasks", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveUserTasks");
                throw ex;
            }
        }
        /// <summary>
        /// Genera una lista con las categorias y las tareas de un rol, llenando la propiedad tieneCheck para su futuro uso en el TreeView
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="unidadAdministrativa_Id"></param>
        /// <returns>Lista de Permisos</returns>
        public List<DtoPermission> LoadRolePermissions(long roleId)
        {
            List<DtoPermission> permissionList = new List<DtoPermission>();
            DBFrameworkMapping mapping = new DBFrameworkMapping();
            try
            {
                mapping.Load<DtoPermission>("TaskCategories", new DtoPermission());
                permissionList.AddRange(mapping.dtoList.Select(renglon => (DtoPermission)renglon.Dto));

                foreach (var row in permissionList)
                {

                    mapping.Load<DtoTask>("Tasks", new DtoTask(), "TaskCategoryId=" + row.TaskCategoryId);

                    foreach (DtoTask dtoTarea in mapping.dtoList.Select(renglon2 => (DtoTask)renglon2.Dto))
                    {
                        row.TaskList.Add(dtoTarea);
                    }
                }
                CheckRoleTasks(ref permissionList, roleId);
                return permissionList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Role Permissions", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadRolePermissions");
                throw ex;
            }
        }
        /// <summary>
        /// Llena la propiedad tieneCheck con true a las tareas asignadas al rol.
        /// </summary>
        /// <param name="permissionList"></param>
        /// <param name="roleId"></param>
        public void CheckRoleTasks(ref List<DtoPermission> permissionList, long roleId)
        {
            try
            {
                DataTable userTasks = new DataTable();
                string query = "SELECT TaskId FROM RoleTasks WHERE RoleId=" + roleId;
                userTasks = oBD.GetTable(query, "Recupera las tarea asignados al rol");

                foreach (var permission in permissionList.FindAll(permission => permission.TaskList.Any()))
                {
                    foreach (var task in permission.TaskList)
                    {
                        if (userTasks.Select("TaskId=" + task.TaskId).Any())
                        {
                            task.hasCheck = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Check Role Tasks", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "CheckRoleTasks");
                throw ex;
            }



        }
        /// <summary>
        /// Recupera una lista de usuarios por rol.
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns>Lista de usaurios de tipo cadena</returns>
        private List<string> GetUsersByRole(long roleId)
        {
            try
            {
                DataTable users = new DataTable();
                List<string> userList = new List<string>();
                string sqlString = "SELECT Name, FirstSurname, LastSurname FROM Users WHERE RoleId=" + roleId;

                users = oBD.GetTable(sqlString, "Get users By Rol");

                foreach (DataRow renglon in users.Rows)
                {
                    userList.Add(renglon[0].ToString() + " " + renglon[1].ToString() + " " + renglon[2].ToString());
                }

                return userList;
            }
            catch (Exception ex)
            {

                log.WriteToErrorLog("HR System", "Get Users By Role", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetUsersByRole");
                throw ex;
            }

        }
        /// <summary>
        /// Elimina todas las tareas con respecto al usuario y le registra las nuevas.
        /// </summary>
        /// <param name="taskList"></param>
        /// <param name="roleId"></param>
        /// <returns>true o false</returns>
        public string SaveRoleTree(List<DtoTask> taskList, string roleId)
        {
            try
            {
                List<string> queryList = new List<string>();
                string sqlString = "DELETE RoleTasks WHERE RoleId = " + roleId;
                oBD.ExecuteNonQuery(sqlString, "Elimina tareas del usuario del Treeview del módulo Roles");

                foreach (var permission in taskList.FindAll(permission => permission.hasCheck))
                {
                    queryList.Add("INSERT INTO RoleTasks VALUES (" + roleId + "," + permission.TaskId + ")");
                }

                oBD.ExecuteMultipleNonQueryList(queryList, "Inserta las nuevas tareas al rol");

                return "true";
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Role Tree", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveRoleTree");
                throw ex;
            }
        }
        /// <summary>
        /// Elimina un rol de su tabla principal y relacionados, solo si no tiene usuarios asignados.
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns>true o false</returns>
        public string DeleteRole(string roleId)
        {
            try
            {
                DataTable rolesByUser = new DataTable();
                string sqlString = "SELECT UserId FROM User WHERE RoleId =" + roleId;
                string sqlString1 = "DELETE Roles WHERE RoleId = " + roleId;
                string sqlString2 = "DELETE RoleTasks WHERE RoleId = " + roleId;

                rolesByUser = oBD.GetTable(sqlString, "Obtener los usuarios por rol");

                if (rolesByUser.Rows.Count > 0)
                {
                    return "false";
                }
                oBD.ExecuteNonQuery(sqlString2, "Se eliminan las tareas del rol seleccionado para poder eliminar el rol");
                oBD.ExecuteNonQuery(sqlString1, "Se elimina el rol seleccionado sin usuarios relacionados");
                return "true";
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Delete Role", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DeleteRole");
                throw ex;
            }

        }
        /// <summary>
        /// Method in charge of asking if a user has the determined role
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="userId"></param>
        /// <returns>Returns true if the user has the role, or false if not</returns>
        public static bool QueryRole(string roleName, long userId)
        {
            try
            {
                SQLTools oBD = new SQLTools();
                DataRow dr;
                dr = oBD.GetRow("select COALESCE(COUNT(*),0) as Total from Users u " +
                                       " JOIN Roles r on u.RoleId = r.RoleId " +
                                       " where u.UserId =" + userId + "  and r.Name = '" + roleName + "'", "");

                return dr["Total"].ToString().Equals("0") ? false : true;
            }
            catch (Exception ex)
            {
                Log log = new Log();
                log.WriteToErrorLog("HR System", "Query Role", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "QueryRole");
                throw ex;
            }

        }
        #endregion



    }
}
