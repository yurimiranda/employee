using Employee.Application.Abstractions;
using Employee.Application.Interfaces.UseCases;
using Employee.Application.UseCases.Employee.Requests;
using Employee.Application.UseCases.Employee.Responses;
using Employee.Domain.ResultPattern;
using Mapster;
using EmployeeModel = Employee.Domain.Models.Employee;

namespace Employee.Application.UseCases.Employee;

public class EmployeeCreationUseCase : UseCaseBase, IEmployeeCreationUseCase
{
    public async Task<Result<CreateEmployeeResponse, Error>> AddEmployee(CreateEmployeeRequest request)
    {
        var model = request.Adapt<EmployeeModel>();
        model.Position = new(request.PositionRoleId);
    }
}