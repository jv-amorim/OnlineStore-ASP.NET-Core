using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Models.ShoppingCart;
using OnlineStore.Repositories.Interfaces;
using OnlineStore.Libraries.Cookie;
using OnlineStore.Libraries.Language;

namespace OnlineStore.Controllers
{
    public class CartController : Controller
    {
        private CartCookieManager cartCookieManager;
        private IProductRepository productRepository;

        public CartController(CartCookieManager cartCookieManager, IProductRepository productRepository)
        {
            this.cartCookieManager = cartCookieManager;
            this.productRepository = productRepository;
        }

        public IActionResult Index()
        {
            List<CartItem> cartItems = cartCookieManager.GetCookieData();

            foreach (var cartItem in cartItems)
                cartItem.Product = productRepository.GetProduct(cartItem.Id);

            return View(cartItems);
        }

        public IActionResult AddProductToTheCart(int id)
        {
            Product product = productRepository.GetProduct(id);

            if (product == null)
            {
                ViewData["MSG_ERROR"] = Message.MSG_ERROR_011;
                return View();
            }

            if (product.UnitsInStock < 1)
            {
                ViewData["MSG_ERROR"] = Message.MSG_ERROR_012;
                return View();
            }

            cartCookieManager.IncrementTheCartItemAmountInCookie(new CartItem{ Id = id });
            return RedirectToAction(nameof(Index));
        }

        public IActionResult UpdateProductInCart(int id, int amount)
        {
            cartCookieManager.UpdateCartItemInCookie(new CartItem{ Id = id, Amount = amount });
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveProductFromCart(int id)
        {
            cartCookieManager.DeleteCartItemFromCookie(new CartItem{ Id = id });
            return RedirectToAction(nameof(Index));
        }
    }
}