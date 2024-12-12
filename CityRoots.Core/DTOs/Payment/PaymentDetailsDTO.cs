namespace CityRoots.Core.DTOs.Payment
{
    public class PaymentDetailsDTO
    {
        public int PaymentId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string PayerName { get; set; }
        public string PayeeName { get; set; }
        public string PayerEmail { get; set; }
        public string PayeeEmail { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public AssociatedCycleDTO? AssociatedCycle { get; set; } // For investments
        public AssociatedHarvestDTO? AssociatedHarvest { get; set; } // For purchases
    }
    public class AssociatedCycleDTO
    {
        public int CycleId { get; set; }
        public string CycleName { get; set; }
    }

    public class AssociatedHarvestDTO
    {
        public int HarvestId { get; set; }
        public string HarvestName { get; set; }
    }
}
