using AspNetCoreIdentityApp.Service.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UserClaimController : Controller
    {
        private readonly IMemberService _memberService;

        public UserClaimController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        public IActionResult Index()
        {
            var claims =_memberService.GetUserClaimsAsync(User);
            return View(claims);
        }
        [Authorize(Policy = "IstanbulPolicy")]//sadece Citysi istanbul olanlar erişebilir.
        public IActionResult IstanbulPage()
        {
            string message = "İstanbulda Yaşayanlar İçin Bu Sayfaya Erişim Var :D";
            ViewBag.message = message;
            return View();
        }
        [Authorize(Policy = "ExchangeExpireDatePolicy")]//Üye olan kullanıcıların 10 gün süreyle sayfada erişimine izin verilmesi
        public IActionResult ExchangeExpirePage()
        {
            string message = "10 Gün Sayfada Erişimin Var :D";
            ViewBag.message = message;
            return View();
        }
        [Authorize(Policy = "AgeLimitPolicy")]
        public IActionResult AgeLimitPage()
        {
            string message = "18 Yaşından Büyük Olduğun İçin Sayfada Erişimin Var :D";
            ViewBag.message = message;
            return View();
        }
        [Authorize(Policy ="PermissionReadAndDeletePolicy")]
        public IActionResult OrderPermissionPage()
        {
            ViewBag.message= "Permissions.Permissions.Order.Delete";
            ViewBag.message2 = "Permissions.Permissions.Stock.Delete";
            ViewBag.message3 = "Permissions.Permissions.Order.Read";
            return View();
        }
    }
}
