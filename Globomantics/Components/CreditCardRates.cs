using Globomantics.Core.Models;
using Globomantics.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globomantics.Components
{
    [ViewComponent]
    public class CreditCardRates: ViewComponent
    {
        private readonly IRateService _rateService;
        public CreditCardRates(IRateService rateService)
        {
            _rateService = rateService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string title, string subTitle)
        {
            var ratesVM = new CreditCardWidgetVM()
            {
               
                Rates = _rateService.GetCreditCardRates(),
                WidgetSubTitle = subTitle,
                WidgetTitle = title
            };
            return View(ratesVM);

        }
    }

    public class CreditCardWidgetVM
    {
        public List<Rate> Rates { get; set; }
        public string WidgetTitle { get; set; }
        public string WidgetSubTitle { get; set; }
    
    }
}
