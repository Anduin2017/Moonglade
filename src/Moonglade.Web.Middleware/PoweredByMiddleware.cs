using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Moonglade.Web.Middleware
{
    public class PoweredByMiddleware
    {
        private readonly RequestDelegate _next;

        public PoweredByMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.Headers["X-Powered-By"] = "Moonglade";
            return _next.Invoke(httpContext);
        }
    }
}
