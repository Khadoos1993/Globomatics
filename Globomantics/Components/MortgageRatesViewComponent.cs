using Globomantics.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globomantics.Components
{
    public class MortgageRatesViewComponent: ViewComponent
    {
        private readonly IRateService _rateService;
        public MortgageRatesViewComponent(IRateService rateService)
        {
            _rateService = rateService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var rates = _rateService.GetMortgageRates();
            return View(rates);
        }   
    }
}
