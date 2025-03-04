using Employee.Domain.Abstractions.Interfaces;
using Employee.Domain.Models;
using Employee.Domain.Pagination;

namespace Employee.Domain.Repositories;

public interface IEmployeeRepository : IRepository<EmployeeModel, Guid>
{
    Task<PagedResult<EmployeeModel>> GetPaged(string email, string document, int page, int pageSize);
}