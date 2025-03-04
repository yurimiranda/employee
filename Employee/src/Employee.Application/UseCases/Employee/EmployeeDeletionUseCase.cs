using Employee.Application.Abstractions;
using Employee.Application.Interfaces.UseCases;
using Employee.Application.Resources;
using Employee.Application.UseCases.Employee.Responses;
using Employee.Domain.Abstractions.Interfaces;
using Employee.Domain.Repositories;
using Employee.Domain.ResultPattern;

namespace Employee.Application.UseCases.Employee;

public class EmployeeDeletionUseCase(
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork) : UseCaseBase, IEmployeeDeletionUseCase
{
    public async Task<Result<DeleteEmployeeResponse, Error>> DeleteEmployee(Guid id)
    {
        var employee = await employeeRepository.Get(id);
        if (employee is null)
            return Error.Throw("Employee.NotFound", string.Format(Messages.NotFound, "Funcionário"));

        await employeeRepository.Delete(employee);
        await unitOfWork.Commit();

        return new DeleteEmployeeResponse(employee.Id);
    }
}