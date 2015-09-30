using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSNUAlumni.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["Type"] == "Land")
                {
                    return Redirect("CollegeClassmate");
                }
                else
                {
                    return Redirect("Classmate");
                }
              
            }
            else
            {
                return View();
            }
             
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}