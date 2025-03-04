using Employee.Domain.Models;

namespace Employee.Domain.Services;

public interface IUserContextAccessor
{
    UserContextModel UserContext { get; }
}