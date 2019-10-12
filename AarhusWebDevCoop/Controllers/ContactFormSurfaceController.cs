using AarhusWebDevCoop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;

namespace AarhusWebDevCoop.Controllers
{
    public class ContactFormSurfaceController : SurfaceController
    {
        // GET: Default
        public ActionResult Index()
        {
            return PartialView("ContactForm", new ContactForm());
        }

        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            // Get the GuidUdi of the current page
            GuidUdi currentPageUdi = new GuidUdi(CurrentPage.ContentType.ItemType.ToString(), CurrentPage.Key);

            // Create the new document of 'Message'
            IContent message = Services.ContentService.Create(model.Subject, currentPageUdi.Guid, "message");
            message.SetValue("messageName", model.Name);
            message.SetValue("email", model.Email);
            message.SetValue("subject", model.Subject);
            message.SetValue("messageContent", model.Message);
            message.SetValue("umbracoNaviHide", true);

            // Save
            Services.ContentService.Save(message);

            // Would send an email, but I do not want to save credentials here :P
            //MailMessage message = new MailMessage();
            //message.To.Add("username@eaaa.dk");
            //message.Subject = model.Subject;
            //message.From = new MailAddress(model.Email, model.Name);
            //message.Body = model.Message;
            //using (SmtpClient smtp = new SmtpClient())
            //{
            //    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //    smtp.UseDefaultCredentials = false;
            //    smtp.EnableSsl = true;
            //    smtp.Host = "smtp.gmail.com";
            //    smtp.Port = 587;
            //    smtp.Credentials = new System.Net.NetworkCredential("username@gmail.com", "password");

            //    // send mail
            //    smtp.Send(message);
            //}

            TempData["success"] = true;
            return RedirectToCurrentUmbracoPage();
        }
    }
}