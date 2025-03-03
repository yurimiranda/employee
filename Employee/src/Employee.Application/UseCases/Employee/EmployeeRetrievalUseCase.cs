using Employee.Application.Abstractions;
using Employee.Application.Interfaces.UseCases;
using Employee.Application.UseCases.Employee.Requests;
using Employee.Application.UseCases.Employee.Responses;
using Employee.Domain.ResultPattern;

namespace Employee.Application.UseCases.Employee;

public class EmployeeRetrievalUseCase : UseCaseBase, IEmployeeRetrievalUseCase
{
    public Task<Result<GetEmployeeResponse, Error>> GetEmployee(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<GetEmployeesResponse>, Error>> GetEmployees(GetEmployeesRequest request)
    {
        throw new NotImplementedException();
    }
}