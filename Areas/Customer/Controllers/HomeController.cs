using Microsoft.AspNetCore.Mvc;
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
        private CustomerSession customerSession;

        public HomeController(ICustomerRepository customerRepository, CustomerSession customerSession)
        {
            this.customerRepository = customerRepository;
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
                customerRepository.Register(customer);
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