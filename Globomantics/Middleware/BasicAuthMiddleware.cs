using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globomantics.Middleware
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;
        public BasicAuthMiddleware(RequestDelegate requestDelegate)
        {
            _next = requestDelegate;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string authHeader = httpContext.Request.Headers["Authorization"];
            if(authHeader != null)
            {
                var authHeaderValue = authHeader.Replace("Basic", "").Trim();
                await _next.Invoke(httpContext);

            }
            
            httpContext.Response.StatusCode = (int)StatusCodes.Status401Unauthorized;
        }
        
    }

    
}
