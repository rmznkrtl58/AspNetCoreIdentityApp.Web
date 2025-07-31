using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Core.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage ="Rol Adını Mutlaka Giriniz!")]
        public string Name { get; set; }
    }
}
