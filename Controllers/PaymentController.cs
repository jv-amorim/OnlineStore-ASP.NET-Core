using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models.ShoppingCart;
using OnlineStore.Repositories.Interfaces;
using OnlineStore.Libraries.Cookie;

namespace OnlineStore.Controllers
{
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
            var shippingInfos = shippingInfoCookieManager.GetCookieData();
            
            if (cartItems.Count == 0 || shippingInfos.Count == 0)
                return RedirectToAction("Index", "Cart");

            foreach (var cartItem in cartItems)
                cartItem.Product = productRepository.GetProduct(cartItem.Id);

            ViewData["ShippingInformation"] = shippingInfos.Find(s => s.IsSelected == true);

            return View(cartItems);
        }
    }
}