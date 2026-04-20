using G3SharedKernel.Middleware;
using Microsoft.AspNetCore.Builder;

namespace G3SharedKernel.Extensions
{
    public static class GR3MiddlewareExtensions
    {
        public static IApplicationBuilder UseGR3ApiKeyMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GR3ApiKeyMiddleware>();
        }

        public static IApplicationBuilder UseGR3GlobalExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GR3GlobalExceptionMiddleware>();
        }
    }
}