using Employee.Domain.Models;
using Employee.Domain.Repositories;
using Employee.Infra.EFCore.Abstractions;

namespace Employee.Infra.EFCore.Repositories;

public class PhoneRepository(ApplicationDbContext context)
    : Repository<ApplicationDbContext, PhoneModel, int>(context), IPhoneRepository
{
    public Task<ICollection<PhoneModel>> GetByEmployee(Guid id)
    {
        throw new NotImplementedException();
    }
}