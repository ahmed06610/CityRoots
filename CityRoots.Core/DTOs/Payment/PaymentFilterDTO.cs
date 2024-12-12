namespace CityRoots.Core.DTOs.Payment
{
    public class PaymentFilterDTO
    {
        public string Id { get; set; }
        public string? Type { get; set; } // "Investment", "Purchase"
        public string? Status { get; set; } // "Accepted", "Rejected"
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
      
    }
}
