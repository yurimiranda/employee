using Carter;
using Employee.Application.Interfaces.UseCases;
using Employee.Application.UseCases.Employee.Requests;
using Employee.Application.UseCases.Employee.Responses;
using Employee.Domain.Pagination;
using Employee.Domain.ResultPattern;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.AspNetCore.Routing;

public sealed class EmployeeEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/employee", async (
            [FromServices] IEmployeePagedRetrievalUseCase useCase,
            [AsParameters] GetEmployeesRequest request) =>
        {
            var result = await useCase.GetEmployees(request);
            return result.Match(
                response => Results.Ok(response),
                error => Results.BadRequest(error));
        })
        .WithTags("Employee")
        .ConfigureRoute<PagedResult<GetEmployeesResponse>, Error>(
            StatusCodes.Status200OK,
            StatusCodes.Status400BadRequest);

        app.MapGet("/api/employee/{id}", async (
            Guid id,
            [FromServices] IEmployeeRetrievalUseCase useCase) =>
        {
            var result = await useCase.GetEmployee(id);
            return result.Match(
                response => Results.Ok(response),
                error => Results.BadRequest(error));
        })
        .WithTags("Employee")
        .ConfigureRoute<GetEmployeeResponse, Error>(
            StatusCodes.Status200OK,
            StatusCodes.Status400BadRequest);

        app.MapPost("/api/employee", async (
            [FromServices] IEmployeeCreationUseCase useCase,
            [FromBody] CreateEmployeeRequest employee) =>
        {
            var result = await useCase.CreateEmployee(employee);
            return result.Match(
                response => Results.Ok(response),
                error => Results.BadRequest(error));
        })
        .WithTags("Employee")
        .ConfigureRoute<CreateEmployeeResponse, Error>(
            StatusCodes.Status200OK,
            StatusCodes.Status400BadRequest);

        app.MapPut("/api/employee/{id}", async (
            Guid id,
            [FromServices] IEmployeeUpdateUseCase useCase,
            [FromBody] UpdateEmployeeRequest employee) =>
        {
            var result = await useCase.UpdateEmployee(id, employee);
            return result.Match(
                response => Results.Ok(response),
                error => Results.BadRequest(error));
        })
        .WithTags("Employee")
        .ConfigureRoute<UpdateEmployeeResponse, Error>(
            StatusCodes.Status200OK,
            StatusCodes.Status400BadRequest);

        app.MapDelete("/api/employee/{id}", async (
            Guid id,
            [FromServices] IEmployeeDeletionUseCase useCase) =>
        {
            var result = await useCase.DeleteEmployee(id);
            return result.Match(
                response => Results.Ok(response),
                error => Results.BadRequest(error));
        })
        .WithTags("Employee")
        .ConfigureRoute<DeleteEmployeeResponse, Error>(
            StatusCodes.Status200OK,
            StatusCodes.Status400BadRequest);
    }
}
