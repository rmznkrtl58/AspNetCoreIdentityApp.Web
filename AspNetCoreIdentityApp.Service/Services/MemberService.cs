using AspNetCoreIdentityApp.Core.ViewModels;
using AspNetCoreIdentityApp.Repository.Entities;
using AspNetCoreIdentityApp.Service.ServiceInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;

namespace AspNetCoreIdentityApp.Service.Services
{
    public class MemberService : IMemberService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IFileProvider _fileProvider;
        private readonly SignInManager<AppUser> _signInManager;

        public MemberService(UserManager<AppUser> userManager, IFileProvider fileProvider, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _fileProvider = fileProvider;
            _signInManager = signInManager;
        }
        public async Task<(bool, IEnumerable<IdentityError>?)> ChangePassword(string userName, string oldPassword, string newPassword)
        {
            var findUser = await _userManager.FindByNameAsync(userName)!;
            var resultchangePassword = await _userManager.ChangePasswordAsync(findUser!, oldPassword
              ,newPassword);
            if (!resultchangePassword.Succeeded)
            {
                return (false, resultchangePassword.Errors);
            }
            //Kullanıcımın kritik bilgileri değiştiğinde securitStampide güncelle
            await _userManager.UpdateSecurityStampAsync(findUser!);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(findUser!, newPassword, true, false);
            return (true, null);
        }
        public async Task<bool> CheckPasswordAsync(string userName, string oldPassword)
        {
            var findUser = await _userManager.FindByNameAsync(userName);
            //kullanıcının eski girdiği şifre ile db'deki şifre eşleşiyor mu?
            bool checkOldPassword = await _userManager.CheckPasswordAsync(findUser!, oldPassword);
            return checkOldPassword;
        }
        public List<ClaimViewModel> GetUserClaimsAsync(ClaimsPrincipal userPrincipal)
        {
            var claims = userPrincipal.Claims.Select(x => new ClaimViewModel()
            {
                Issuer = x.Issuer,
                Type = x.Type,
                Value = x.Value
            }).ToList();
            return claims;
        }
        public async Task<UserEditViewModel> GetUserInfoByUserNameAsync(string userName)
        {
            var findUser = await _userManager.FindByNameAsync(userName);
            var userInfo = new UserEditViewModel()
            {
                Email = findUser.Email,
                Phone = findUser.PhoneNumber,
                Username = findUser.UserName,
                BirthDate = findUser.BirthDate,
                City = findUser.City,
                Gender = findUser.Gender,
            };
            return userInfo;
        }
        public async Task SignOutAsync()
        {
            //IdentityExtension classımda LogOutPathi buraya verdim cs.html tarafında ise asp-route-returnurl ile nereye yönlendirmem gerektiğini yazdım.
            await _signInManager.SignOutAsync();
        }
        public async Task<(bool, IEnumerable<IdentityError>?)> UserUpdateAsync(UserEditViewModel request,string userName)
        {
            throw new NotImplementedException();
        }

    }
}
