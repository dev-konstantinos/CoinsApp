using System.Security.Cryptography;

namespace CoinsApp.BLL.Features.Users;

public sealed class PasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 600_000;

    private const string Version = "v1";
    private const string Algorithm = "pbkdf2-sha256";

    public string Hash(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        var salt = RandomNumberGenerator.GetBytes(SaltSize);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            HashSize);

        return string.Join(
            '$',
            Version,
            Algorithm,
            Iterations,
            Convert.ToBase64String(salt),
            Convert.ToBase64String(hash));
    }
}