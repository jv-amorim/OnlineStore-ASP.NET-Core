using Microsoft.AspNetCore.Http;

namespace OnlineStore.Libraries.Session
{
    public class SessionManager
    {
        private IHttpContextAccessor httpContextAccessor;
        
        public SessionManager(IHttpContextAccessor httpContextAccessor) => 
            this.httpContextAccessor = httpContextAccessor;

        public void SetValue(string key, string value)
        {
            if (IsTheValueExisting(key))
                UpdateValue(key, value);
            else
                httpContextAccessor.HttpContext.Session.SetString(key, value);   
        }

        public void UpdateValue(string key, string value)
        {
            RemoveValue(key);
            SetValue(key, value);
        }

        public string GetValue(string key)
        {   
            if (IsTheValueExisting(key))
                return httpContextAccessor.HttpContext.Session.GetString(key);
            else
                return null;
        }

        public bool IsTheValueExisting(string key)
        {
            if (httpContextAccessor.HttpContext.Session.GetString(key) == null)
                return false;         
            return true;
        }

        public void RemoveValue(string key)
        {
            if (IsTheValueExisting(key))
                httpContextAccessor.HttpContext.Session.Remove(key);
        }

        public void RemoveAllValues() => httpContextAccessor.HttpContext.Session.Clear();
    }
}