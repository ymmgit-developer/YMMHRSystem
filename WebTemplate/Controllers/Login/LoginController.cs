using System;
using System.Data;
using System.Web.Mvc;
using YMMHRSystemLogic;
using System.Web.Configuration;

namespace WebTemplate.Controllers
{
    public class LoginController : Controller
    {
        Login login = new Login();
        User user = new User();
        public ActionResult Index()
        {
            return View(user.Load(0));
        }
        /// <summary>
        /// Si la acción proviene del correo de recuperacion, obtiene la URL encriptada y recupera los datos del usuario y del correo.
        /// Utilizandolos para validar la vigencia del correo de recuperación y generando el autologin para
        /// el usuario. Pasandoló a la pagina para cambiar su contraseña. Caso contrario, lo redirecciona a la pagina para cambiar su contraseña.
        /// </summary>
        /// <returns></returns>
        //[SessionExpireFilter]
        public ActionResult PasswordRecovery(bool isEmail = true)
        {
            try
            {
                User user = new User();
                if (isEmail)
                {
                    string url = Request.Url.AbsoluteUri;
                    string[] encryptedData = url.Split(Convert.ToChar("/"));
                    string[] logInData = login.DecryptUrlPasswordRecovery(encryptedData[encryptedData.Length - 1]);

                    DateTime horaLimite = Convert.ToDateTime(logInData[1]).AddHours(1);
                    DtoUser dtoUser = user.Load(Convert.ToInt32(logInData[0]));

                    Session["UserId"] = dtoUser.UserId;

                    if (DateTime.Now < horaLimite)
                    {
                        return View("PasswordRecovery");
                    }
                    return View("Index", dtoUser);
                }

                return View("PasswordRecovery");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Método que valida las credenciales del usuario, permitiendole el acceso a la pagina principal
        /// o redireccionandolo al Login con mensaje de error.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Mensaje de error o acceso a pagina principal</returns>
        public ActionResult LogIn(DtoUser user)
        {
            try
            {
                string mensaje = login.ValidateUser(user.Email, user.Password);

                Session["UserId"] = this.user.GetUserId((user.Email));               
                user = this.user.Load(Convert.ToInt64(Session["UserId"]));
                Session["UserName"] = user.Name + user.FirstSurname;

                //Cargar variables para DBTools
                login.LoadSQLToolsStaticVariables(Session["UserId"].ToString());

                if (string.IsNullOrEmpty(mensaje))
                {
                    return RedirectToAction("Index", "Home");
                }
                return View("Index", user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Borra todas las sesiones y se redirige al Controlador URL.
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
        /// <summary>
        /// Envia el correo al usuario con un enlace que lo redirije a la página Recuperar Contraseña.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>True o False</returns>
        public string SendEmailPasswordRecovery(string user)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(user)) return "";
                string url = Request.Url.AbsoluteUri;
                return login.SendEmailPasswordRecovery(user, url);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Actualiza la contraseña del usuario en la base de datos y lo redirecciona a la 
        /// página principal.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Pagina Principal</returns>
        [SessionExpireFilter]
        public ActionResult ChangePassword(DtoUser user)
        {
            try
            {
                string userId = Session["UserId"].ToString();
                login.ChangePassword(userId, user.Password);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}