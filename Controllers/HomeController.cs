using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Models.ViewModels;
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
        private IProductRepository productRepository;
        private CustomerSession customerSession;
        private const int NumberOfItemsPerPage = 12;

        public HomeController(ICustomerRepository customerRepository, INewsletterRepository newsletterRepository, 
            CustomerSession customerSession, IProductRepository productRepository)
        {
            this.customerRepository = customerRepository;
            this.newsletterRepository = newsletterRepository;
            this.customerSession = customerSession;
            this.productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult Index(int? page, string searchParameter, string sortingOption)
        {
            IndexViewModel indexViewModel = new IndexViewModel()
            {
                Products = productRepository.GetAllProducts(page, NumberOfItemsPerPage, searchParameter, sortingOption)
            };

            return View(indexViewModel);
        }

        [HttpPost]
        public IActionResult Index(int? page, string searchParameter, string sortingOption, [FromForm]IndexViewModel indexViewModel)
        {
            if (ModelState.IsValid)
            {
                newsletterRepository.Subscribe(indexViewModel.NewsletterEmail);

                TempData["MSG_OK"] = 
                    "Your email has been successfully registered to the newsletter!";

                return RedirectToAction(nameof(Index));
            }
            
            indexViewModel = new IndexViewModel()
            {
                Products = productRepository.GetAllProducts(page, NumberOfItemsPerPage, searchParameter, sortingOption)
            };

            return View(indexViewModel);
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
                ViewData["MSG_ERROR"] = Message.MSG_ERROR_007;
            
                LogWriter.WriteNewDataInLogFile("./Logs/ContactErrorLog.log", e.Message);
            }
            
            return View("Contact");
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

        public IActionResult Cart() => View();
    }
}