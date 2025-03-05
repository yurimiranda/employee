using Employee.Domain.Models;

namespace Employee.Domain.Services;

public interface ITokenService
{
    Task<string> GenerateToken(UserModel user);
}