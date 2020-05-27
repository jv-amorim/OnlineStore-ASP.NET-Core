using System.Collections.Generic;
using System.Linq;
using OnlineStore.Models.Cookies;
using Newtonsoft.Json;

namespace OnlineStore.Libraries.Cookie
{
    public class CartCookieManager
    {
        private const string Key = "CartCookie";
        private const int CookieLifeTimeInDays = 7;

        private CookieManager cookieManager;
        
        public CartCookieManager(CookieManager cookieManager) => this.cookieManager = cookieManager;

        public void SaveCookieData(List<CartItem> cookieData)
        {
            var value = JsonConvert.SerializeObject(cookieData);
            cookieManager.SetValue(Key, value, CookieLifeTimeInDays);
        }

        public List<CartItem> GetCookieData()
        {
            string value = cookieManager.GetValue(Key);

            if (value == null)
                return new List<CartItem>();

            return JsonConvert.DeserializeObject<List<CartItem>>(value);
        }

        public void AddCartItemToCookie(CartItem cartItem)
        {
            List<CartItem> cookieData = GetCookieData();
            CartItem cartItemInCookie = cookieData.SingleOrDefault(c => c.Id == cartItem.Id);

            if (cartItemInCookie != null)
            {
                cookieData.Add(cartItem);
                SaveCookieData(cookieData);
            }
            else
            {
                cartItem.Amount++;
                UpdateCartItemInCookie(cartItem); 
            }
        }

        public void UpdateCartItemInCookie(CartItem cartItem)
        {
            List<CartItem> cookieData = GetCookieData();
            CartItem cartItemInCookie = cookieData.SingleOrDefault(c => c.Id == cartItem.Id);

            if (cartItemInCookie != null)
            {
                cartItemInCookie.Amount = cartItem.Amount;
                SaveCookieData(cookieData);
            }
            else
                AddCartItemToCookie(cartItem);
        }

        public void DeleteCartItemFromCookie(CartItem cartItem)
        {
            List<CartItem> cookieData = GetCookieData();
            CartItem cartItemInCookie = cookieData.SingleOrDefault(c => c.Id == cartItem.Id);

            if (cartItemInCookie != null)
            {
                cookieData.Remove(cartItemInCookie);
                SaveCookieData(cookieData);
            }
        }

        public void DeleteAllCartItemsFromCookie() => cookieManager.RemoveValue(Key);
    }
}