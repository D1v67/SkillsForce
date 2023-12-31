using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLayer.SkillsForce.Helpers
{
    public class PasswordHasher
    {
  
        public static Tuple<byte[], byte[]> HashPassword(string password)
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[16];
                rng.GetBytes(salt);

                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashedPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + Convert.ToBase64String(salt)));

                    return new Tuple<byte[], byte[]>(hashedPassword, salt);
                }
            }
        }

        public static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + Convert.ToBase64String(storedSalt)));

                // Compare the computed hash with the stored hash
                return StructuralComparisons.StructuralEqualityComparer.Equals(inputHash, storedHash);
            }
        }
    }
}
