using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using TestTypingApi.Models;
using TestTypingApi.Models.DB;

namespace TestTypingApi.Service
{
    public class PasswordService
    {
        //private readonly PasswordHasher<TestTypeUser> _hasher = new();

        //public string HashPassword(TestTypeUser user, string password)
        //{
        //    return _hasher.HashPassword(user, password);
        //}

        //public bool VerifyPassword(TestTypeUser user, string password)
        //{
        //    var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
        //    return result == PasswordVerificationResult.Success;
        //}

        public string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32));

            return $"{Convert.ToBase64String(salt)}.{hash}";

        }

        public bool VerifyPassword(TestTypeUser user, string password)
        {
            var parts = user.PasswordHash.Split('.');
            if (parts.Length != 2) return false;

            var salt = Convert.FromBase64String(parts[0]);
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32));

            return hash == parts[1];
        }
    }
}
