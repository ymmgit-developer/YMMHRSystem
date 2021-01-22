using System.Web.Mvc;

namespace WebTemplate.Areas.YMMHRSystem
{
    public class YMMHRSystemAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "YMMHRSystem";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "YMMHRSystem_default",
                "YMMHRSystem/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}