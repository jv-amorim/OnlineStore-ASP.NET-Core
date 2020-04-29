using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Libraries.Email;

namespace OnlineStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult ContactAction()
        {
            try
            {
                Contact newContact = new Contact();
                newContact.Name = HttpContext.Request.Form["username"];
                newContact.Email = HttpContext.Request.Form["email"];
                newContact.Text = HttpContext.Request.Form["text"];
                
                var listOfMessages = new List<ValidationResult>();
                var context = new ValidationContext(newContact);
                bool isValid = Validator.TryValidateObject(newContact, context, listOfMessages, true);

                if (isValid)
                {
                    EmailContact.SendContactMessageByEmail(newContact);
                    
                    ViewData["MSG_OK"] = "Your contact message has been successfully sent!";
                }
                else
                {
                    StringBuilder completeErrorMessage = new StringBuilder();

                    foreach (var text in listOfMessages)
                        completeErrorMessage.Append(text.ErrorMessage + "<br/>");

                    ViewData["MSG_ERROR"] = completeErrorMessage.ToString();
                    ViewData["CONTACT"] = newContact;
                }
            }
            catch (Exception e)
            {
                ViewData["MSG_ERROR"] = "Oops, something went wrong. Try again!";
            
                OnlineStore.Libraries.LogSystem.LogWriter.WriteNewDataInLogFile("./Logs/ContactErrorLog.log", e.Message);
            }
            
            return View("Contact");
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult Cart()
        {
            return View();
        }
    }
}