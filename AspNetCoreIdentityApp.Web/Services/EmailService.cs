using AspNetCoreIdentityApp.Web.OptionsModels;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AspNetCoreIdentityApp.Web.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings>_options)
        {
            _emailSettings = _options.Value;//IdentityExt metodumda yapılandırdım datayı okuyup valueye data gelecek _emailSettingse atama yapacak
        }
        public async Task SendResetPasswordEmail(string resetPasswordEmailLink, string receiver)
        {
            //Mail işlemlerini kapsayan clientim
            var smptClient = new SmtpClient();
            smptClient.Host = _emailSettings.Host;
            smptClient.DeliveryMethod= SmtpDeliveryMethod.Network;
            smptClient.UseDefaultCredentials= false;
            smptClient.Port = 587;//Gmailin port numarası
            smptClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);//Gönderecek Kurumun Maili ve Google Hesabındaki Uygulama Şifresi
            smptClient.EnableSsl = true;

            var mailMessage=new MailMessage();
            mailMessage.From=new MailAddress(_emailSettings.Email);//gönderici Epostası
            mailMessage.To.Add(receiver);//alıcının mail adresi
            mailMessage.Subject = "LocalHost | Şifre Sıfırlama Linki";
            mailMessage.Body =
            @$"
             <h3>Şifrenizi Yenilemek İçin Aşağıdaki Linke Tıklayınız!</h3>
             <p><a href='{resetPasswordEmailLink}'>Şifre Yenileme Linki</a></p>";
            mailMessage.IsBodyHtml = true;
            await smptClient.SendMailAsync(mailMessage);
        }
    }
}
