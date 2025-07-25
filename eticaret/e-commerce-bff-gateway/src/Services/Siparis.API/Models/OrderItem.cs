using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Siparis.API.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; } = string.Empty;
      
        public double Price { get; set; }

        public int Quantity { get; set; }


        public int OrderId { get; set; }

        [JsonIgnore]
        public Order? Order { get; set; }

    }
}
