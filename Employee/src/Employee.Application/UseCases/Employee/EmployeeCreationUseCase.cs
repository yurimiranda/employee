using Employee.Application.Abstractions;
using Employee.Application.Interfaces.UseCases;
using Employee.Application.UseCases.Employee.Requests;
using Employee.Application.UseCases.Employee.Responses;
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

        // Save model to database

        return new CreateEmployeeResponse
        {
            Id = model.Id,
            Name = model.Name,
            Surname = model.Surname,
            Email = model.Email
        };
    }
}