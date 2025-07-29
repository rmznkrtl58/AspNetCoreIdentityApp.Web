using AspNetCoreIdentityApp.Web.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace AspNetCoreIdentityApp.Web.TagHelpers
{
    public class UserRoleNamesTagHelper:TagHelper
    {
        public string UserId { get; set; } = default!;
        private readonly UserManager<AppUser> _userManager;
        public UserRoleNamesTagHelper(string userId, UserManager<AppUser> userManager)
        {
            UserId = userId;
            _userManager = userManager;
        }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
           var findUser=await _userManager.FindByIdAsync(UserId);
           var userRoles = await _userManager.GetRolesAsync(findUser!);
           var stringBuilder = new StringBuilder();

            foreach(string x in userRoles)
            {
                stringBuilder
                .Append(@$"
                <span class='badge bg-secondary mx-1'>{x.ToLower()}</span>
                ");
            }
            output.Content.SetHtmlContent(stringBuilder.ToString());
        }
    }
}
