public static class StringExtensions
{
    public static string RemoveMask(this string s) => s.Replace(".", "").Replace(",", "").Replace("-", "");
}