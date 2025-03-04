using Employee.Application.Abstractions;
using Employee.Application.Interfaces.UseCases;
using Employee.Application.Resources;
using Employee.Application.UseCases.Employee.Requests;
using Employee.Application.UseCases.Employee.Responses;
using Employee.Domain.Models;
using Employee.Domain.ResultPattern;
using FluentValidation;
using Mapster;
using EmployeeModel = Employee.Domain.Models.Employee;

namespace Employee.Application.UseCases.Employee;

public class EmployeeCreationUseCase(
    IValidator<CreateEmployeeRequest> validator)
    : UseCaseBase, IEmployeeCreationUseCase
{
    public async Task<Result<CreateEmployeeResponse, Error>> AddEmployee(CreateEmployeeRequest request)
    {
        var validationResult = await ValidateRequest(validator, request);
        if (validationResult.IsError)
            return validationResult.GetError();

        var model = request.Adapt<EmployeeModel>();
        model.Position = new(request.PositionRoleId);
        model.Id = Guid.NewGuid();
        model.Active = true;

        if (!model.OfLegalAge)
            return Error.Throw("Employee.Underage", Messages.EmployeeUnderage);

        var employeeUser = new User
        {
            Id = Guid.NewGuid(),
            Username = model.Email,
            EmailConfirmed = true,
            Active = true,
            Employee = model,
            Password = request.Password
        };

        // Save model to database

        return new CreateEmployeeResponse
        {
            Id = model.Id,
            FullName = model.Name + " " + model.Surname,
            Email = model.Email
        };
    }
}