using AspNetCoreIdentityApp.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreIdentityApp.Service.ServiceInterfaces
{
    public interface IMemberService
    {
        Task<UserEditViewModel> GetUserInfoByUserNameAsync(string userName);
        Task SignOutAsync();
        Task<bool> CheckPasswordAsync(string userName,string oldPassword);
        Task<(bool, IEnumerable<IdentityError>)> ChangePassword(string userName,string oldPassword,string newPassword);
        Task<(bool, IEnumerable<IdentityError>?)> UserUpdateAsync(UserEditViewModel request, string userName);
        List<ClaimViewModel> GetUserClaimsAsync(ClaimsPrincipal userPrincipal);
    }
}
