using Employee.Domain.Models;
using Employee.Domain.Repositories;
using Employee.Infra.EFCore.Abstractions;

namespace Employee.Infra.EFCore.Repositories;

public class PositionRepository(ApplicationDbContext context)
    : Repository<ApplicationDbContext, PositionModel, int>(context), IPositionRepository
{
}