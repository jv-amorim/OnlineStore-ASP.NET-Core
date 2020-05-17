using System;

namespace OnlineStore.Libraries.Helpers.StringHelpers
{
    public static class RandomString
    {
        public static string GenerateRandomString(int stringLength)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[stringLength];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
                stringChars[i] = chars[random.Next(chars.Length)];

            return new string(stringChars);
        }
    }
}