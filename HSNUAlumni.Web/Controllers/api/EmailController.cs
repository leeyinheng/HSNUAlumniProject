using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HSNUAlumni.ModelLib;
using SendGrid;
using System.Net.Mail;
 


namespace HSNUAlumni.Web.Controllers.api
{
    public class EmailController : ApiController
    {
        private readonly DALLib.CloudTableOperation<Classmate> op = new DALLib.CloudTableOperation<Classmate>("classmates");

        public void Send(EmailModel value)
        {
            Helper.Guard.New().IsAuthentic();

            // Create the email object first, then add the properties.
            SendGridMessage emailMessage = new SendGridMessage();
            emailMessage.To = GetEmailList();  
            emailMessage.From = new MailAddress(value.FromAddress, value.Name);
            emailMessage.Subject = "Message from 通訊錄系統: " + value.Title;
            emailMessage.Html = value.Message;
                       
            //web api key 
            var transportWeb = new SendGrid.Web(System.Configuration.ConfigurationManager.AppSettings["SendGridApiKey"]);

            // Send the email.
            transportWeb.DeliverAsync(emailMessage);

        }

        private System.Net.Mail.MailAddress[] GetEmailList()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["Type"] == "Land")
            {

            }
            else
            {

            }
                  
            return new MailAddress[] { new MailAddress("leeyinheng@gmail.com", "henry") , new MailAddress("leeyinheng@yahoo.com","test2") }; 

        }

    }
}
