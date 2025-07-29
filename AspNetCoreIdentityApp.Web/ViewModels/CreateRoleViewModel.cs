using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage ="Rol Adını Mutlaka Giriniz!")]
        public string Name { get; set; }
    }
}
