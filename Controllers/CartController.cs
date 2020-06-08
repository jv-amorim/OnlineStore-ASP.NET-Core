using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Models.ShoppingCart;
using OnlineStore.Models.Consts;
using OnlineStore.Repositories.Interfaces;
using OnlineStore.Libraries.Cookie;
using OnlineStore.Libraries.Language;
using OnlineStore.Libraries.Services.Shipping;

namespace OnlineStore.Controllers
{
    public class CartController : Controller
    {
        private readonly string shippingOriginCep = "39404018";

        private CartCookieManager cartCookieManager;
        private ShippingInfoCookieManager shippingInfoCookieManager;
        private IProductRepository productRepository;
        private ShippingPackageFactory shippingPackageFactory;
        private ShippingRateCalculator shippingRateCalculator;

        public CartController(
            CartCookieManager cartCookieManager, 
            ShippingInfoCookieManager shippingInfoCookieManager,
            IProductRepository productRepository, 
            ShippingRateCalculator shippingRateCalculator, 
            ShippingPackageFactory shippingPackageFactory)
        {
            this.cartCookieManager = cartCookieManager;
            this.shippingInfoCookieManager = shippingInfoCookieManager;
            this.productRepository = productRepository;
            this.shippingRateCalculator = shippingRateCalculator;
            this.shippingPackageFactory = shippingPackageFactory;
        }

        public IActionResult Index()
        {
            List<CartItem> cartItems = cartCookieManager.GetCookieData();
            foreach (var cartItem in cartItems)
                cartItem.Product = productRepository.GetProduct(cartItem.Id);

            var shippingInfos = shippingInfoCookieManager.GetCookieData();
            if (shippingInfos.Count > 0)
                ViewData["DestinationCep"] = shippingInfos[0].DestinationCep;
            
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
            Product product = productRepository.GetProduct(id);

            if (product == null)
                return BadRequest(new { message = Message.MSG_ERROR_011 });

            if (amount > product.UnitsInStock)
                return BadRequest(new { message = Message.MSG_ERROR_012 });

            cartCookieManager.UpdateCartItemInCookie(new CartItem{ Id = id, Amount = amount });
            return Ok();
        }

        public IActionResult RemoveProductFromCart(int id)
        {   
            cartCookieManager.DeleteCartItemFromCookie(new CartItem{ Id = id });
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CalculateShippingRate(string destinationCep)
        {
            List<CartItem> cartItems = cartCookieManager.GetCookieData();
            foreach (var cartItem in cartItems)
                cartItem.Product = productRepository.GetProduct(cartItem.Id); 
            var packages = shippingPackageFactory.CreateShippingPackages(cartItems);

            var shippingInfoPAC = 
                await shippingRateCalculator.CalculateShippingRateAndETA(shippingOriginCep, destinationCep, FreightTypes.PAC, packages);
            var shippingInfoSEDEX = 
                await shippingRateCalculator.CalculateShippingRateAndETA(shippingOriginCep, destinationCep, FreightTypes.SEDEX, packages);

            if (shippingInfoPAC == null || shippingInfoSEDEX == null)
            {
                shippingInfoCookieManager.DeleteCookie();
                return BadRequest();
            }

            List<ShippingInformation> shippingInfos = new List<ShippingInformation>()
            {
                shippingInfoPAC,
                shippingInfoSEDEX
            };

            shippingInfoCookieManager.SetCookie(shippingInfos);
            
            return Ok(shippingInfos);
        }
    }
}