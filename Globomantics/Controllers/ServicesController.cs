using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Globomantics.Models;

namespace Globomantics.Controllers
{
    public class ServicesController : Controller
    {
        [HttpGet]
        [Route("[controller]/{token:versionCheck(2)}")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
