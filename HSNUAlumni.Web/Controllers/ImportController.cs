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
            return View();
        }


        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            string path = string.Empty; 

            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                file.SaveAs(path);
            }

            var op = new HSNUAlumni.DALLib.CloudTableOperation<HSNUAlumni.ModelLib.Classmate>("classmate");

              op.ImportData(path).ContinueWith(o=>
              {
                  if (o.IsFaulted)
                  {
                      throw new Exception(o.Exception.Message); 
                  }

                  ViewBag.Message = "Importing process is completed !";

                  return View(); 
              }); 

            return View(); 
        }
    }
}