using System;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Repositories.Interfaces;
using OnlineStore.Libraries.Helpers.EmailHelpers;
using OnlineStore.Libraries.Helpers.LogHelpers;
using OnlineStore.Libraries.Language;

namespace OnlineStore.Controllers
{
    public class HomeController : Controller
    {
        private INewsletterRepository newsletterRepository;

        public HomeController(INewsletterRepository newsletterRepository) => this.newsletterRepository = newsletterRepository;

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Index([FromForm]NewsletterEmail newsletterEmail)
        {
            if (ModelState.IsValid)
            {
                newsletterRepository.Subscribe(newsletterEmail);

                TempData["MSG_OK"] = 
                    "Your email has been successfully registered to the newsletter!";

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]
        public IActionResult Contact() => View();

        [HttpPost]
        public IActionResult Contact([FromForm]Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    EmailContact.SendContactMessageByEmail(contact);
                    TempData["MSG_OK"] = "Your contact message has been successfully sent!";
                    return RedirectToAction(nameof(Contact));
                }
                catch (Exception e)
                {
                    TempData["MSG_ERROR"] = Message.MSG_ERROR_007;
                    LogWriter.WriteNewDataInLogFile("./Logs/ContactErrorLog.log", e.Message);
                }
            }

            return View();
        }
    }
}