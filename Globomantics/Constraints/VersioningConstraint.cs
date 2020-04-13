using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globomantics.Constraints
{
    public class VersioningConstraint : IRouteConstraint
    {
        private double requiredVersion;
        public VersioningConstraint(double version)
        {
            requiredVersion = version;
        }
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
                       //if(values[routeKey] == )
            return true;
        }
    }
}
