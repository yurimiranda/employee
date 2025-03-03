using Employee.Domain.Abstractions.Interfaces;
using Employee.Domain.Models;

namespace Employee.Domain.Repositories;

public interface IPositionRoleRepository : IRepository<PositionRole, int>
{
}