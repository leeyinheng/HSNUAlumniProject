using System;
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
        public ActionResult Index(string[] agree , string[] answer)
        {
            
            if (agree != null && agree[0] == "on")
            {

               if (answer != null)
                {
                    if (answer[0] == "陳奉瑤")
                    {
                        AssignCookie();

                        return Redirect("CollegeClassmate");
                    }
                    else
                    {
                        try
                        {
                            string classId = answer[0].Substring(0, 3);

                            int id = Convert.ToInt16(classId);

                            if (id >= 618 && id <= 643 && answer[0].ToLower().Contains("ilovehsnu")) 
                            {
                                AssignCookie(classId);

                                return Redirect("Classmate");
                            }
                            else
                            {
                                return Redirect("Home");
                            }
                        }
                        catch (Exception)
                        {
                            return Redirect("Home");
                        }
                    }
                }
                else
                {
                    return Redirect("Home");
                }
             
            }
            else
            {
                return Redirect("Home");
            }
        }


        private void AssignCookie(string classId = "000")
        {
            HttpCookie cookie = new HttpCookie("Agreement");

            cookie.Value = classId +  " | Agreement is accepted! CreatedOn: " + DateTime.Now.ToShortDateString() + " | " + DateTime.Now.ToShortTimeString();

            this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
            
            var operation = new HSNUAlumni.DALLib.AddAgreement();

            operation.Add(new ModelLib.Agreement() { PartitionKey = User.Identity.Name, RowKey = DateTime.Now.ToLongDateString(), ClassId = classId });
   
        }
    }
}