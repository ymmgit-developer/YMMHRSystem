using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace YMMHRSystemLogic
{
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Validar la información que se encuentra en la sesión.
            if (HttpContext.Current.Session["UserId"] == null)
            {
                // Si la información es nula, redireccionar a 
                // página de error u otra página deseada.
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                    { "Controller", "Login" },
                    { "Action", "Index" },
                    { "Area", string.Empty}
                });
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
