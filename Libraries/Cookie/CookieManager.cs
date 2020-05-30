using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace OnlineStore.Libraries.Cookie
{
    public class CookieManager
    {
        private IHttpContextAccessor httpContextAccessor;
        
        public CookieManager(IHttpContextAccessor httpContextAccessor) => 
            this.httpContextAccessor = httpContextAccessor;

        public void SetValue(string key, string value, int lifeTimeInDays)
        {
            CookieOptions cookieOptions= new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddDays(lifeTimeInDays);
            cookieOptions.IsEssential = true;
            httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, cookieOptions);
        }

        public string GetValue(string key)
        {   
            if (IsTheValueExisting(key))
                return httpContextAccessor.HttpContext.Request.Cookies[key];
            else
                return null;
        }

        public bool IsTheValueExisting(string key)
        {
            if (httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(key))
                return true;         
            return false;
        }

        public void RemoveValue(string key)
        {
            if (IsTheValueExisting(key))
                httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
        }

        public void RemoveAllValues()
        {
            var cookiesList = httpContextAccessor.HttpContext.Request.Cookies.ToList();
            foreach (var cookie in cookiesList)
                RemoveValue(cookie.Key);
        }
    }
}