using Employee.Domain.Models;
using Employee.Domain.Repositories;
using Employee.Infra.EFCore.Abstractions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Employee.Infra.EFCore.Repositories;

public class UserRepository(ApplicationDbContext context)
    : Repository<ApplicationDbContext, UserModel, Guid>(context), IUserRepository
{
    public async Task<UserModel> GetByEmployee(Guid id)
    {
        return await Context.Users
            .Where(x => x.EmployeeId == id)
            .FirstOrDefaultAsync();
    }

    public async Task<UserContextModel> GetUserContext(Guid id)
    {
        return await Context.Users
            .Where(x => x.Id == id)
            .ProjectToType<UserContextModel>()
            .FirstOrDefaultAsync();
    }
}