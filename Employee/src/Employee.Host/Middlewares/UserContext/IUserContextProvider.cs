using Employee.Domain.Models;

namespace Employee.Host.Middlewares.UserContext;

public interface IUserContextProvider
{
    Task<UserContextModel> GenerateUserContext(string userId);
}