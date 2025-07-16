using System.ComponentModel.DataAnnotations;

namespace Staj_ETicaretornek.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
