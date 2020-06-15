using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Repositories.Interfaces;
using OnlineStore.Libraries.Filters;
using OnlineStore.Libraries.Language;
using OnlineStore.Libraries.Session;

namespace OnlineStore.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private ICustomerRepository customerRepository;
        private IAddressRepository addressRepository;
        private CustomerSession customerSession;

        public HomeController(ICustomerRepository customerRepository, IAddressRepository addressRepository, CustomerSession customerSession)
        {
            this.customerRepository = customerRepository;
            this.addressRepository = addressRepository;
            this.customerSession = customerSession;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login([FromForm] Models.Customer customer, string redirectTo)
        {
            Models.Customer customerFromDB = customerRepository.Login(customer.Email, customer.Password);

            if (customerFromDB == null)
            {
                ViewData["MSG_ERROR"] = Message.MSG_ERROR_006;
                return View();
            }

            customerSession.Login(customerFromDB);

            if (redirectTo != null)
                return LocalRedirectPermanent(redirectTo);

            return RedirectToAction(nameof(CustomerPanel));
        }

        [HttpGet]
        public IActionResult SignUp() => View();

        [HttpPost]
        public IActionResult SignUp([FromForm] Models.Customer customer, string redirectTo)
        {
            if (ModelState.IsValid)
            {
                customer.CPF = customer.CPF.RemoveMask();
                customerRepository.Register(customer);

                bool theAddressFormHaveBeenFilled = HttpContext.Request.Form["address-save-status"].ToString() == "true";

                if (theAddressFormHaveBeenFilled)
                {
                    Address newAddress = new Address()
                    {
                        CustomerId = customer.Id,
                        Cep = HttpContext.Request.Form["cep"].ToString(),
                        State = HttpContext.Request.Form["state"].ToString(),
                        City = HttpContext.Request.Form["city"].ToString(),
                        Neighborhood = HttpContext.Request.Form["neighborhood"].ToString(),
                        AddressLine = HttpContext.Request.Form["address-line"].ToString(),
                        Complement =  HttpContext.Request.Form["complement"].ToString(),
                        Number =  HttpContext.Request.Form["number"].ToString()
                    };

                    addressRepository.Register(newAddress);
                }

                TempData["MSG_OK"] = Message.MSG_OK_001;
                return RedirectToAction(nameof(Login), new { redirectTo = redirectTo });
            }

            return View();
        }

        [HttpGet]
        [CustomerAuthorization]
        public IActionResult CustomerPanel() => new ContentResult() { Content = "Customer Panel." };
    }
}