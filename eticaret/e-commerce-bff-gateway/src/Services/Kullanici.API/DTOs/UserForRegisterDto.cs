using System.ComponentModel.DataAnnotations;

namespace Kullanici.API.DTOs
{
    public class UserForRegisterDto
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        [StringLength(50, ErrorMessage = "Kullanıcı adı en az 3 ve en fazla 50 karakter olmalıdır.", MinimumLength =3)]
        public string Username { get; set; }

        [Required(ErrorMessage = "E-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola zorunludur.")]
        [StringLength(20, ErrorMessage = "Parola en az 6 ve en fazla 20 karakter olmalıdır.", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
