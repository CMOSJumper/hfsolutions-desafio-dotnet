namespace HFSolutions.TestDotNet.Application.Interfaces
{
    public interface ICustomPasswordHasher
    {
        byte[] GenerateHash(string password, byte[] salt);
        string HashPassword(string password);
        bool VerifyPasswordHash(string passwordHash, string password);
    }
}
