using Employee.Domain.Models;
using Employee.Domain.Repositories;
using Employee.Infra.EFCore.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Employee.Infra.EFCore.Repositories;

public class PhoneRepository(ApplicationDbContext context)
    : Repository<ApplicationDbContext, PhoneModel, int>(context), IPhoneRepository
{
    public async Task<ICollection<PhoneModel>> GetByEmployee(Guid id)
    {
        return await Context.Phones
            .Where(p => p.EmployeeId == id)
            .ToListAsync();
    }
}