﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
 

namespace HSNUAlumni.Web.Controllers
{
    public class AgreementController : Controller
    {
        // GET: Agreement
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
        public ActionResult Index(string[] agree)
        {
            
            if (agree != null && agree[0] == "on")
            {
                var operation = new HSNUAlumni.DALLib.AddAgreement();

                operation.Add(new ModelLib.Agreement() { PartitionKey = User.Identity.Name, RowKey = DateTime.Now.ToLongDateString() }); 

                HttpCookie cookie = new HttpCookie("Agreement");

                cookie.Value = "Agreement is accepted! CreatedOn: " + DateTime.Now.ToShortDateString() + " | " + DateTime.Now.ToShortTimeString();

                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);

                return Redirect("Classmate");
            }
            else
            {
                return Redirect("Home");
            }
        }
    }
}