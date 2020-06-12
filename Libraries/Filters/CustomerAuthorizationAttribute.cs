using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineStore.Models;
using OnlineStore.Libraries.Session;

namespace OnlineStore.Libraries.Filters
{
    public class CustomerAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private CustomerSession customerSession;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            customerSession = 
                (CustomerSession)context.HttpContext.RequestServices.GetService(typeof(CustomerSession));
            
            Customer customerFromSession = customerSession.GetLoggedInCustomer();

            if (customerFromSession == null)
                context.Result = new RedirectToActionResult("Login", "Home", new { area = "Customer" });
        }
    }
}