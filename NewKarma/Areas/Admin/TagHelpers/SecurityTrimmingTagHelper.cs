using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NewKarma.Areas.Identity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Areas.Admin.TagHelpers
{
    [HtmlTargetElement("security-trimming")]
    public class SecurityTrimmingTagHelper : TagHelper
    {
        private readonly ISecurityTrimmingServices _securityTrimmingServices;

        public SecurityTrimmingTagHelper(ISecurityTrimmingServices securityTrimmingServices)
        {
            _securityTrimmingServices = securityTrimmingServices;
        }
        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }
        [HtmlAttributeName("asp-area")]
        public string Area { get; set; }
        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }
        [ViewContext,HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            if (!ViewContext.HttpContext.User.Identity.IsAuthenticated)
            {
                output.SuppressOutput();
            }
            if (_securityTrimmingServices.CanCurrentUserAccess(Area,Controller,Action))
            {
                return;
            }
            output.SuppressOutput();
        }
    }
}
