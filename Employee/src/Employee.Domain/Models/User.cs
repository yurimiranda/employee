using Employee.Domain.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace Employee.Domain.Models;

public class User : Entity<Guid>
{
    public string Username { get; set; }
    public bool EmailConfirmed { get; set; }
    public Employee Employee { get; set; }

    public string Password
    {
        get => HashPassword(_password);
        set => _password = value;
    }

    public bool Verify(string inputPassword)
    {
        var elements = _password.Split(_delimiter);
        var salt = Convert.FromBase64String(elements[0]);
        var hash = Convert.FromBase64String(elements[1]);

        var hashInput = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(inputPassword),
            salt,
            _iterations,
            _hashAlgorithm,
            _keySize);

        return CryptographicOperations.FixedTimeEquals(hash, hashInput);
    }

    private static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(_keySize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            _iterations,
            _hashAlgorithm,
            _keySize);

        return string.Join(_delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    private string _password;
    private const int _keySize = 64;
    private const int _iterations = 350000;
    private const char _delimiter = ';';
    private static readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;
}