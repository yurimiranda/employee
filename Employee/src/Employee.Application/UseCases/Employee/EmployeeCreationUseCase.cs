using Employee.Application.Abstractions;
using Employee.Application.Interfaces.UseCases;
using Employee.Application.Resources;
using Employee.Application.UseCases.Employee.Requests;
using Employee.Application.UseCases.Employee.Responses;
using Employee.Domain.Abstractions.Interfaces;
using Employee.Domain.Models;
using Employee.Domain.Repositories;
using Employee.Domain.ResultPattern;
using FluentValidation;
using Mapster;

namespace Employee.Application.UseCases.Employee;

public class EmployeeCreationUseCase(
    IValidator<CreateEmployeeRequest> validator,
    IEmployeeRepository employeeRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : UseCaseBase, IEmployeeCreationUseCase
{
    public async Task<Result<CreateEmployeeResponse, Error>> AddEmployee(CreateEmployeeRequest request)
    {
        var validationResult = await ValidateRequest(validator, request);
        if (validationResult.IsError)
            return validationResult.GetError();

        var model = request.Adapt<EmployeeModel>();
        model.Id = Guid.NewGuid();
        model.Active = true;

        if (!model.OfLegalAge)
            return Error.Throw("Employee.Underage", Messages.EmployeeUnderage);

        var employeeUser = new UserModel
        {
            Id = Guid.NewGuid(),
            Username = model.Email,
            EmailConfirmed = true,
            Active = true,
            EmployeeId = model.Id,
            Password = request.Password,
            Role = request.Role
        };

        await employeeRepository.Insert(model);
        await userRepository.Insert(employeeUser);
        await unitOfWork.Commit();

        return new CreateEmployeeResponse
        {
            Id = model.Id,
            FullName = model.Name + " " + model.Surname,
            Email = model.Email
        };
    }
}