using System.ComponentModel.DataAnnotations;

namespace Urun.API.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün adı gereklidir.")]
        [StringLength(100, ErrorMessage = "Ürün adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; }
       
        [StringLength(500, ErrorMessage = "Ürün açıklaması en fazla 500 karakter olabilir.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Ürün fiyatı gereklidir.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Ürün fiyatı 0'dan büyük olmalıdır.")]
        public double Price { get; set; }

        public int StockQuantity { get; set; }

        public DateTime? CreatedAt { get; set; }

    }
}
