using AspNetCoreIdentityApp.Web.Entities;
using AspNetCoreIdentityApp.Web.Enums;
using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    [Authorize]
    public class DefaultController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IFileProvider _fileProvider;
        public DefaultController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IFileProvider fileProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _fileProvider = fileProvider;
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
            var findUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            var userInfo = new UserEditViewModel()
            {
                Email = findUser.Email,
                Phone = findUser.PhoneNumber,
                Username = findUser.UserName,
                BirthDate= findUser.BirthDate,
                City=findUser.City,
                Gender= findUser.Gender,
            };
            return View(userInfo);
        }
        [HttpPost]
        public async Task<IActionResult> UserInfo(UserEditViewModel p)
        {   //Kullanıcı Bilgilerimi Güncellediğim yerdir.
            
            if (!ModelState.IsValid)
            {
                return View();
            }
            var findUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            findUser!.UserName = p.Username;
            findUser.BirthDate= p.BirthDate;
            findUser.City = p.City;
            findUser.Gender = p.Gender;
            findUser.PhoneNumber = p.Phone;
            findUser.Email= p.Email;
            //Eğerki resim parametremde bir değer var ise
            if (p.Picture != null && p.Picture.Length > 0)
            {
                var wwwRootFolder = _fileProvider.GetDirectoryContents("wwwroot");
                //resmin Uzantısını Al ->.jpg gibi
                string randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(p.Picture.FileName)}";
                //wwwroot klasörü içerisinde dosya adı==UserPictures eşit olan klasörü ve o klasörün bulunduğu yolu al uzantısıyla birlikte
                var newPicturePath = Path.Combine(wwwRootFolder!.First(x => x.Name == "UserPictures").PhysicalPath!,randomFileName);
                using var stream = new FileStream(newPicturePath, FileMode.Create);
                await p.Picture.CopyToAsync(stream);//IFormFile türündeki picture parametremden gelen resmi streamdeki yola oluştur.
                findUser.Picture= randomFileName;
            }
            var updateResult = await _userManager.UpdateAsync(findUser);
            if (!updateResult.Succeeded)
            {
                ModelState.AddModelErrorListExt(updateResult.Errors);
                return View();
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

            var findUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

            //kullanıcının eski girdiği şifre ile db'deki şifre eşleşiyor mu?
            bool checkOldPassword = await _userManager.CheckPasswordAsync(findUser!, p.PasswordOld);
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
            var resultchangePassword = await _userManager.ChangePasswordAsync(findUser!, p.PasswordOld
                , p.PasswordNew);
            if (!resultchangePassword.Succeeded)
            {
                ModelState.AddModelErrorListExt(resultchangePassword.Errors.Select(x => x.Description).ToList());
            }
            //Kullanıcımın kritik bilgileri değiştiğinde securitStampide güncelle
            await _userManager.UpdateSecurityStampAsync(findUser!);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(findUser!, p.PasswordNew, true, false);
            TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirilmiştir";
            return View();
        }
        [Authorize(Roles ="Admin")]
        [HttpGet]
        public IActionResult AdminPage()
        {
            string message = "Sadece Admin Bu Sayfaya Erişebilir!";
            ViewBag.Message = message;
            return View();
        }
    }
}
