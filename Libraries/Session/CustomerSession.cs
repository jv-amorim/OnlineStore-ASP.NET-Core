using OnlineStore.Models;
using Newtonsoft.Json;

namespace OnlineStore.Libraries.Session
{
    public class CustomerSession
    {
        private const string Key = "CustomerLogin";
        private SessionManager sessionManager;

        public CustomerSession(SessionManager sessionManager) => this.sessionManager = sessionManager;

        public void Login(Customer customer) => sessionManager.SetValue(Key, JsonConvert.SerializeObject(customer));

        public Customer GetLoggedInCustomer()
        {
            string customer = sessionManager.GetValue(Key);

            if (customer == null)
                return null;
            
            return JsonConvert.DeserializeObject<Customer>(customer);
        }

        public void Logout() => sessionManager.RemoveAllValues();
    }
}