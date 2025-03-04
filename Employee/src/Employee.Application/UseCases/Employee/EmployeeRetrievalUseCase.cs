using Employee.Application.Abstractions;
using Employee.Application.Interfaces.UseCases;
using Employee.Application.Resources;
using Employee.Application.UseCases.Employee.Responses;
using Employee.Domain.Repositories;
using Employee.Domain.ResultPattern;
using Mapster;

namespace Employee.Application.UseCases.Employee;

public class EmployeeRetrievalUseCase(
    IPositionRepository positionRepository,
    IEmployeeRepository employeeRepository,
    IPhoneRepository phoneRepository) : UseCaseBase, IEmployeeRetrievalUseCase
{
    public async Task<Result<GetEmployeeResponse, Error>> GetEmployee(Guid id)
    {
        var employee = await employeeRepository.Get(id);
        if (employee is null)
            return Error.Throw("Employee.NotFound", string.Format(Messages.NotFound, "Funcionário"));

        var position = await positionRepository.Get(employee.PositionId);
        employee.Position = position;

        var phones = await phoneRepository.GetByEmployee(employee.Id);
        employee.Phones = phones;

        return employee.Adapt<GetEmployeeResponse>();
    }
}