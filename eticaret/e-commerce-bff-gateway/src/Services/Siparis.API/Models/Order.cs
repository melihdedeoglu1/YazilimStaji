using System.ComponentModel.DataAnnotations;

namespace Siparis.API.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public double TotalPrice { get; set; }

        public string Status { get; set; } = string.Empty;

        public ICollection<OrderItem>? OrderItems { get; set; }


    }
}
