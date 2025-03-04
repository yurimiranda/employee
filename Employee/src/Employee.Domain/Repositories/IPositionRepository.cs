using Employee.Domain.Abstractions.Interfaces;
using Employee.Domain.Models;

namespace Employee.Domain.Repositories;

public interface IPositionRepository : IRepository<PositionModel, int>
{
}