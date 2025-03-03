using Employee.Domain.Models;
using Employee.Domain.Repositories;
using Employee.Infra.EFCore.Abstractions;

namespace Employee.Infra.EFCore.Repositories;

public class PositionRoleRepository(ApplicationDbContext context) : Repository<ApplicationDbContext, PositionRole, int>(context), IPositionRoleRepository
{
}