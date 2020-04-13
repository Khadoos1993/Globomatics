using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globomantics.Filters
{
    public class RateExceptionFilter: IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if(context.Exception is TimeoutException)
            {
                context.Result = new StatusCodeResult(504);
            }
            //base.OnException(context);
        }
    }
}
