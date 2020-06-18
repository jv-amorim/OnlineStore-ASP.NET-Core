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

        public PaymentController(CartCookieManager cartCookieManager, ShippingInfoCookieManager shippingInfoCookieManager, IProductRepository productRepository)
        {
            this.cartCookieManager = cartCookieManager;
            this.shippingInfoCookieManager = shippingInfoCookieManager;
            this.productRepository = productRepository;
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

            foreach (var cartItem in cartItems)
                cartItem.Product = productRepository.GetProduct(cartItem.Id);

            return View(cartItems);
        }
    }
}