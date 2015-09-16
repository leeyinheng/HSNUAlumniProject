using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSNUAlumni.Web.Controllers
{
    public class ClassmateController : Controller
    {
        // GET: Classmate
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return Redirect("Home"); 
            }
            
        }
    }
}