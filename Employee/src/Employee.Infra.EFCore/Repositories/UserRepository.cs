using Employee.Domain.Models;
using Employee.Domain.Repositories;
using Employee.Infra.EFCore.Abstractions;

namespace Employee.Infra.EFCore.Repositories;

public class UserRepository(ApplicationDbContext context)
    : Repository<ApplicationDbContext, UserModel, Guid>(context), IUserRepository
{
    public Task<UserModel> GetByEmployee(Guid id)
    {
        throw new NotImplementedException();
    }
}