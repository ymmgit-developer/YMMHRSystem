using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using YMMHRSystemLogic;
using DBFramework;

namespace YMMHRSystemLogic
{
    public class User
    {
        Permission permission = new Permission();
        SQLTools oDatabase = new SQLTools();
        Log log = new Log();
        Role role = new Role();
        #region Standard Methods
        /// <summary>
        /// Carga el dto de Usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>DtoUsuario Cargado</returns>
        public DtoUser Load(long userId)
        {
            Login login = new Login();
            DtoUser user = new DtoUser();
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                if (userId != 0)
                {
                    mapping.Load<DtoUser>("Users", new DtoUser(), "UserId=" + userId);
                    user = (DtoUser)mapping.dtoList.FirstOrDefault().Dto;
                }
                user.PermissionList = permission.GetPermissionsByUser(userId);
                user.RoleList = role.LoadMultiple();

                return user;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load User", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Load");
                throw ex;
            }
        }
        /// <summary>
        /// Registrar usuario 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="usuarioLogeado"></param>
        /// <returns>Usuario registrado</returns>
        public void Save(DtoUser user)
        {
            try
            {
                bool nuevo;
                nuevo = user.UserId == 0;
                if (!string.IsNullOrEmpty(user.Password))
                {
                    if (nuevo)
                    {
                        user.Password = Encriptador.cEncriptacion.fMD5Encriptar(user.Password).ToString();
                    }
                    else
                    {
                        //Es modificación
                        string passwordActualBd = (string)new SQLTools().ExecuteScalar("select password from Users where UserId=" + user.UserId, "Recuperar el password del usuario");
                        if (!passwordActualBd.Equals(user.Password.Trim()))
                        {
                            user.Password = Encriptador.cEncriptacion.fMD5Encriptar(user.Password).ToString();
                        }
                    }
                }
                user.Email = user.Email.ToLower();
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.dtoList.Add(new DBFrameworkDto() { Dto = user, TableName = "Users" });
                mapping.Save();

                if (nuevo)
                {
                    CreatedBy("Users", SQLTools.userId, "UserId", user.UserId);
                }
                else
                {
                    ModifiedBy("Users", SQLTools.userId, "UserId", user.UserId);
                }
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save User", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary>
        /// Carga multiples usuarios con sus respectivos campos.
        /// </summary>
        /// <returns>Dtos de usuario cargados.</returns>
        public List<DtoUser> LoadMultiple(bool activeOnly = false)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoUser> userList = new List<DtoUser>();

                string activeFilter = activeOnly ? " WHERE usuario.Status = 1" : "";
                mapping.Load<DtoUser>("SELECT usuario.*, rol.Name as RoleName FROM Users usuario JOIN Roles rol ON usuario.RoleId=rol.RoleId" + activeFilter, "Users", new DtoUser());
                userList.AddRange(mapping.dtoList.Select(renglon => (DtoUser)renglon.Dto));

                return userList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Load Multiple Users", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "LoadMultiple");
                throw ex;
            }

        }
        #endregion

        #region General Methods
        /// <summary>
        /// Comprueba la existencia de un usuario regresando sus valores por referencia. 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="email2"></param>
        /// <returns>Datos del Usuario</returns>
        public bool VerifyUserExistence(ref string user, ref string password, ref string email)
        {
            try
            {
                DataRow drUser = null;
                int status = 0;
                string sqlString = "select UserId, Email, Password, Status from Users Where ((WorkerId = '" + user + "') OR (Email='" + user + "'))";

                drUser = oDatabase.GetRow(sqlString, "VerifyUserExistence");

                if (drUser == null) return false;

                status = Convert.ToInt32(drUser["Status"]);

                switch (status)
                {
                    case 2:
                        throw new Exception("Su usuario ha sido bloqueado,  favor de contactar a su administrador");
                    case 3:
                        throw new Exception("Cuenta no verificada, favor de contactar a su administrador");
                }

                user = drUser["UserId"].ToString();
                password = drUser["Password"].ToString();
                email = drUser["Email"].ToString();
                return true;

            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Verify User Existence", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "VerifyUserExistence");
                throw ex;
            }
        }
        /// <summary>
        /// Recupera el ID del usuario deseado.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public long GetUserId(string user)
        {
            try
            {
                DataRow drUser = null;
                string sqlString = "SELECT UserId FROM Users WHERE (WorkerId = '" + user + "') OR (Email='" + user + "')";

                drUser = oDatabase.GetRow(sqlString, "Get UserId");

                if (drUser == null) return 0;

                return Convert.ToInt64(drUser["UserId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Gets User Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetUserId");
                throw ex;
            }

        }
        /// <summary>
        /// Recupera el correo del usuario deseado.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserEmail(long userId)
        {
            try
            {
                DataRow drUser = null;
                string sqlString = "SELECT Email FROM Users WHERE UserId=" + userId;

                drUser = oDatabase.GetRow(sqlString, "Get User Email");

                if (drUser == null) return "";

                return drUser["Email"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get User Email", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetUserEmail");
                throw ex;
            }

        }
        /// <summary>
        /// Recupera el nombre del usuario deseado.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserName(string userId)
        {
            try
            {
                DataRow drUser = null;
                string sqlString = "SELECT (Name + ' ' + FirstSurname + ' ' + LastSurname) as Name FROM Users WHERE UserId=" + userId;

                drUser = oDatabase.GetRow(sqlString, "Get User Name");

                if (drUser == null) return "";

                return drUser["Name"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Username", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetUserName");
                throw ex;
            }

        }
        /// <summary>
        /// Obtiene el Nick de un usuario en específico
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Retorna el nick del usuario deseado</returns>
        public string GetWorkerId(long userId)
        {
            try
            {
                DataRow drUser = null;
                string sqlString = "SELECT WorkerId FROM Users WHERE UserId=" + userId;

                drUser = oDatabase.GetRow(sqlString, "Get Worker ID");

                if (drUser == null) return "";

                return drUser["WorkerId"].ToString();
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Worker Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetWorkerId");
                throw ex;
            }
        }
        /// <summary>
        /// Permite la atualización del estatus del usuario a bloqueado/desbloqueado. 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public string BlockUser(string userId, string status)
        {
            try
            {
                status = (status.Equals("false")) ? "0" : "1";
                string query = "UPDATE Users SET Status='" + status + "' WHERE UserId='" + userId + "'";

                oDatabase.ExecuteNonQuery(query, " User status update");
                return status;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Block User", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "BlockUser");
                throw ex;
            }

        }
        /// <summary>
        /// Permite la recuperación del rol asignado al usuario. 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int RecoveryRoleIdByUser(int userId)
        {
            try
            {
                DataRow rolId;
                string sqlString = "SELECT RoleId FROM Users WHERE UserId  = '" + userId + "'";

                rolId = oDatabase.GetRow(sqlString, "Get User Role Id ");

                return Convert.ToInt32(rolId["RoleId"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Recovery Role Id By User", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "RecoveryRoleIdByUser");
                throw ex;
            }


        }
        /// <summary>
        /// Actualiza el usuario nuevo, registrando su primer inicio de sesión.
        /// </summary>
        /// <param name="userId"></param>
        public void SaveFirstLogIn(string userId)
        {
            try
            {
                string sqlString = "UPDATE Users SET FirstLogIn=0 WHERE UserId=" + userId;
                oDatabase.ExecuteNonQuery(sqlString, "Actualiza el campo de FirstLogIn a 0, afirmando que ya no es su primer inicio de sesion");
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save First LogIn", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SaveFirstLogIn");
                throw ex;
            }

        }
        /// <summary>
        /// Recupera el valor de la columna primer inicio de sesión.
        /// </summary>
        /// <param name="userId"></param>
        public bool RecoverFirstLogIn(string userId)
        {
            try
            {
                string sqlString = "SELECT FirstLogIn FROM Users WHERE UserId=" + userId;
                DataRow firstLogIn = oDatabase.GetRow(sqlString, "Get FirstLogIn Value");

                return firstLogIn != null && Convert.ToBoolean(firstLogIn["FirstLogIn"]);
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Recover First LogIn", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "RecoverFirstLogIn");
                throw ex;
            }

        }
        /// <summary>
        /// Método que se encarga de validar la disponibilidad de un correo para un usuario en una unidad administrativa
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Retorna true si el correo ya esta asociado a otro usuario</returns>
        private bool ValidateSameUserEmail(DtoUser user)
        {
            try
            {
                string query = "SELECT top(1) UserId from Users where (email='" + user.Email + "' or WorkerId='" + user.Email + "') and UserId <>" + user.UserId;
                SQLTools dbManager = new SQLTools();
                int userEmailExistence = Convert.ToInt32(dbManager.ExecuteScalar(query, "Recuperar si ya existe un usuario con el mismo email en la misma unidad administrativa"));
                if (userEmailExistence > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Validate Same User Email", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "ValidateSameUserEmail");
                throw ex;
            }

        }
        /// <summary>
        /// Validates if the worker ID already exists
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private bool ValidateSameWorkerId(DtoUser user)
        {
            try
            {
                string query = "select top(1) UserId from Users where WorkerId='" + user.WorkerId + "' and UserId <>" + user.UserId;
                SQLTools dbManager = new SQLTools();
                int existeUsuarioConEmail = Convert.ToInt32(dbManager.ExecuteScalar(query, "Recuperar si ya existe un usuario con el mismo nick en la misma unidad administrativa"));
                if (existeUsuarioConEmail > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Validate Same Worker Id", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "ValidateSameWorkerId");
                throw ex;
            }
           
        }
        /// <summary>
        /// Actualiza los campos Usr_Registro, Usr_Mod y sus fechas al insertar un nuevo registro a una tabla. Nota: la tabla debe tener estos campos.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="userId"></param>
        /// <param name="pkName"></param>
        /// <param name="pkKey"></param>
        public void CreatedBy(string table, long userId, string pkName, long pkKey)
        {
            try
            {
                string sqlString = "UPDATE " + table + " SET CreatedBy=" + userId + ", DateCreated='" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "',ModifiedBy=" + userId + ", DateModified='" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "' WHERE " + pkName + "=" + pkKey;
                oDatabase.ExecuteNonQuery(sqlString, "");
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Inserts Created By Field", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "CreatedBy");
                throw ex;
            }
        }
        /// <summary>
        /// Actualiza los campos Usr_Mod y su fecha al actualizar un registro existente de una tabla. Nota: la tabla debe tener estos campos.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="userId"></param>
        /// <param name="pkName"></param>
        /// <param name="pkKey"></param>
        public void ModifiedBy(string table, long userId, string pkName, long pkKey)
        {
            try
            {
                string sqlString = "UPDATE " + table + " SET ModifiedBy=" + userId + ", DateModified='" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "' WHERE " + pkName + "=" + pkKey;
                oDatabase.ExecuteNonQuery(sqlString, "");
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Updates Modified By Field", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "ModifiedBy");
                throw ex;
            }
        }
        /// <summary>
        /// Method in charge of asking if a user has the determined process
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Process</returns>
        public static string QueryProcess(long userId)
        {
            try
            {
                SQLTools oBD = new SQLTools();
                DataRow dr;
                dr = oBD.GetRow("select Process FROM Users u WHERE u.UserId =" + userId, "");

                return dr["Process"].ToString();
            }
            catch (Exception ex)
            {
                Log log = new Log();
                log.WriteToErrorLog("HR System", "Query Process", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "QueryProcess");
                throw ex;
            }

        }
        /// <summary>
        /// Recupera una lista de correo de los usuarios sugeridos.
        /// </summary>
        /// <param input="typing"></param>
        /// <returns></returns>
        public List<DtoUser> GetUserEmailSuggestions()
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                List<DtoUser> userList = new List<DtoUser>();

                mapping.Load<DtoUser>("SELECT DISTINCT Name, FirstSurname, LastSurname ,Email FROM Users WHERE Status = 1", "Users", new DtoUser());
                userList.AddRange(mapping.dtoList.Select(renglon => (DtoUser)renglon.Dto));

                return userList;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get User Email", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetUserEmailSuggestions");
                throw ex;
            }

        }
        /// <summary>
        /// Recupera el proceso de un usuario.
        /// </summary>
        /// <param input="userId"></param>
        /// <returns>El proceso al que pertenece el usuario</returns>
        public string GetProcessbyUser(long userId)
        {
            try
            {
                SQLTools oBD = new SQLTools();
                DataRow dr;
                dr = oBD.GetRow("select Process FROM Users u WHERE u.UserId =" + userId, "");

                return dr["Process"].ToString();
            }
            catch (Exception ex)
            {
                Log log = new Log();
                log.WriteToErrorLog("HR System", "Get Process by User", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetProcessbyUser");
                throw ex;
            }
        }

        public string GetEmailbyName(string Name) 
        {
            try
            {
                SQLTools oBD = new SQLTools();
                DataRow dr;
                dr = oBD.GetRow("select Email FROM Users u WHERE CONCAT(Name, ' ' ,FirstSurname)  LIKE('" + Name + "')", "");

                return dr["Email"].ToString();
            }
            catch (Exception ex)
            {
                Log log = new Log();
                log.WriteToErrorLog("HR System", "Get email by Name", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetEmailbyName");
                throw ex;
            }
        }
        #endregion
    }
}
