using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLayer.SkillsForce.Helpers
{
    public class PasswordHasher
    {
        /// <summary>
        /// Generates a hashed password and a unique salt based on the provided password using the SHA-256 hashing algorithm.
        /// https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.sha256?view=net-8.0
        /// </summary>
        /// <param name="password">The password to be hashed.</param>
        /// <returns>
        /// A Tuple containing the hashed password (byte array) and the salt (byte array).
        /// </returns>
        /// 
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

        /// <summary>
        /// Verifies whether a provided password matches the stored hash and salt using the SHA-256 hashing algorithm.
        /// </summary>
        /// <param name="password">The password to be verified.</param>
        /// <param name="storedHash">The stored hashed password.</param>
        /// <param name="storedSalt">The stored salt used during password hashing.</param>
        /// <returns>
        /// True if the provided password matches the stored hash and salt; otherwise, false.
        /// </returns>
        /// 
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
