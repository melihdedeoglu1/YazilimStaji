using System.ComponentModel.DataAnnotations;

namespace Urun.API.DTOs
{
    public class ProductForCreateDto
    {
        
        public string Name { get; set; }
       
        public string Description { get; set; }
       
        public double Price { get; set; }
       
        public int StockQuantity { get; set; }
    }
}
