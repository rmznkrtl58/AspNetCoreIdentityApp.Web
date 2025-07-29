using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class UpdateRoleViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Rol Adını Mutlaka Giriniz!")]
        public string Name { get; set; }
    }
}
