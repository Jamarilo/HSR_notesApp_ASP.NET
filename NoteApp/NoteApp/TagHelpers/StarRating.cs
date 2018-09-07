using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NoteApp.TagHelpers
{

    /**
     * Tag helper for star rating.
     * In the future it may be extended to support the number of stars as an 
     * attribute of the tag.
     */
    [HtmlTargetElement("starrating")]
    public class StarRatingTagHelper : TagHelper
    {
        public string Rating { get; set; }
        public string Itemid { get; set; }
        public string Inputname { get; set; }
        public string disablecheck { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "input";
            // Process the asp-for as normal - this will set the id,name,value attributes of the input element based on the model
            base.Process(context, output);
            string htmlContent = "";
            for (int i = 5; i > 0; i--)
            {
                string star;
                string strid = $"rating-input-1-{i}";
                string itemstarname = $"{Itemid}";
                string checkedString = (i.ToString() == Rating) ? "checked='checked'" : "";
                string disabledString = disablecheck == "true" ? "disabled" : "";
                string codeString = $@"document.getElementById(""{Inputname}"").value = {i};";
                star = $@"
                    <input {disabledString}  onclick='{codeString}' type='radio' class='rating-input' 
                            id='{strid}' name='{itemstarname}' {checkedString}>
                    <label for='{strid}' class='rating-star'></label>";
                htmlContent += star;
            }

            output.Content.SetHtmlContent(htmlContent);
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("name", Inputname);
            output.Attributes.SetAttribute("hidden", "true");
            output.Attributes.SetAttribute("value", Rating);
            output.Attributes.SetAttribute("id", Inputname);

        }
    }
}
