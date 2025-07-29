using Microsoft.AspNetCore.Mvc;
using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using AspNetCoreIdentityApp.Web.Entities;
using AspNetCoreIdentityApp.Web.Services;
using NuGet.Protocol.Core.Types;
using System.Security.Claims;
namespace AspNetCoreIdentityApp.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        public LoginController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel p, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            //returnUrl boş ise beni Default Controllerimdaki UserListe yönlendirmesi için returnUrli dolduruyom.
            //eğer dolu ise returnUrl'e gelen değeri returnUrl'e atıyorum.
            returnUrl = returnUrl ?? Url.Action("UserList", "Default");

            var hasUser = await _userManager.FindByEmailAsync(p.Mail);
            if (hasUser is null)
            {
                ModelState.AddModelError(string.Empty, "Mail Adresin Yanlıştır!");
                return View();
            }

            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, p.Password, p.RememberMe, true);

            if (signInResult.IsLockedOut)//Eğer 3 den fazla yanlış şifre girilirse
            {
                ModelState.AddModelErrorListExt(new List<string>() {
                "3'den fazla yanlış giriş yapıldı! 3 dakika sonra tekrar deneyin"});
                return View();
            }

            if (!signInResult.Succeeded)
            {
                int countOfLockouts = await _userManager.GetAccessFailedCountAsync(hasUser);
                ModelState.AddModelErrorListExt(new List<string>() { "Şifre Yanlıştır!", $"'{countOfLockouts}'Kere Şifreyi Yanlış Girdin" });
                return View();
            }
            //İlgili kullanıcının doğum tarihi mevcut mu?
            if (hasUser.BirthDate.HasValue)
            {
                await _signInManager.SignInWithClaimsAsync(hasUser, p.RememberMe, new[]
                {
                    new Claim("birthDate",hasUser.BirthDate.Value.ToString())
                });
            }
            return Redirect(returnUrl!);
        }
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel p)
        {

            var hasUser = await _userManager.FindByEmailAsync(p.Mail);
            if (hasUser is null)
            {
                ModelState.AddModelError(string.Empty, "Bu Mail Adresine Sahip Kullanıcı Bulunamamıştır!");
                return View();
            }
            //Link Örneği:https://localhost:7185?userId=11111&token=aajasdasdasfcxvsdgh
            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);
            var passwordResetLink = Url.Action("ResetPassword", "Login", new { userId = hasUser.Id, Token = passwordResetToken },HttpContext.Request.Scheme);

            //Mail Gönderimi
            await _emailService.SendResetPasswordEmail(passwordResetLink!,hasUser.Email!);

            TempData["success"] = "Şifre Yenileme Linki E-posta Adresinize Gönderilmiştir.";
            return RedirectToAction(nameof(ForgetPassword));
        }
        [HttpGet]
        public IActionResult ResetPassword(string userId,string Token)
        {   //otomatik olarak yüklenen şifre yenileme sayfamda zaten Urlimde bu değerler geleceğinden dolayı tempdatalar ile direk çekiyorum
            TempData["userid"] = userId;
            TempData["token"]= Token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel p)
        {
            var userId = TempData["userid"];
            var token = TempData["token"];
            //burda null kontrolü yapıyorum sebebi,tempdata sayfa ilk yüklendiğinde içine data gelecek ve şifreyi yenile butonuna bastığım zaman şifreyi yenileyecek ikinci bir defa butona basarsa tempdatanın içi boş olacak ve hata fırlatacak 
            if(userId is null || token is null)
            {
                throw new Exception("UserId veya Tokena Null Değer Atandı!");
            }

            var hasUser = await _userManager.FindByIdAsync(userId.ToString()!);
            if(hasUser is null)
            {
                ModelState.AddModelError(string.Empty, "Sistemde Böyle Bir Kullanıcı Yoktur!");
                return View();
            }

            var result = await _userManager.ResetPasswordAsync(hasUser, token.ToString()!, p.Password);
            if (result.Succeeded)
            {
                TempData["ResetSuccess"] = "Şifre Başarıyla Yenilenmiştir.";
                return View();
            }

            ModelState.AddModelErrorListExt(result.Errors.Select(x => x.Description).ToList());
            return View();
        }
        public async Task SignOut()
        {
            //IdentityExtension classımda LogOutPathi buraya verdim cs.html tarafında ise asp-route-returnurl ile nereye yönlendirmem gerektiğini yazdım.
            await _signInManager.SignOutAsync();
        }
    }
}
