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
                await antiforgery.ValidateRequestAsync(context);

            await next(context);
        }
    }
}