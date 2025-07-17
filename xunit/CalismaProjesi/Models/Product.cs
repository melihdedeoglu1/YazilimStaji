using System.ComponentModel.DataAnnotations;

namespace CalismaProjesi.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün adı boş olamaz.")]
        public string Name { get; set; }

        [Range(0.01, 10000.00, ErrorMessage ="Fiyat 0.01 ile 10000.00 arasında olmalıdır.")]
        public double Price { get; set; }
        
    }
}
