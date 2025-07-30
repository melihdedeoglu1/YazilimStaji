namespace Rapor.API.Models
{
    public class MusteriSiparisRaporu
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public DateTime UserCreatedDate { get; set; }

        public int OrderCount { get; set; } = 0;

        public int RefundCount { get; set; } = 0;

    }
}
