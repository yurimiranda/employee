using Employee.Domain.Abstractions.Interfaces;
using Employee.Domain.Models;

namespace Employee.Domain.Repositories;

public interface IUserRepository : IRepository<UserModel, Guid>
{
    Task<UserModel> GetByEmployee(Guid id);
    Task<UserContextModel> GetUserContext(Guid id);
    Task UpdatePassword(Guid id, string password);
    Task<UserModel> Get(string username);
}