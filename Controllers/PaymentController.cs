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
            foreach (var cartItem in cartItems)
                cartItem.Product = productRepository.GetProduct(cartItem.Id);
            
            // var shippingInfos = shippingInfoCookieManager.GetCookieData();

            return View(cartItems);
        }
    }
}