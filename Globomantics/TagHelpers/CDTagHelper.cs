using Globomantics.Core.Models;
using Globomantics.Services;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globomantics.TagHelpers
{
    [HtmlTargetElement("cdrate")]
    public class CDTagHelper: TagHelper
    {
        public string Title { get; set; }
        public string MeterPercent { get; set; }

        public CDTermLength TermLength { get; set; }

        private readonly IRateService _rateService;
        public CDTagHelper(IRateService rateService)
        {
            _rateService = rateService;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var rate = _rateService.GetCDRateByTerm(TermLength);
            output.Content.SetHtmlContent(
                $@"<div class='meter'>
                    <p>{Title}</p>
                    <div class='progress'>
                        <div class='progress-bar bg-info' style='width: {MeterPercent}%'></div>
                    </div>
                </div>
            ");
            base.Process(context, output);
        }
    }
}
