using System.ComponentModel.DataAnnotations;

namespace Staj_ETicaretornek.Models
{
    public class Customer
    {
        
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        [Length(3,30,ErrorMessage ="Kullanıcı adı 3 ile 30 karakter arasında olmalıdır.")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Kullanıcı adı sadece harf ve rakamlardan oluşmalıdır.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "E-posta adresi gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [StringLength(40, ErrorMessage = "Şifre en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string Password { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
