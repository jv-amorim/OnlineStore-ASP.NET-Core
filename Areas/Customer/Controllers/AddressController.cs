using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Repositories.Interfaces;
using OnlineStore.Libraries.Filters;
using OnlineStore.Libraries.Language;
using OnlineStore.Libraries.Session;

namespace OnlineStore.Controllers
{
    [Area("Customer")]
    [CustomerAuthorization]
    public class AddressController : Controller
    {
        private IAddressRepository addressRepository;
        private CustomerSession customerSession;

        public AddressController(IAddressRepository addressRepository, CustomerSession customerSession)
        {
            this.addressRepository = addressRepository;
            this.customerSession = customerSession;
        }
        
        public IActionResult Index() 
        {
            List<Address> addresses = 
                customerSession.GetLoggedInCustomer()
                .Addresses.ToList();

            return View(addresses);
        }
    
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register([FromForm] Address address, string redirectTo)
        {
            if (ModelState.IsValid)
            {
                int customerId = customerSession.GetLoggedInCustomer().Id;

                address.CustomerId = customerId;
                addressRepository.Register(address);

                if (redirectTo != null)
                    return LocalRedirectPermanent(redirectTo);

                TempData["MSG_OK"] = Message.MSG_OK_001;
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}