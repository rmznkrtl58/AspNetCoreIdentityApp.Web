using AspNetCoreIdentityApp.Core.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Core.ViewModels
{
    public class UserEditViewModel
    {
        [Display(Name = "Kullanıcı Adı :")]
        [Required(ErrorMessage = "Kullanıcı Adı Zorunludur!")]
        public string Username { get; set; } = default!;
        [Required(ErrorMessage = "Mail Adresi Zorunludur!")]
        [EmailAddress(ErrorMessage = "Mail Adresi Formatında Giriniz!")]
        [Display(Name = "Mail Adresi :")]
        public string Email { get; set; } = default!;
        [Required(ErrorMessage = "Telefon Numarası Zorunludur!")]
        [Display(Name = "Telefon :")]
        public string Phone { get; set; } = default!;
        [Display(Name = "Şehir Seçiniz")]
        public City? City { get; set; }
        [Display(Name = "Profil Resmi :")]
        public IFormFile? Picture { get; set; }
        public string? PictureUrl{ get; set; }
        [Display(Name = "Doğum Tarihi :")]
        public DateTime? BirthDate { get; set; }
        [Display(Name = "Cinsiyet Seçiniz :")]
        public Gender? Gender { get; set; }
    }
}
