namespace CityRoots.Core.DTOs.Purchasereque
{
    public class OnePurchaseRequestForHarvest
    {
        public int Requestcount {  get; set; }
        public int PurchaseRequestId { get; set; }
        public string merchantName {  get; set; }
        public decimal RequestedPrice { get; set; }
        public double RequestedAmount { get; set; }
        public int harvestId {  get; set; }
    }
}
