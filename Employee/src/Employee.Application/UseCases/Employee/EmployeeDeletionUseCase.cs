using Employee.Application.Abstractions;
using Employee.Application.Interfaces.UseCases;
using Employee.Application.UseCases.Employee.Responses;
using Employee.Domain.ResultPattern;

namespace Employee.Application.UseCases.Employee;

public class EmployeeDeletionUseCase : UseCaseBase, IEmployeeDeletionUseCase
{
    public Task<Result<DeleteEmployeeResponse, Error>> DeleteEmployee(int id)
    {
        throw new NotImplementedException();
    }
}