using Employee.Application.UseCases.Employee.Requests;
using Employee.Application.UseCases.Employee.Responses;
using Employee.Domain.ResultPattern;

namespace Employee.Application.Interfaces.UseCases;

public interface IEmployeeRetrievalUseCase
{
    Task<Result<IEnumerable<GetEmployeesResponse>, Error>> GetEmployees(GetEmployeesRequest request);
    Task<Result<GetEmployeeResponse, Error>> GetEmployee(int id);
}