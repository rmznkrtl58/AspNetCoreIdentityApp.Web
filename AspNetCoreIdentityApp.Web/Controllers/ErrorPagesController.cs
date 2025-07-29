using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    public class ErrorPagesController : Controller
    {
        public IActionResult AccessDenied(string returnUrl)
        {
            string message = string.Empty;
            message = "Giriş Yetkiniz Yoktur! Yetkililerle İletişime Geçiniz....";
            ViewBag.accessDenied = message;
            return View();
        }
    }
}
