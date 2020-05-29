using Newtonsoft.Json;

namespace OnlineStore.Models.ShoppingCart
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Amount { get; set; }

        [JsonIgnore]
        public Product Product { get; set; }
    }
}