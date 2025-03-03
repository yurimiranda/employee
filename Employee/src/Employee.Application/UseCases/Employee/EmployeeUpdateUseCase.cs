using Employee.Application.Abstractions;
using Employee.Application.Interfaces.UseCases;
using Employee.Application.UseCases.Employee.Requests;
using Employee.Application.UseCases.Employee.Responses;
using Employee.Domain.ResultPattern;

namespace Employee.Application.UseCases.Employee;

public class EmployeeUpdateUseCase : UseCaseBase, IEmployeeUpdateUseCase
{
    public Task<Result<UpdateEmployeeResponse, Error>> UpdateEmployee(int id, UpdateEmployeeRequest request)
    {
        throw new NotImplementedException();
    }
}