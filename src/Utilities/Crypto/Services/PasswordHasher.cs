using System.Security.Cryptography;
using System.Text;
using Crypto.Abstractions;
using Konscious.Security.Cryptography;

namespace Crypto.Services;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16; // Tamaño del salt en bytes
    private const int HashSize = 32; // Tamaño del hash en bytes
    private const int Iterations = 4; // Número de iteraciones
    private const int MemorySize = 65536; // Memoria en KB
    private const int Parallelism = 2; // Número de hilos

    public (string Hash, string Salt) Hash(string password)
    {
        // Generar un salt aleatorio
        var salt = GenerateSalt(SaltSize);

        // Crear el hash usando Argon2
        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            Iterations = Iterations,
            MemorySize = MemorySize,
            DegreeOfParallelism = Parallelism
        };

        // Generar el hash
        var hash = argon2.GetBytes(HashSize);

        // Retornar el hash y el salt como strings codificados en Base64
        return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
    }

    public bool Verify(string storedHash, string storedSalt, string inputPassword)
    {
        // Decodificar el salt y el hash almacenados
        var salt = Convert.FromBase64String(storedSalt);
        var hash = Convert.FromBase64String(storedHash);

        // Crear un nuevo hash con el salt almacenado y la contraseña ingresada
        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(inputPassword))
        {
            Salt = salt,
            Iterations = Iterations,
            MemorySize = MemorySize,
            DegreeOfParallelism = Parallelism
        };

        var newHash = argon2.GetBytes(HashSize);

        // Comparar los hashes
        return CryptographicEquals(hash, newHash);
    }

    private static byte[] GenerateSalt(int size)
    {
        var salt = new byte[size];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }

    private static bool CryptographicEquals(byte[] a, byte[] b)
    {
        var result = a.Length == b.Length;

        for (var i = 0; i < a.Length && i < b.Length; i++)
        {
            result &= a[i] == b[i];
        }

        return result;
    }
}
