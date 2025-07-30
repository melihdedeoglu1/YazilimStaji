namespace Rapor.API.Models
{
    public class UrunPerformansRaporu
    {
        public int id { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public DateTime ProductCreatedDate { get; set; }

        public int OrderedCount { get; set; } = 0;

        public int RefundedCount { get; set; } = 0;

    }
}
