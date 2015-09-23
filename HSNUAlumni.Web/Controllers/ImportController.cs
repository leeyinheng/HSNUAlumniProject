using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSNUAlumni.Web.Controllers
{
    public class ImportController : Controller
    {
        // GET: Import
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


        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            string path = string.Empty;

            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                    file.SaveAs(path);

                    var op = new HSNUAlumni.DALLib.ImportClassmatesFromFile();

                    op.ImportData(path, this.User.Identity.Name);

                    return View(model: "Completed!");
                }
                else
                {
                    return View(model: "No File Selected!");
                }
            }
            catch (Exception ex)
            {

                return View(model: "Error: " + ex.Message); 
            }

          

           


        }

         
    }
}