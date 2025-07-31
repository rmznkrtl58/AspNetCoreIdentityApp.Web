using AspNetCoreIdentityApp.Repository.Entities;
using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        public RegisterController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult SignUp()
        {
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel p)
        {   
            if (!ModelState.IsValid)
            {
                return View();
            }
            var newUser = new AppUser()
            {
                UserName=p.Username,
                Email=p.Email,
                PhoneNumber=p.Phone,
                TwoFactor=0
            };
            var identityResult = await _userManager.CreateAsync(newUser,p.ConfirmPassword);
            if (!identityResult.Succeeded)
            {
                ModelState.AddModelErrorListExt(identityResult.Errors.Select(x => x.Description).ToList());
                return View();
            }

            var findUser = await _userManager.FindByNameAsync(p.Username);
            //Siteye üye olan bir kimse 10 gün boyunca ilgili sayfaya erişim sağlar.10 günün sonunda erişimi kaybeder.
            var exchangeExpireClaim = new Claim("ExchangeExpireDate", DateTime.Now.AddDays(10).ToShortDateString());
            var addClaimResult = await _userManager.AddClaimAsync(findUser!, exchangeExpireClaim);
            if (!addClaimResult.Succeeded)
            {
                ModelState.AddModelErrorListExt(addClaimResult.Errors.Select(x => x.Description).ToList());
                return View();
            }
            TempData["SuccessMessage"] = "Kullanıcı Kaydı Başarıyla Gerçekleşti.";
            return RedirectToAction(nameof(SignUp));
           
        }
    }
}
