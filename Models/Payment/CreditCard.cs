namespace OnlineStore.Models.Payment
{
    public class CreditCard
    {
        public string Number { get; set; }
        public string HolderName { get; set; }
        public string ExpirationDate { get; set; }
        public string Cvv { get; set; }
    }
}