using System;
using System.Linq;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TravelGod.ru.Models;

namespace TravelGod.ru.Infrastructure
{
    [HtmlTargetElement("img", Attributes = "asp-file", TagStructure = TagStructure.WithoutEndTag)]
    public class DbImageTagHelper : TagHelper
    {
        public File AspFile { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.SetAttribute("src",
                $"data:image/jpeg;base64,{Convert.ToBase64String(AspFile.BinaryData)}");
        }
    }
}