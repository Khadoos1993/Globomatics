using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globomantics.Middleware
{
    public class MyHttpHandlerMiddleware
    {
        private RequestDelegate _next;
        private IOptions<AppSettings> _appSettings;

        public MyHttpHandlerMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings;
        }

        public async Task Invoke(HttpContext context)
        {
            await context.Response.WriteAsync("My Http Handler Middleware"+ _appSettings.Value.ActiveTheme);
        }
    }

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMyHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyHttpHandlerMiddleware>();
        }
    }
}
