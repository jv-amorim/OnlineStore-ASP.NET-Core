using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Antiforgery;

namespace OnlineStore.Libraries.Middlewares
{
    public class ValidateAntiForgeryTokenMiddleware
    {
        private RequestDelegate next;
        private IAntiforgery antiforgery;

        public ValidateAntiForgeryTokenMiddleware(RequestDelegate next, IAntiforgery antiforgery)
        {
            this.next = next;
            this.antiforgery = antiforgery;
        }

        public async Task Invoke(HttpContext context)
        {
            if (HttpMethods.IsPost(context.Request.Method))
            {
                bool isTheRequestMadeWithAjax = context.Request.Headers["x-requested-with"] == "XMLHttpRequest"; 
                bool isTheRequestAFileUpload = isTheRequestMadeWithAjax && context.Request.Form.Files.Count > 0;

                if (!isTheRequestAFileUpload)
                    await antiforgery.ValidateRequestAsync(context);
            }

            await next(context);
        }
    }
}