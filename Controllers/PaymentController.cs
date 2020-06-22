using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Models.ShoppingCart;
using OnlineStore.Models.Payment;
using OnlineStore.Repositories.Interfaces;
using OnlineStore.Libraries.Cookie;
using OnlineStore.Libraries.Filters;
using OnlineStore.Libraries.Services.Payment;
using OnlineStore.Libraries.Session;

namespace OnlineStore.Controllers
{
    [CustomerAuthorization]
    public class PaymentController : Controller
    {
        private CartCookieManager cartCookieManager;
        private ShippingInfoCookieManager shippingInfoCookieManager;
        private IProductRepository productRepository;
        private IAddressRepository addressRepository;
        private CustomerSession customerSession;
        private PagarMeManager pagarMeManager;

        public PaymentController(
            CartCookieManager cartCookieManager, 
            ShippingInfoCookieManager shippingInfoCookieManager, 
            IProductRepository productRepository, 
            IAddressRepository addressRepository, 
            CustomerSession customerSession,
            PagarMeManager pagarMeManager)
        {
            this.cartCookieManager = cartCookieManager;
            this.shippingInfoCookieManager = shippingInfoCookieManager;
            this.addressRepository = addressRepository;
            this.productRepository = productRepository;
            this.customerSession = customerSession;
            this.pagarMeManager = pagarMeManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<CartItem> cartItems = cartCookieManager.GetCookieData();
            List<ShippingInformation> shippingInfos = shippingInfoCookieManager.GetCookieData();

            if (cartItems.Count == 0 || shippingInfos.Count == 0)
                return RedirectToAction("Index", "Cart");

            ShippingInformation selectedShippingRate = shippingInfos.Find(s => s.IsSelected == true);

            if (selectedShippingRate.SelectedAddressId == 0)
                return RedirectToAction("Index", "Cart");

            foreach (var cartItem in cartItems)
                cartItem.Product = productRepository.GetProduct(cartItem.Id);
            
            ViewData["CartItems"] = cartItems;
            ViewData["ShippingInformation"] = selectedShippingRate;
            ViewData["SelectedAddress"] = addressRepository.GetAddress(selectedShippingRate.SelectedAddressId);

            return View();
        }

        [HttpPost]
        public IActionResult Index([FromForm] CreditCard creditCard)
        {
            if (ModelState.IsValid)
            {
                List<CartItem> cartItems = cartCookieManager.GetCookieData();
                foreach (var cartItem in cartItems)
                    cartItem.Product = productRepository.GetProduct(cartItem.Id);
                    
                Customer loggedInCustomer = customerSession.GetLoggedInCustomer();

                ShippingInformation selectedShippingRate = 
                    shippingInfoCookieManager.GetCookieData()
                    .Find(s => s.IsSelected == true);
                Address selectedAddress = addressRepository.GetAddress(selectedShippingRate.SelectedAddressId);

                dynamic transactionResult = 
                    pagarMeManager.GenerateCreditCardPayment(loggedInCustomer, selectedShippingRate, selectedAddress, cartItems, creditCard);

                // return new ContentResult{ Content = $"OK! Transaction ID = {transactionResult.TransactionId}" };
                return new ContentResult{ Content = "OK!" };
            }

            return Index();
        }
    }
}