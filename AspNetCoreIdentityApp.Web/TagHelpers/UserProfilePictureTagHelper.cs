using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspNetCoreIdentityApp.Web.TagHelpers
{
    public class UserProfilePictureTagHelper:TagHelper
    {
        public string? ImageUrl{ get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!string.IsNullOrEmpty(ImageUrl))
            {
                output.Attributes.SetAttribute("src",$"/wwwroot/UserPictures/Default64.png");
            }
            else
            {
                output.Attributes.SetAttribute("src", $"/wwwroot/UserPictures/{ImageUrl}");
            }
        }
    }
}
