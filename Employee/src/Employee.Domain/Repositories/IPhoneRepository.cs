using Employee.Domain.Abstractions.Interfaces;
using Employee.Domain.Models;

namespace Employee.Domain.Repositories;

public interface IPhoneRepository : IRepository<PhoneModel, int>
{
    Task<ICollection<PhoneModel>> GetByEmployee(Guid id);
}