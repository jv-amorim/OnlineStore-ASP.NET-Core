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
                Customer loggedInCustomer = customerSession.GetLoggedInCustomer();

                address.CustomerId = loggedInCustomer.Id;
                addressRepository.Register(address);

                loggedInCustomer.Addresses.Add(address);
                customerSession.Login(loggedInCustomer);

                if (redirectTo != null)
                    return LocalRedirectPermanent(redirectTo);

                TempData["MSG_OK"] = Message.MSG_OK_001;
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        public IActionResult Update(int id)
        {
            Address addressToUpdate = addressRepository.GetAddress(id);

            Customer loggedInCustomer = customerSession.GetLoggedInCustomer();

            if (addressToUpdate.CustomerId != loggedInCustomer.Id)
                return RedirectToAction(nameof(Index));

            return View(addressToUpdate);
        }

        [HttpPost]
        public IActionResult Update([FromForm] Address address)
        {
            Customer loggedInCustomer = customerSession.GetLoggedInCustomer();

            if (address.CustomerId != loggedInCustomer.Id)
                return RedirectToAction(nameof(Index));

            if (ModelState.IsValid)
            {
                addressRepository.Update(address);

                Address addressInCustomerSession = loggedInCustomer.Addresses.First(a => a.Id == address.Id);  
                loggedInCustomer.Addresses.Remove(addressInCustomerSession);
                loggedInCustomer.Addresses.Add(address);

                customerSession.Login(loggedInCustomer);

                TempData["MSG_OK"] = Message.MSG_OK_002;
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [ValidateHttpReferer]
        public IActionResult Delete(int id)
        {
            Customer loggedInCustomer = customerSession.GetLoggedInCustomer();
            
            Address addressToDelete = addressRepository.GetAddress(id);
            if (addressToDelete.CustomerId != loggedInCustomer.Id)
                return RedirectToAction(nameof(Index));

            Address addressInCustomerSession = loggedInCustomer.Addresses.First(a => a.Id == id);
            loggedInCustomer.Addresses.Remove(addressInCustomerSession);
            customerSession.Login(loggedInCustomer);

            addressRepository.Delete(id);

            TempData["MSG_OK"] = Message.MSG_OK_003;
            return RedirectToAction(nameof(Index));
        }
    }
}