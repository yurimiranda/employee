using Employee.Application.UseCases.Employee.Responses;
using Employee.Domain.ResultPattern;

namespace Employee.Application.Interfaces.UseCases;

public interface IEmployeeDeletionUseCase
{
    Task<Result<DeleteEmployeeResponse, Error>> DeleteEmployee(Guid id);
}