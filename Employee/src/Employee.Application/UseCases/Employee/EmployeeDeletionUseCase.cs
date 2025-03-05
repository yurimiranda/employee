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
    IUserRepository userRepository,
    IPhoneRepository phoneRepository,
    IUnitOfWork unitOfWork) : UseCaseBase, IEmployeeDeletionUseCase
{
    public async Task<Result<DeleteEmployeeResponse, Error>> DeleteEmployee(Guid id)
    {
        var employee = await employeeRepository.Get(id);
        if (employee is null)
            return Error.Throw("Employee.NotFound", string.Format(Messages.NotFound, "Funcionário"));

        var user = await userRepository.GetByEmployee(id);
        var phones = await phoneRepository.GetByEmployee(id);

        if (user is not null)
            await userRepository.Delete(user);

        if (phones.Count != 0)
            await phoneRepository.DeleteAll(phones);

        await employeeRepository.Delete(employee);
        await unitOfWork.Commit();

        return new DeleteEmployeeResponse(employee.Id);
    }
}