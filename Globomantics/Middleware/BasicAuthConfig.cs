using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globomantics.Middleware
{
    public class BasicAuthConfig
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<BasicAuthMiddleware>();
        }
    }
}
