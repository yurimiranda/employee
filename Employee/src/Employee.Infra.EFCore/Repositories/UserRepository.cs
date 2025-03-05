using Employee.Domain.Models;
using Employee.Domain.Repositories;
using Employee.Infra.EFCore.Abstractions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Employee.Infra.EFCore.Repositories;

public class UserRepository(ApplicationDbContext context)
    : Repository<ApplicationDbContext, UserModel, Guid>(context), IUserRepository
{
    public async Task<UserModel> Get(string username)
    {
        return await Context.Users
            .Include(x => x.Employee)
            .Where(x => x.Username == username)
            .Where(x => x.EmailConfirmed)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

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

    public async Task UpdatePassword(Guid id, string password)
    {
        var user = await Context.Users.FindAsync(id);
        if (user is not null)
        {
            user.Password = password;
            Context.Entry(user).Property(u => u.Password).IsModified = true;
            await Context.SaveChangesAsync();
        }
    }
}