using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSNUAlumni.Web.Controllers
{
    public class CollegeClassmateController : Controller
    {
        // GET: CollegeClassmate
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated && System.Web.Configuration.WebConfigurationManager.AppSettings["Type"] == "Land")
            {
                if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("Agreement"))
                {
                    return View();
                }
                else
                {
                    return Redirect("Agreement");
                }
            }
            else
            {
                return Redirect("Home");
            }
        }
    }
}