using AspNetCoreIdentityApp.Service.ServiceInterfaces;
using System.Text.Encodings.Web;

namespace AspNetCoreIdentityApp.Service.Services
{
    public class TwoFactorService:ITwoFactorService
    {
        private readonly UrlEncoder _urlEncoder;
        public TwoFactorService(UrlEncoder urlEncoder)
        {
            _urlEncoder = urlEncoder;
        }
        public string GenerateQRCodeUri(string email, string unformattedKey)
        {
            //UnformettedKey=>Identitynin İlgili kullanıcıya oluşturmuş olduğu SharedKeydir
            const string format = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
            //"www.kartalRentACar.com"->0.parametre
            //email=1.parametre unFormattedKey->2.parametre
            //digits=>6 haneli kod talebini belirtir
            return string.Format(format, _urlEncoder.Encode("www.kartalRentACar.com"), _urlEncoder.Encode(email),unformattedKey);
        }
    }
}
