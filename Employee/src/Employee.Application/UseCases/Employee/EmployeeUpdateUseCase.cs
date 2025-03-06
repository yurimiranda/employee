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

public class EmployeeUpdateUseCase(
    IValidator<UpdateEmployeeRequest> validator,
    IEmployeeRepository employeeRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : UseCaseBase, IEmployeeUpdateUseCase
{
    public async Task<Result<UpdateEmployeeResponse, Error>> UpdateEmployee(Guid id, UpdateEmployeeRequest request)
    {
        var validationResult = await ValidateRequest(validator, request, errorCodePrefix: "UpdateEmployee");
        if (validationResult.IsError)
            return validationResult.GetError();

        var employee = await employeeRepository.Get(id);
        if (employee is null)
            return Error.Throw("Employee.NotFound", string.Format(Messages.NotFound, "Funcionário"));

        employee = request.Adapt(employee);
        employee.Active = true;

        foreach (var phone in request.Phones)
        {
            var phoneToUpdate = employee.Phones.FirstOrDefault(p => p.Id == phone.Id);
            if (phoneToUpdate is not null)
            {
                phoneToUpdate.Number = phone.Number;
                phoneToUpdate.AreaCode = phone.AreaCode;
                phoneToUpdate.IsPrimary = phone.IsPrimary;
                phoneToUpdate.Active = true;
            }
            else
            {
                var phoneModel = phone.Adapt<PhoneModel>();
                phoneModel.Active = true;
                phoneModel.EmployeeId = employee.Id;
                employee.Phones.Add(phoneModel);
            }
        }

        await employeeRepository.Update(employee);

        var user = await userRepository.GetByEmployee(employee.Id);
        user.Active = true;
        user.Username = employee.Email;
        user.Role = request.Role;
        await userRepository.Update(user);

        await unitOfWork.Commit();

        var response = request.Adapt<UpdateEmployeeResponse>();
        response.Id = id;

        return response;
    }
}