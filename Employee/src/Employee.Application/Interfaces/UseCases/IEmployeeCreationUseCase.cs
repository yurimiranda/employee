using Employee.Application.UseCases.Employee.Requests;
using Employee.Application.UseCases.Employee.Responses;
using Employee.Domain.ResultPattern;

namespace Employee.Application.Interfaces.UseCases;

public interface IEmployeeCreationUseCase
{
    Task<Result<CreateEmployeeResponse, Error>> AddEmployee(CreateEmployeeRequest request);
}