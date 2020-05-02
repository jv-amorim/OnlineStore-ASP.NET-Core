using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Libraries.Email;
using OnlineStore.Libraries.LogSystem;
using OnlineStore.Repositories.Interfaces;

namespace OnlineStore.Controllers
{
    public class HomeController : Controller
    {
        private ICustomerRepository customerRepository;
        private INewsletterRepository newsletterRepository;

        public HomeController(ICustomerRepository customerRepository, INewsletterRepository newsletterRepository)
        {
            this.customerRepository = customerRepository;
            this.newsletterRepository = newsletterRepository;
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Index([FromForm]NewsletterEmail newsletter)
        {
            if( ModelState.IsValid)
            {
                newsletterRepository.Subscribe(newsletter);

                TempData["MSG_OK"] = 
                    "Your email has been successfully registered to the newsletter!";

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        public IActionResult Contact() => View();

        public IActionResult ContactAction()
        {
            try
            {
                Contact newContact = new Contact()
                {
                    Name = HttpContext.Request.Form["username"],
                    Email = HttpContext.Request.Form["email"],
                    Text = HttpContext.Request.Form["text"]
                };
                
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
            
                LogWriter.WriteNewDataInLogFile("./Logs/ContactErrorLog.log", e.Message);
            }
            
            return View("Contact");
        }

        public IActionResult Login() => View();

        [HttpGet]
        public IActionResult SignUp() => View();

        [HttpPost]
        public IActionResult SignUp([FromForm] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customerRepository.SignUp(customer);

                TempData["MSG_OK"] = "Successful registration!";

                // TODO - Redirect to other pages, depending on the situation (Login, Panel, Cart, etc.).
                return RedirectToAction(nameof(SignUp));
            }

            return View();
        }

        public IActionResult Cart() => View();
    }
}