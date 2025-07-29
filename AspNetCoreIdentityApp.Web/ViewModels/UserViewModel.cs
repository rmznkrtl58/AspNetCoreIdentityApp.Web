namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class UserViewModel
    {
        public string UserName { get; set; } = default!;
        public string Mail{ get; set; } = default!;
        public string? PhoneNumber { get; set; }
    }
}
