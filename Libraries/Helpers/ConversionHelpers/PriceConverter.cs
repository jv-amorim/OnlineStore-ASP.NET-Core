namespace OnlineStore.Libraries.Helpers.ConversionHelpers
{
    public static class PriceConverter
    {
        public static int ConvertDoublePriceToPriceInCents(double price)
        {
            string priceString = price.ToString().Replace(".", "").Replace(",", "");
            return int.Parse(priceString);
        }

        public static int ConvertDecimalPriceToPriceInCents(decimal price)
        {
            string priceString = price.ToString().Replace(".", "").Replace(",", "");
            return int.Parse(priceString);
        }
    }
}