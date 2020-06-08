using System.Collections.Generic;
using OnlineStore.Models.ShoppingCart;
using Newtonsoft.Json;

namespace OnlineStore.Libraries.Cookie
{
    public class ShippingInfoCookieManager
    {
        private const string Key = "ShippingInfo";
        private const int CookieLifeTimeInDays = 7;

        private CookieManager cookieManager;
        
        public ShippingInfoCookieManager(CookieManager cookieManager) => this.cookieManager = cookieManager;

        public void SetCookie(List<ShippingInformation> shippingInfos)
        {
            var value = JsonConvert.SerializeObject(shippingInfos);
            cookieManager.SetValue(Key, value, CookieLifeTimeInDays);
        }

        public List<ShippingInformation> GetCookieData()
        {
            string value = cookieManager.GetValue(Key);

            if (value == null)
                return new List<ShippingInformation>();

            return JsonConvert.DeserializeObject<List<ShippingInformation>>(value);
        }

        public void DeleteCookie() => cookieManager.RemoveValue(Key);
    }
}