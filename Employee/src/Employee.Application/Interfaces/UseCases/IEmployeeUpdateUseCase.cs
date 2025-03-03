using Employee.Application.UseCases.Employee.Requests;
using Employee.Application.UseCases.Employee.Responses;
using Employee.Domain.ResultPattern;

namespace Employee.Application.Interfaces.UseCases;

public interface IEmployeeUpdateUseCase
{
    Task<Result<UpdateEmployeeResponse, Error>> UpdateEmployee(int id, UpdateEmployeeRequest request);
}