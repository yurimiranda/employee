using Employee.Application.UseCases.Employee.Requests;
using Employee.Application.UseCases.Employee.Responses;
using Employee.Domain.Pagination;
using Employee.Domain.ResultPattern;

namespace Employee.Application.Interfaces.UseCases;

public interface IEmployeePagedRetrievalUseCase
{
    Task<Result<PagedResult<GetEmployeesResponse>, Error>> GetEmployees(GetEmployeesRequest request);
}