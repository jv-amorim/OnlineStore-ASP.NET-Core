namespace OnlineStore.Models.ShoppingCart
{
    public class ShippingInformation
    {
        public string FreightType { get; set; }
        public double Price { get; set; }
        public int EstimatedTimeOfArrivalInDays { get; set; }
    }
}