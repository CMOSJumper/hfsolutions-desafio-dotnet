using System.Security.Cryptography;
using HFSolutions.TestDotNet.Application.Interfaces;

namespace HFSolutions.TestDotNet.Application.Services
{
    public class PasswordHasher : ICustomPasswordHasher
    {
        private const int _saltSize = 128 / 8;
        private const int _hashSize = 256 / 8;
        private const int _iterations = 100000;

        public byte[] GenerateHash(string password, byte[] salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, _iterations, HashAlgorithmName.SHA512);

            return pbkdf2.GetBytes(_hashSize);
        }

        public string HashPassword(string password)
        {
            using var rng = RandomNumberGenerator.Create();

            byte[] salt = new byte[_saltSize];
            rng.GetBytes(salt);

            var hash = GenerateHash(password, salt);

            byte[] hashBytes = new byte[_saltSize + _hashSize];
            Buffer.BlockCopy(salt, 0, hashBytes, 0, _saltSize);
            Buffer.BlockCopy(hash, 0, hashBytes, _saltSize, _hashSize);

            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyPasswordHash(string passwordHash, string password)
        {
            byte[] hashBytes = Convert.FromBase64String(passwordHash);

            byte[] salt = new byte[_saltSize];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, _saltSize);

            var hash = GenerateHash(password, salt);

            for (int i = 0; i < _hashSize; i++)
            {
                if (hashBytes[i + _saltSize] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
