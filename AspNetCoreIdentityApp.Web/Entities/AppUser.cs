using AspNetCoreIdentityApp.Web.Enums;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.Entities
{
    public class AppUser : IdentityUser
    {
        //kullanıcıdan güncellemesini zorunlu tutmadığım yerlere null değer olabilir diyorum çünkü üye olurken bunları istemiyoruz userEdit panelinden isterlerse burayı güncelleyebilirler.
        public City? City{ get; set; }
        public string? Picture{ get; set; }
        public DateTime? BirthDate{ get; set; }
        public Gender? Gender{ get; set; }
    }
}
