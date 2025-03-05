using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YMMHRSystemLogic;

namespace WebTemplate.Controllers
{
    [SessionExpireFilter]
    public class HomeController : Controller
    {
        Login login = new Login();
        WorkerFile workerFile = new WorkerFile();
        User user = new User();
        EmailNotification emailNotification = new EmailNotification();
        // GET: Home
        public ActionResult Index()
        {
            try
            {
                HttpContext.Session["WorkerFileName"] = workerFile.GetWorkerFileName(long.Parse(user.GetWorkerId(long.Parse(HttpContext.Session["UserId"].ToString()))));
                HttpContext.Session["UserProcess"] = user.GetProcessbyUser(long.Parse(HttpContext.Session["UserId"].ToString()));
                HttpContext.Session["WorkerId"] = user.GetWorkerId(long.Parse(HttpContext.Session["UserId"].ToString()));
                HttpContext.Session["WorkerFileId"] = workerFile.GetWorkerFileId(user.GetWorkerId(long.Parse(HttpContext.Session["UserId"].ToString())));
                if (Permission.QueryPermission("WORKERFILE.VIEW", long.Parse(HttpContext.Session["UserId"].ToString())))
                {
                    HttpContext.Session["CanSaveWorkerFile"] = Permission.QueryPermission("WORKERFILE.REGISTER", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    HttpContext.Session["CanDismissWorkerFile"] = Permission.QueryPermission("WORKERFILE.DISMISS", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    HttpContext.Session["CanRehireWorkerFile"] = Permission.QueryPermission("WORKERFILE.REHIRE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    HttpContext.Session["CanDeleteWorkerFile"] = Permission.QueryPermission("WORKERFILE.DELETE", long.Parse(HttpContext.Session["UserId"].ToString())) ? true : (object)false;
                    HttpContext.Session["KiFirstHalfYear"] = workerFile.SetKiFirstHalfYear();
                    login.LoadSQLToolsStaticVariables(Session["UserId"].ToString());
                    //emailNotification.SendAnniversaryGiftEmail();
                    //emailNotification.SendLegalRequirementEmail();
                    emailNotification.SendVehicleEmail();

                    return View("~/Areas/YMMHRSystem/Views/WorkerFile/Index.cshtml", workerFile.LoadMultiple());
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}