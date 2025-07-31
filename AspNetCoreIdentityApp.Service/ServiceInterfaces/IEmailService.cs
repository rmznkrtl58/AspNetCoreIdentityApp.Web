namespace AspNetCoreIdentityApp.Service.ServiceInterfaces
{
    public interface IEmailService
    {
        Task SendResetPasswordEmail(string resetPasswordEmailLink, string receiver);
    }
}
