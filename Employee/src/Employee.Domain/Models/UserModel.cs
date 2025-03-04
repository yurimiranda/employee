using Employee.Domain.Abstractions;
using Employee.Domain.Enums;
using Employee.Domain.Extensions;
using System.Security.Cryptography;
using System.Text;

namespace Employee.Domain.Models;

public class UserModel : Entity<Guid>
{
    public Role Role { get; set; }
    public string Username { get; set; }
    public bool EmailConfirmed { get; set; }
    public Guid EmployeeId { get; set; }
    public EmployeeModel Employee { get; set; }

    public string Password
    {
        get => HashPassword(_password);
        set => _password = value;
    }

    public bool VerifyPassword(string inputPassword)
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
        if (string.IsNullOrEmpty(password))
            return null;
        if (password.Contains(_delimiter) && password.Split(_delimiter)[0].IsBase64())
            return password;

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