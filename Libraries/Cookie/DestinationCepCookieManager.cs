using System.Collections.Generic;
using OnlineStore.Models.ShoppingCart;

namespace OnlineStore.Libraries.Cookie
{
    public class DestinationCepCookieManager
    {
        private const string Key = "ShippingInfo";
        private const int CookieLifeTimeInDays = 7;

        private CookieManager cookieManager;
        
        public DestinationCepCookieManager(CookieManager cookieManager) => this.cookieManager = cookieManager;

        public void SetCookie(string destinationCep) => cookieManager.SetValue(Key, destinationCep, CookieLifeTimeInDays);

        public string GetCookieData() => cookieManager.GetValue(Key);

        public void DeleteCookie() => cookieManager.RemoveValue(Key);
    }
}