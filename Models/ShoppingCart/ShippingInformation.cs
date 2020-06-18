namespace OnlineStore.Models.ShoppingCart
{
    public class ShippingInformation
    {
        public string FreightType { get; set; }
        public string DestinationCep { get; set; }
        public double Price { get; set; }
        public int EstimatedTimeOfArrivalInDays { get; set; }
        public bool IsSelected { get; set; }
        public int SelectedAddressId { get; set; }
    }
}