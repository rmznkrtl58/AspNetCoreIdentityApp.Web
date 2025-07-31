using AspNetCoreIdentityApp.Repository.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AspNetCoreIdentityApp.Web.ClaimProviders
{   //Login Olduğunda claimleri otomatik olarak maplayerek çalışır
    public class UserClaimProvider : IClaimsTransformation
    {
        private readonly UserManager<AppUser> _userManager;
        public UserClaimProvider(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {    
            //ClaimsIdentity=>Kullanıcıyı tespit ederken Cookielere bakıyoruz o cookilerin ClaimsIdentityden oluştuğunu gösterir
            var identityUser = principal.Identity as ClaimsIdentity;
            var findUser = await _userManager.FindByNameAsync(identityUser!.Name!);
      
            if(string.IsNullOrEmpty(findUser!.City.ToString()))
            {
                return principal;
            }
            //City claimi oluşturdum yalnız burda claims tablosuna eklemiyor login yapınca direk claims içerisinde gözüküyor yani default olarak geliyor.ilerdede claims tablosuna ekleme yaparız
            if(principal.HasClaim(x=>x.Type!="city"))
            {
                Claim cityClaim = new Claim("city",findUser.City.ToString()!);
                identityUser.AddClaim(cityClaim);
            }
            return principal;


        }
    }
}
