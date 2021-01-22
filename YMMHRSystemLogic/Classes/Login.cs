using DBFramework;
using Encriptador;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace YMMHRSystemLogic
{
    public class Login
    {
        User user = new User();
        SQLTools oDatabase = new SQLTools();
        Log log = new Log();
        SendEmail sendEmail = new SendEmail();
        #region Standard Methods
        /// <summary>
        /// Valida los campos obligatorios del Login
        /// </summary>
        /// <param name="user"></param>
        public DataTable ValidateRequiredFields(DtoUser user)
        {
            var requiredProperties = new DataTable();
            requiredProperties.Columns.Add("Propiedad");
            requiredProperties.Columns.Add("Mensaje");

            if (string.IsNullOrEmpty(user.Email))
            {
                requiredProperties.Rows.Add("Email", "Es un campo Obligatorio");
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                requiredProperties.Rows.Add("Password", "Es un campo Obligatorio");
            }
            return requiredProperties;
        }
        /// <summary>
        /// Carga un usuario con sus campos
        /// </summary>
        /// <param name="unidadAdministrativaId"></param>
        /// <returns>dtoUsuario cargado</returns>
        public DtoLogin Load(long unidadAdministrativaId)
        {
            try
            {
                DBFrameworkMapping mapping = new DBFrameworkMapping();
                mapping.Load<DtoLogin>("CB_Unidades_Administrativas", new DtoLogin(), "UnidadAdministrativa_Id=" + unidadAdministrativaId);
                return (DtoLogin)mapping.dtoList.FirstOrDefault().Dto;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Ticket", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }

        }
        #endregion

        #region General Methods
        /// <summary>
        /// Carga el ID del usuario para que DBTools los use para la Bitácora.
        /// </summary>
        /// <param name="userId"></param>
        public void LoadSQLToolsStaticVariables(string userId)
        {
            SQLTools.userId = Convert.ToInt64(userId);
        }
        /// <summary>
        /// Método que valida las credenciales del usuario, 
        /// regresando un mensaje de Error si encuentra alguna anomalía.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="encrypted"></param>
        /// <returns>Mensaje</returns>
        public string ValidateUser(string user, string password, bool encrypted = false)
        {
            try
            {
                DataRow drUser;
                int status = 0;
                if (!encrypted)
                {
                    password = (string)cEncriptacion.fMD5Encriptar(password);
                }

                string sqlString = "SELECT Status,UserId FROM Users WHERE (WorkerId='" + user + "' OR Email = '" + user + "') AND Password='" + password + "' ";

                drUser = oDatabase.GetRow(sqlString, "Validate User");

                if (drUser == null)
                {
                    return "Usuario o Contraseña incorrecto";

                }
                status = Convert.ToInt32(drUser["Status"]);
                switch (status)
                {
                    case 0:
                        return "Su usuario ha sido bloqueado,  favor de contactar a su administrador";

                    case 2:
                        return "Cuenta no verificada, favor de contactar a su administrador";
                }

                return "";
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Validates User's Credentials", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "ValidateUser");
                throw ex;
            }
        }
        /// <summary>
        /// Envia un correo de recuperación de contraseña para que el usuario tenga el acceso al sitio.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="url"></param>
        /// <returns>True o False</returns>
        public string SendEmailPasswordRecovery(string user, string url)
        {
            try
            {
                string email = "";
                string password = "";
                string username = user;
                //Verifico que exista el usuario, si existe recupero su password y sus correos
                if (this.user.VerifyUserExistence(ref user, ref password, ref email))
                {
                    string[,] templateVariables =
                    {
                        {"$USER_NAME$", username},
                        {"$USER_URL$", EncryptUrlPasswordRecovery(user, url)}
                    };

                    return sendEmail.SendEmailTemplate("YMM HR System: Recuperación Contraseña", "TemplatePasswordRecovery", templateVariables, "", new List<string>() { email }).ToString();
                }
                return "";
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Send Email Password Recovery", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "SendEmailPasswordRecovery");
                throw ex;
            }
        }
        /// <summary>
        /// Encripta la URL que se envia al usuario mediante correo para recuperar su contraseña.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="unidad"></param>
        /// <param name="url"></param>
        /// <returns>URL encriptada</returns>
        public string EncryptUrlPasswordRecovery(string user, string url)
        {
            try
            {
                string message = url.Replace("SendEmailPasswordRecovery", "PasswordRecovery") + "/";

                string stringurl = user + "\\" + DateTime.Now;
                stringurl = cEncriptacion.f3DESEncriptar(stringurl);
                stringurl = stringurl.Replace("/", "sM=Ms");
                message += stringurl;
                message = message.Replace("+", "Ms=sM");
                return message;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Encrypts the  Password Recovery Url", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "EncryptUrlPasswordRecovery");
                throw ex;
            }
        }
        /// <summary>
        /// Desencripta la URL que el usuario presiono para recuperar su contraseña.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>URL Desencriptada</returns>
        public string[] DecryptUrlPasswordRecovery(string url)
        {
            try
            {
                string[] values = new string[2];

                url = url.Replace("Ms=sM", "+");
                url = url.Replace("sM=Ms", "/");

                string stringUrl = cEncriptacion.f3DESDesencriptar(url);
                string[] data = stringUrl.Split("\\".ToCharArray());

                values[0] = data[0];
                values[1] = data[1];

                return values;
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Decrypts the Password Recovery Url", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "DecryptUrlPasswordRecovery");
                throw ex;
            }
        }
        /// <summary>
        /// Actualiza la contraseña nueva del usuario.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        public void ChangePassword(string user, string password)
        {
            try
            {
                SQLTools oBD = new SQLTools();
                string nuevaContraseña = (string)cEncriptacion.fMD5Encriptar(password);

                oBD.ExecuteNonQuery("UPDATE Users SET Password ='" + nuevaContraseña + "' WHERE UserId=" + user, " Se actualiza la contraseña del usuario");
            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Change User Password", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "ChangePassword");
                throw ex;
            }

        }
        /// <summary>
        /// Envia el correo al administrador del Fraccionamiento con el asunto del usuario.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="asunto"></param>
        /// <param name="unidadAdministrativaId"></param>
        /// <returns>True o False</returns>
        public string SendContactEmail(string name, string email, string issue)
        {
            try
            {
                return sendEmail.SendEmailTemplate("YMM Child Parts System: Contactar al Admin", "TemplateContactAdmin", new[,]
                {
                    {"$USER_NAME$", name},
                    {"$USER_EMAIL$", email},
                    {"$USER_ASUNTO$", issue}
                }, "", new List<string>() { GetAdminEmail() }).ToString();

            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Save Ticket", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "Save");
                throw ex;
            }
        }
        /// <summary>
        /// Gets Admin Email
        /// </summary>
        /// <returns></returns>
        public string GetAdminEmail()
        {
            try
            {
                DataRow row = null;
                string sqlQuery = "SELECT AdminEmail FROM EmailConfiguration WHERE EmailConfigurationId = 1";
                row = oDatabase.GetRow(sqlQuery,"Get Admin Email");

                if (row == null || row["AdminEmail"].ToString() == "") return "";

                return row["AdminEmail"].ToString();

            }
            catch (Exception ex)
            {
                log.WriteToErrorLog("HR System", "Get Admin Email", SQLTools.userId.ToString(), ex.Message, ex.StackTrace, "GetAdminEmail");
                throw ex;
            }
        }
        #endregion
    }
}
