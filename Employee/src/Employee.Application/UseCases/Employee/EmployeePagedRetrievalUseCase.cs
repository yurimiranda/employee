using Employee.Application.Abstractions;
using Employee.Application.Interfaces.UseCases;
using Employee.Application.UseCases.Employee.Requests;
using Employee.Application.UseCases.Employee.Responses;
using Employee.Domain.Pagination;
using Employee.Domain.Repositories;
using Employee.Domain.ResultPattern;
using FluentValidation;
using Mapster;

namespace Employee.Application.UseCases.Employee;

public class EmployeePagedRetrievalUseCase(
    IValidator<GetEmployeesRequest> validator,
    IEmployeeRepository employeeRepository) : UseCaseBase, IEmployeePagedRetrievalUseCase
{
    public async Task<Result<PagedResult<GetEmployeesResponse>, Error>> GetEmployees(GetEmployeesRequest request)
    {
        var validationResult = await ValidateRequest(validator, request);
        if (validationResult.IsError)
            return validationResult.GetError();

        var pagedResult = await employeeRepository.GetPaged(request.Email, request.Document, request.Page, request.PageSize);
        var response = pagedResult.Adapt<PagedResult<GetEmployeesResponse>>();
        return response;
    }
}