using Employee.Application.Abstractions;
using Employee.Application.Interfaces.UseCases;
using Employee.Application.Resources;
using Employee.Application.UseCases.Employee.Requests;
using Employee.Application.UseCases.Employee.Responses;
using Employee.Domain.Abstractions.Interfaces;
using Employee.Domain.Repositories;
using Employee.Domain.ResultPattern;
using FluentValidation;
using Mapster;

namespace Employee.Application.UseCases.Employee;

public class EmployeeUpdateUseCase(
    IValidator<UpdateEmployeeRequest> validator,
    IEmployeeRepository employeeRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : UseCaseBase, IEmployeeUpdateUseCase
{
    public async Task<Result<UpdateEmployeeResponse, Error>> UpdateEmployee(Guid id, UpdateEmployeeRequest request)
    {
        var employee = await employeeRepository.Get(id);
        if (employee is null)
            return Error.Throw("Employee.NotFound", string.Format(Messages.NotFound, "Funcionário"));

        var validationResult = await ValidateRequest(validator, request, errorCodePrefix: "UpdateEmployee");
        if (validationResult.IsError)
            return validationResult.GetError();

        employee = request.Adapt(employee);

        await employeeRepository.Update(employee);

        var user = await userRepository.GetByEmployee(employee.Id);
        user.Username = employee.Email;
        user.Role = request.Role;
        await userRepository.Update(user);

        await unitOfWork.Commit();

        var response = request.Adapt<UpdateEmployeeResponse>();
        response.Id = id;

        return response;
    }
}