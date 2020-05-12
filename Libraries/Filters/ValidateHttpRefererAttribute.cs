using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OnlineStore.Libraries.Filters
{
    public class ValidateHttpRefererAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            string referer = context.HttpContext.Request.Headers["Referer"];

            if (string.IsNullOrEmpty(referer))
                context.Result = new ContentResult() { Content = "Access denied." };
            else
            {
                Uri refererUri = new Uri(referer);
                
                string refererHost = refererUri.Host;
                string serverHost = context.HttpContext.Request.Host.Host;

                if (refererHost != serverHost)
                    context.Result = new ContentResult() { Content = "Access denied." };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}