using Ambev.DeveloperEvaluation.WebApi.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Ambev.DeveloperEvaluation.WebApi.Extensions
{
    public static class ErrorHandlingExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
} 