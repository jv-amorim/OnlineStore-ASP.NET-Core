using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Libraries.Cookie;
using OnlineStore.Libraries.Filters;
using OnlineStore.Libraries.Session;

namespace OnlineStore.Controllers
{
    [CustomerAuthorization]
    public class AddressSelection : Controller
    {
        private CustomerSession customerSession;
        private CartCookieManager cartCookieManager;
        private ShippingInfoCookieManager shippingInfoCookieManager;

        public AddressSelection(
            CustomerSession customerSession,
            CartCookieManager cartCookieManager, 
            ShippingInfoCookieManager shippingInfoCookieManager)
        {
            this.customerSession = customerSession;
            this.cartCookieManager = cartCookieManager;
            this.shippingInfoCookieManager = shippingInfoCookieManager;
        }

        public IActionResult Index()
        {
            var cartItems = cartCookieManager.GetCookieData();
            var shippingInfos = shippingInfoCookieManager.GetCookieData();
            
            if (cartItems.Count == 0 || shippingInfos.Count == 0)
                return RedirectToAction("Index", "Cart");

            ViewData["DestinationCep"] = shippingInfos[0].DestinationCep;

            List<Address> addresses = customerSession.GetLoggedInCustomer().Addresses.ToList();
            
            return View(addresses);
        }

        public IActionResult SelectAddress(int addressId)
        {
            var shippingInfos = shippingInfoCookieManager.GetCookieData();

            shippingInfos.First(s => s.IsSelected == true).SelectedAddressId = addressId;
            shippingInfoCookieManager.SetCookie(shippingInfos);

            return Ok();
        }
    }
}