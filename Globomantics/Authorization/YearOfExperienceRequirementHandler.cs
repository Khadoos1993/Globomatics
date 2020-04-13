using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globomantics.Authorization
{
    //public class YearOfExperienceRequirementHandler : AuthorizationHandler<YearOfExperienceRequirement>
    //{
    //    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, YearOfExperienceRequirement requirement)
    //    {
    //        if(!context.User.HasClaim(x=>x.Type == "CareerStarted" && x.Issuer == ""))
    //        {
    //            return Task.CompletedTask;
    //        }

    //        var careerStarted = DateTimeOffset.Parse(context.User.FindFirst(c => c.Type == "CareerStarted" && c.Issuer == "").Value);
    //         context.Succeed;
    //    }
    //}
}
