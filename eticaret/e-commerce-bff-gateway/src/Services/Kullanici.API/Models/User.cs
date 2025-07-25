using System.ComponentModel.DataAnnotations;

namespace Kullanici.API.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];

        public DateTime CreatedAt { get; set; }


        public string Role { get; set; } = string.Empty;

    }
}
