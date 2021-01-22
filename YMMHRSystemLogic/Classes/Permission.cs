using DBFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace YMMHRSystemLogic
{
    public class Permission
    {
        Log log = new Log();
        SQLTools oBD = new SQLTools();

        #region Standard Methods

        /// <summary>
        /// Carga un usuario con todos sus campos.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Dto de usuario cargado</returns>
        public List<DtoPermission> Load(long userId)
        {
            List<DtoPermission> permissionList = new List<DtoPermission>();
            DBFrameworkMapping mapping = new DBFrameworkMapping();
            try
            {

                mapping.Load<DtoPermission>("TaskCategories", new DtoPermission(), "Show=1");
                permissionList.AddRange(mapping.dtoList.Select(renglon => (DtoPermission)renglon.Dto));

                foreach (var row in permissionList)
                {
                    mapping.Load<DtoTask>("Tasks", new DtoTask(), "TaskCategoryId=" + row.TaskCategoryId);

                    foreach (DtoTask dtoTask in mapping.dtoList.Select(row2 => (DtoTask)row2.Dto))
                    {
                        row.TaskList.Add(dtoTask);
                    }
                }
                CheckUserTask(ref permissionList, userId);
                return permissionList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Permissions", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        #endregion

        #region General Methods

        /// <summary>
        /// Obtiene los permisos por capas/niveles de un usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Lista de permisos de un Usuario</returns>
        public List<DtoPermission> GetPermissionsByUser(long userId)
        {
            var permissionList = new List<DtoPermission>();
            try
            {
                var data = new DataTable();
                var orderedData = new DataTable();
                string firstQuery = "";
                string secondQuery = "";

                //Recuperar las subcategorias de las Categorias
                firstQuery = " exec spGetMenuPrincipal " + userId;

                data = oBD.GetTable(firstQuery, "Get Categories");
                orderedData = oBD.GetTable(firstQuery, "Get Categories");

                orderedData.Rows.Clear();

                //VALIDACION PARA ELIMINAR LOS PAPAS QUE NO TIENEN HIJOS
                DataTable totalFathers = new DataTable();

                secondQuery =
                    " SELECT DISTINCT TP.TaskCategoryId, TP.Name, TP.FatherCategoryId, TP.Icon,TP.Url,TP.Show,'FATHER' as Menu " +
                    " FROM [TaskCategories] TP " +
                    " JOIN  [TaskCategories] TC ON TP.TaskCategoryId=TC.FatherCategoryId " +
                    " WHERE TP.Show=1";
                totalFathers = oBD.GetTable(secondQuery, "");


                data.PrimaryKey = new[] { data.Columns["TaskCategoryId"] };

                // Print column 0 of each returned row.

                foreach (DataRow father in totalFathers.Rows)
                {
                    var hasChild = false;
                    var expression = " (MENU = 'CHILD' AND FatherCategoryId=" + father["TaskCategoryId"] +
                                     ") OR FatherCategoryId=" + father["TaskCategoryId"];
                    var existingRows = data.Select(expression);

                    for (int iP = 0; iP <= existingRows.GetUpperBound(0); iP++)
                    {
                        hasChild = true;
                        break;
                    }
                    if (hasChild == false)
                    {
                        if (father["Menu"].ToString() != "FATHER") continue;
                        //Es un Papa sin Hijos (En la tabla datos no existen hijos)
                        DataRow f = data.Rows.Find(father["FatherCategoryId"]);

                        expression = " (MENU = 'CHILD' AND FatherCategoryId=" +
                                     father["FatherCategoryId"] + ")" +
                                     " AND TaskCategoryId <>" + father["TaskCategoryId"];
                        existingRows = data.Select(expression);

                        if (existingRows.GetUpperBound(0) != 0)
                        {
                            //Si hay Hermanos, entonces elimino el hijo sin hijos
                            f = (data.Rows.Find(father["TaskCategoryId"]));
                            data.Rows.Remove(f);
                        }
                        else
                        {
                            ////No hay hermanos
                            data.Rows.Remove(f);
                            f = data.Rows.Find(father["TaskCategoryId"]);
                            data.Rows.Remove(f);
                        }
                    }
                    else
                    {
                        //Validar Si el Hijo Es un Papa sin Hijos
                        expression = " FatherCategoryId=" + father["TaskCategoryId"];
                        existingRows = data.Select(expression);
                        for (int iP = 0; iP <= existingRows.GetUpperBound(0); iP++)
                        {
                            //Es un Papa sin Hijos (En la tabla dtDatos existen hijos)
                            hasChild = true;
                            break;
                        }
                    }
                }

                if (data.Rows.Count == 1 && data.Rows[0]["Icon"].ToString() == "mif-cog")
                {
                    data.Rows.Clear();
                }
                permissionList.AddRange(from DataRow row in data.Rows
                                        select new DtoPermission
                                        {
                                            TaskCategoryId = (long)row["TaskCategoryId"],
                                            Name = row["Name"].ToString(),
                                            FatherCategoryId = (long)row["FatherCategoryId"],
                                            Icon = (string)row["Icon"],
                                            Url = (string)row["Url"]
                                        });
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Permissions By User", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetPermissionsByUser");
                throw ex;
            }

            return permissionList;
        }
        /// <summary>
        /// Coloca el valor booleano true en la propiedad tieneCheck cuando el usuario tenga esa tarea registrada.
        /// </summary>
        /// <param name="permissionList"></param>
        /// <param name="userId"></param>
        public void CheckUserTask(ref List<DtoPermission> permissionList, long userId)
        {
            try
            {
                DataTable userTasks = new DataTable();
                string query = "SELECT TaskId FROM UserTasks WHERE UserId=" + userId;
                userTasks = oBD.GetTable(query, "Recupera las tarea asignados al usuario");

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
                log.WriteToErrorLog("HR System", "Check User Task", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "CheckUserTask");
                throw ex;
            }
        }
        /// <summary>
        /// Obtiene cual es la tarea que realizó el usuario.
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public string GetTaskIdByName(string keyName)
        {
            try
            {
                DataRow tareaId = null;
                string sqlString = "SELECT TaskId FROM Tasks WHERE KeyName = '" + keyName.ToUpper() + "'";

                tareaId = oBD.GetRow(sqlString, "Obtener TaskId ");

                return tareaId["TaskId"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Task Id By Name", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetTaskIdByName");
                throw ex;
            }


        }
        /// <summary>
        /// Elimina todas las tareas con respecto al usuario y le registra las nuevas.
        /// </summary>
        /// <param name="taskList"></param>
        /// <param name="userId"></param>
        /// <returns>true o false</returns>
        public string SavePermissionTree(List<DtoTask> taskList, string userId)
        {
            try
            {
                List<string> queryList = new List<string>();
                string sqlString = "DELETE UserTasks WHERE UserId = " + userId;
                oBD.ExecuteNonQuery(sqlString, "Elimina tareas del usuario del Treeview del módulo Permisos");

                foreach (var permission in taskList.FindAll(permission => permission.hasCheck))
                {
                    queryList.Add("INSERT INTO UserTasks VALUES (" + userId + "," + permission.TaskId + ")");
                }

                oBD.ExecuteMultipleNonQueryList(queryList, "Inserta las nuevas tareas al usuario");

                return "true";
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Permission Tree", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SavePermissionTree");
                throw ex;
            }
        }
        /// <summary>
        /// Method in charge of asking if a user has the determined permission
        /// </summary>
        /// <param name="keyName">Recibe el nombre clave del permiso a consultar</param>
        /// <param name="userId">Recibe el identificador del usuario</param>
        /// <returns>Devuelve un verdadero si el usuario cuenta con el permiso o un falso si no tiene ese permiso</returns>
        public static bool QueryPermission(string keyName, long userId)
        {
            try
            {
                SQLTools oBD = new SQLTools();
                DataRow dr;
                dr = oBD.GetRow("select COALESCE(COUNT(*),0) as Total from UserTasks ut " +
                                       " JOIN Tasks t on ut.TaskId = t.TaskId " +
                                       " where ut.UserId =" + userId + "  and t.KeyName = '" + keyName + "'", "");

                return dr["Total"].ToString().Equals("0") ? false : true;
            }
            catch (Exception ex)
            {
                Log log = new Log();
                log.WriteToErrorLog("HR System", "Query Permission", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "QueryPermission");
                throw ex;
            }

        }
        #endregion
    }
}