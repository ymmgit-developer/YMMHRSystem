using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YMMChildPartsReceptionSystem.Controllers.Error
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index(int error=0)
        {
            switch (error)
            {
                case 505:
                    ViewBag.Error = 500;
                    break;

                case 404:
                    ViewBag.Error = 404;
                    break;

                default:
                    ViewBag.Error = 500;
                    break;
            }

            return View("~/Views/Error/_ErrorPage.cshtml");
        }

    }
}
