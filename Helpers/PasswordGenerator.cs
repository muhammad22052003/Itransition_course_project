using CourseProject_backend.Interfaces.Helpers;
using System.Security.Cryptography;

namespace CourseProject_backend.Helpers
{
    public class RNGCryptoPasswordGenerator : IPasswordGenerator
    {
        public string Generate(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+";
            var randomBytes = new byte[length];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }

            char[] password = new char[length];
            for (int i = 0; i < length; i++)
            {
                password[i] = validChars[randomBytes[i] % validChars.Length];
            }

            return new string(password);
        }
    }
}
