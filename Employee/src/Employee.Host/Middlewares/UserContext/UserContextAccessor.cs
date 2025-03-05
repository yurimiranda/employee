using Employee.Domain.Models;
using Employee.Domain.Repositories;
using Employee.Domain.Services;

namespace Employee.Host.Middlewares.UserContext;

public class UserContextAccessor(IUserRepository userRepository) : IUserContextAccessor, IUserContextProvider
{
    private static readonly AsyncLocal<UserContextModel> _userContext = new();

    public UserContextModel UserContext
    {
        get => _userContext.Value;
        set => _userContext.Value = value;
    }

    public async Task<UserContextModel> GenerateUserContext(string userId)
    {
        var user = await userRepository.GetUserContext(Guid.Parse(userId));
        _userContext.Value = user;
        return user;
    }
}