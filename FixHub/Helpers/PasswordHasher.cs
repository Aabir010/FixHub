using System;
using System.Security.Cryptography;

namespace FixHub.Helpers
{
    /// <summary>
    /// Secure password hashing using PBKDF2-SHA256.
    /// Uses a random salt per password and configurable iterations.
    /// </summary>
    public static class PasswordHasher
    {
        private const int SaltSize = 16;      // 128-bit salt
        private const int HashSize = 32;      // 256-bit hash
        private const int Iterations = 100000; // OWASP recommended minimum

        /// <summary>
        /// Hash a plain text password with a random salt.
        /// Returns format: base64(salt):base64(hash)
        /// </summary>
        public static string HashPassword(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText));

            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] hash;
            using (var pbkdf2 = new Rfc2898DeriveBytes(plainText, salt, Iterations, HashAlgorithmName.SHA256))
            {
                hash = pbkdf2.GetBytes(HashSize);
            }

            // Combine salt + hash and convert to base64
            byte[] combined = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, combined, 0, SaltSize);
            Array.Copy(hash, 0, combined, SaltSize, HashSize);

            return Convert.ToBase64String(combined);
        }

        /// <summary>
        /// Verify a plain text password against a stored hash.
        /// </summary>
        public static bool VerifyPassword(string plainText, string storedHash)
        {
            if (string.IsNullOrEmpty(plainText) || string.IsNullOrEmpty(storedHash))
                return false;

            try
            {
                byte[] combined = Convert.FromBase64String(storedHash);
                if (combined.Length != SaltSize + HashSize)
                    return false;

                byte[] salt = new byte[SaltSize];
                byte[] storedHashBytes = new byte[HashSize];
                Array.Copy(combined, 0, salt, 0, SaltSize);
                Array.Copy(combined, SaltSize, storedHashBytes, 0, HashSize);

                byte[] computedHash;
                using (var pbkdf2 = new Rfc2898DeriveBytes(plainText, salt, Iterations, HashAlgorithmName.SHA256))
                {
                    computedHash = pbkdf2.GetBytes(HashSize);
                }

                // Constant-time comparison to prevent timing attacks
                return FixedTimeEquals(storedHashBytes, computedHash);
            }
            catch
            {
                return false;
            }
        }

        // Constant-time byte comparison
        private static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;

            int result = 0;
            for (int i = 0; i < a.Length; i++)
            {
                result |= a[i] ^ b[i];
            }
            return result == 0;
        }
    }
}