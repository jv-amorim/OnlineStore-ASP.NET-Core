using System;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Repositories.Interfaces;
using OnlineStore.Libraries.Email;
using OnlineStore.Libraries.LogResources;
using OnlineStore.Libraries.Language;
using OnlineStore.Libraries.Session;
using OnlineStore.Libraries.Filters;

namespace OnlineStore.Controllers
{
    public class HomeController : Controller
    {
        private ICustomerRepository customerRepository;
        private INewsletterRepository newsletterRepository;
        private CustomerSession customerSession;

        public HomeController(ICustomerRepository customerRepository, INewsletterRepository newsletterRepository, CustomerSession customerSession)
        {
            this.customerRepository = customerRepository;
            this.newsletterRepository = newsletterRepository;
            this.customerSession = customerSession;
        }

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

        [HttpGet]
        public IActionResult SignUp() => View();

        [HttpPost]
        public IActionResult SignUp([FromForm] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customerRepository.Register(customer);

                TempData["MSG_OK"] = Message.MSG_OK_001;

                // TODO - Redirect to other pages, depending on the situation (Login, Panel, Cart, etc.).
                return RedirectToAction(nameof(SignUp));
            }

            return View();
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login([FromForm] Customer customer)
        {
            Customer customerFromDB = customerRepository.Login(customer.Email, customer.Password);

            if (customerFromDB == null)
            {
                ViewData["MSG_ERROR"] = Message.MSG_ERROR_006;
                return View();
            }
            
            customerSession.Login(customerFromDB);
            return RedirectToAction(nameof(CustomerPanel));
        }

        [HttpGet]
        [CustomerAuthorization]
        public IActionResult CustomerPanel() => new ContentResult() { Content = "Customer Panel." };
    }
}