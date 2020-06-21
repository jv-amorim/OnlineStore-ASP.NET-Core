using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models.ShoppingCart;
using OnlineStore.Repositories.Interfaces;
using OnlineStore.Libraries.Cookie;
using OnlineStore.Libraries.Filters;

namespace OnlineStore.Controllers
{
    [CustomerAuthorization]
    public class PaymentController : Controller
    {
        private CartCookieManager cartCookieManager;
        private ShippingInfoCookieManager shippingInfoCookieManager;
        private IProductRepository productRepository;
        private IAddressRepository addressRepository;

        public PaymentController(
            CartCookieManager cartCookieManager, 
            ShippingInfoCookieManager shippingInfoCookieManager, 
            IProductRepository productRepository, 
            IAddressRepository addressRepository)
        {
            this.cartCookieManager = cartCookieManager;
            this.shippingInfoCookieManager = shippingInfoCookieManager;
            this.productRepository = productRepository;
            this.addressRepository = addressRepository;
        }

        public IActionResult Index()
        {
            List<CartItem> cartItems = cartCookieManager.GetCookieData();
            List<ShippingInformation> shippingInfos = shippingInfoCookieManager.GetCookieData();
            
            if (cartItems.Count == 0 || shippingInfos.Count == 0)
                return RedirectToAction("Index", "Cart");

            ShippingInformation selectedShippingRate = shippingInfos.Find(s => s.IsSelected == true);

            if (selectedShippingRate.SelectedAddressId == 0)
                return RedirectToAction("Index", "Cart");

            ViewData["ShippingInformation"] = selectedShippingRate;
            ViewData["SelectedAddress"] = addressRepository.GetAddress(selectedShippingRate.SelectedAddressId);

            foreach (var cartItem in cartItems)
                cartItem.Product = productRepository.GetProduct(cartItem.Id);

            return View(cartItems);
        }
    }
}