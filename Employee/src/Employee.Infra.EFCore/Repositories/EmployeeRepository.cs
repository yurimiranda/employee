using Employee.Domain.Models;
using Employee.Domain.Pagination;
using Employee.Domain.Repositories;
using Employee.Infra.EFCore.Abstractions;

namespace Employee.Infra.EFCore.Repositories;

public class EmployeeRepository(ApplicationDbContext context)
    : Repository<ApplicationDbContext, EmployeeModel, Guid>(context), IEmployeeRepository
{
    public Task<PagedResult<EmployeeModel>> GetPaged(string email, string document, int page, int pageSize)
    {
        throw new NotImplementedException();
    }
}