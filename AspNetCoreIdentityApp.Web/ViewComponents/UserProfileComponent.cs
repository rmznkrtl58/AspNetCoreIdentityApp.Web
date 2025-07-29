using AspNetCoreIdentityApp.Web.Entities;
using AspNetCoreIdentityApp.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentityApp.Web.ViewComponents
{
    public class UserProfileComponent:ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public UserProfileComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var findUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            var findInfo = new UserEditViewModel()
            {
                Username = findUser.UserName,
                City = findUser.City,
                PictureUrl=findUser.Picture
            };
            return View(findInfo);
        }
    }
}
