using System;
using System.Security.Cryptography;
using System.Text;

namespace TravelGod.ru.Infrastructure.Cryptography
{
    public static class Cryptography
    {
        public static string GenerateRandomCryptographicKey(int keyLength)
        {
            var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[keyLength];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public static string ComputeMd5HashString(string input)
        {
            var bytes = Encoding.ASCII.GetBytes(input);
            MD5 md5 = new MD5CryptoServiceProvider();
            var hash = md5.ComputeHash(bytes);
            var builder = new StringBuilder();
            foreach (var b in hash)
            {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }
    }
}