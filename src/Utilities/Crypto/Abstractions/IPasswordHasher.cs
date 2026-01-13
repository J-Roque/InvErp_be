namespace Crypto.Abstractions;

public interface IPasswordHasher
{
    public (string Hash, string Salt) Hash(string password);

    public bool Verify(string storedHash, string storedSalt, string inputPassword);

}
