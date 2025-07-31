using AspNetCoreIdentityApp.Repository.Entities;
using AspNetCoreIdentityApp.Core.Enums;
using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;
using AspNetCoreIdentityApp.Service.ServiceInterfaces;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    [Authorize]
    public class DefaultController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IFileProvider _fileProvider;
        private readonly IMemberService _memberService;
        private readonly ITwoFactorService _twoFactorService;
        private string userName => User.Identity!.Name!;//lambda ile tanımladığım için sadece get'i olan bir prop olduğunu belirtmiş olurum.
        public DefaultController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IFileProvider fileProvider, IMemberService memberService, ITwoFactorService twoFactorService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _fileProvider = fileProvider;
            _memberService = memberService;
            _twoFactorService = twoFactorService;
        }
        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = users.Select(x => new UserListViewModel()
            {
                UserId = x.Id,
                UserMail = x.Email,
                UserName = x.UserName
            }).ToList();
            return View(userViewModels);
        }
        [HttpGet]
        public async Task<IActionResult> UserInfo()
        {   //Kullanıcı Bilgilerimi Getirdiğim yerdir.
            ViewBag.genders = new SelectList(Enum.GetNames(typeof(Gender)));
            ViewBag.cities = new SelectList(Enum.GetNames(typeof(City)));
            var userInfo = await _memberService.GetUserInfoByUserNameAsync(userName);
            return View(userInfo);
        }
        [HttpPost]
        public async Task<IActionResult> UserInfo(UserEditViewModel p)
        {   //Kullanıcı Bilgilerimi Güncellediğim yerdir.

            if (!ModelState.IsValid)
            {
                return View();
            }
            var findUser = await _userManager.FindByNameAsync(userName);
            findUser!.UserName = p.Username;
            findUser.BirthDate = p.BirthDate;
            findUser.City = p.City;
            findUser.Gender = p.Gender;
            findUser.PhoneNumber = p.Phone;
            findUser.Email = p.Email;
            //Eğerki resim parametremde bir değer var ise
            if (p.Picture != null && p.Picture.Length > 0)
            {
                var wwwRootFolder = _fileProvider.GetDirectoryContents("wwwroot");
                //resmin Uzantısını Al ->.jpg gibi
                string randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(p.Picture.FileName)}";
                //wwwroot klasörü içerisinde dosya adı==UserPictures eşit olan klasörü ve o klasörün bulunduğu yolu al uzantısıyla birlikte
                var newPicturePath = Path.Combine(wwwRootFolder!.First(x => x.Name == "UserPictures").PhysicalPath!, randomFileName);
                using var stream = new FileStream(newPicturePath, FileMode.Create);
                await p.Picture.CopyToAsync(stream);//IFormFile türündeki picture parametremden gelen resmi streamdeki yola oluştur.
                findUser.Picture = randomFileName;
            }
            var updateResult = await _userManager.UpdateAsync(findUser);
            if (!updateResult.Succeeded)
            {
                ModelState.AddModelErrorListExt(updateResult.Errors);
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(findUser);
            await _signInManager.SignOutAsync();

            //parametremde doğum tarihi mevcut mu?
            if (p.BirthDate.HasValue)
            {
                await _signInManager.SignInWithClaimsAsync(findUser, true, new[]
                {
                   new Claim("birthDate",p.BirthDate.Value.ToString())
                });
            }
            else
            {
                await _signInManager.SignInAsync(findUser, true);
            }
            var userInfo = new UserEditViewModel()
            {
                Email = findUser.Email,
                Phone = findUser.PhoneNumber,
                Username = findUser.UserName,
                BirthDate = findUser.BirthDate,
                City = findUser.City,
                Gender = findUser.Gender,
            };
            TempData["updateSuccess"] = "Kullanıcı Bilgileri başarıyla Güncellenmiştir.";
            return View(userInfo);
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(PasswordChangeViewModel p)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var checkOldPassword = await _memberService.CheckPasswordAsync(userName, p.PasswordOld);
            if (!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Eski Girilen Şifre Yanlış");
                return View();
            }
            //Kullanıcının Eski Şifresiyle Yeni Girdiği Şifre Aynı Olmamalı!
            //if (p.PasswordOld == p.PasswordNew)
            //{
            //    ModelState.AddModelError(string.Empty, "Eski Girilen Şifre İle Yeni Girilen Şifre Aynı Olmamalı!");
            //    return View();
            //}
            var (isSuccess, errors) = await _memberService.ChangePassword(userName, p.PasswordOld, p.PasswordNew);
            if (!isSuccess)
            {
                ModelState.AddModelErrorListExt(errors);
            }

            TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirilmiştir";
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AdminPage()
        {
            string message = "Sadece Admin Bu Sayfaya Erişebilir!";
            ViewBag.Message = message;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> TwoFactorAuth()
        {
            var findUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            var authenticatorViewModel = new AuthenticatorViewModel()
            {
                TwoFactorType = (TwoFactor)findUser!.TwoFactor
            };
            return View(authenticatorViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> TwoFactorAuth(AuthenticatorViewModel p)
        {
            var findUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            switch (p.TwoFactorType)
            {   //Hiçbiri seçiliyse
                case TwoFactor.None:
                    findUser!.TwoFactorEnabled = false;
                    findUser.TwoFactor = (byte)TwoFactor.None;
                    TempData["message"] = "İki Adımlı Doğrulamanız 'Hiçbiri' Olarak Seçildi!";
                break;
                case TwoFactor.MicrosoftAndGoogle:
                    //Eğerki dropdownda microsoft/google ile doğrulama aktif ise
                    return RedirectToAction("TwoFactorWithAuthenticator");

                default:
                    break;
            }
            await _userManager.UpdateAsync(findUser!);
            return View(p);
        }
        [HttpGet]
        public async Task<IActionResult> TwoFactorWithAuthenticator()
        {
            var findUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            string getUnformattedKey = await _userManager.GetAuthenticatorKeyAsync(findUser);
            if (string.IsNullOrEmpty(getUnformattedKey))
            {
                //unformatted key->bizim Identity tarafından Google/Microsoft Authenticatorun okuması için kullanıcıma ait bilgilerin tutulduğu shared keyin şifrelenmişt halidir
                await _userManager.ResetAuthenticatorKeyAsync(findUser);
                getUnformattedKey = await _userManager.GetAuthenticatorKeyAsync(findUser);
            }
            var model = new AuthenticatorViewModel()
            {
                SharedKey = getUnformattedKey!,
                AuthenticationUri = _twoFactorService.GenerateQRCodeUri(findUser.Email!, getUnformattedKey!)
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> TwoFactorWithAuthenticator(AuthenticatorViewModel p)
        {
            var findUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            //kullanıcım birleşik olan kodu ayrı ayrıda girebilir veya - lide girebilir tehlikeye atmamak adına iki taraftanda girerse eğer onları kaldırıp birleşik gelmesini sağladım
            var verificationCode = p.VerificationCode.Replace(" ", string.Empty).Replace("-", string.Empty);
            bool is2FATokenValid = await _userManager.VerifyTwoFactorTokenAsync(findUser!, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);
            if (is2FATokenValid)
            {
                findUser!.TwoFactorEnabled = true;
                findUser.TwoFactor = (byte)TwoFactor.MicrosoftAndGoogle;
                //kurtarma kodlarını belirleyip çağırıyorum çünkü olurda telefondaki uygulamaya giremesse bu kodları bir yere not alsın
                var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(findUser, 5);
                TempData["recoveryCodes"]= recoveryCodes;
                TempData["message"] = "İki Adımlı Doğrulamanız 'Google/Microsoft-Authenticator' Olarak Seçildi!";
                return RedirectToAction("TwoFactorAuth");
            }
            else
            {
                ModelState.AddModelError("", "Girdiğiniz Doğrulama Kodu Yanlıştır!");
                return View(p);
            }
        }
      
    }
}
