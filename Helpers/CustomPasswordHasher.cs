using System.Text;
using CourseProject_backend.Interfaces.Helpers;
using SHA3.Net;
using SHA3.Net.BouncyCastle;

namespace CourseProject_backend.Helpers
{
    public class Sha3_256PasswordHasher : IPasswordHasher
    {
        public string Generate(string password)
        {
            byte[] passwordByte = Encoding.UTF8.GetBytes(password);
            string hashedPassword = string.Empty;

            using (Sha3 sha3 = Sha3.Sha3256())
            {
                hashedPassword = BitConverter.ToString(sha3.ComputeHash(passwordByte))
                                             .Replace("-", string.Empty);
            }

            return hashedPassword;
        }

        public bool Verify(string password, string hashedPassword)
        {
            string result = Generate(password);

            if (hashedPassword == result) { return true; }

            return false;
        }
    }
}
