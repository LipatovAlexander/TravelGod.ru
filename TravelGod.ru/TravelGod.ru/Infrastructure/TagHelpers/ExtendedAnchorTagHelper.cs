using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TravelGod.ru.Infrastructure.TagHelpers
{
    [HtmlTargetElement("a", Attributes = "asp-page, maybe-active")]
    [HtmlTargetElement("a", Attributes = "asp-page-folder")]
    public class ExtendedAnchorTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExtendedAnchorTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (_httpContextAccessor.HttpContext?.Request.RouteValues["Page"] is not string currentPage)
            {
                return;
            }

            if (context.AllAttributes["asp-page"]?.Value is string pageName && pageName == currentPage)
            {
                output.AddClass("active", HtmlEncoder.Default);
            }

            var routeArray = currentPage.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (routeArray.Length < 2)
            {
                return;
            }

            if (context.AllAttributes["asp-page-folder"]?.Value?.ToString() == routeArray[0])
            {
                output.AddClass("active", HtmlEncoder.Default);
            }
        }
    }
}