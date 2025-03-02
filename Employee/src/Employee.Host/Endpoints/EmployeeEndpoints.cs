using Carter;
using Employee.Application.Interfaces.UseCases;
using Employee.Application.UseCases.Employee.Requests;
using Employee.Application.UseCases.Employee.Responses;
using Employee.Domain.ResultPattern;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.AspNetCore.Routing;

public sealed class EmployeeEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGet("/api/employee", async (
                [FromServices] IEmployeeRetrievalUseCase useCase,
                [AsParameters] GetEmployeesRequest request) =>
            {
                var result = await useCase.GetEmployees(request);
                return result.Match(
                    response => Results.Ok(response),
                    error => Results.BadRequest(error));
            })
            .WithTags("Employee")
            .ConfigureRoute<IEnumerable<GetEmployeesResponse>, Error>(
                StatusCodes.Status200OK,
                StatusCodes.Status400BadRequest);

        endpoints
            .MapGet("/api/employee/{id}", async (int id, [FromServices] IEmployeeRetrievalUseCase useCase) =>
            {
                var result = await useCase.GetEmployee(id);
                return result.Match(
                    response => Results.Ok(response),
                    error => Results.BadRequest(error));
            })
            .WithTags("Employee")
            .ConfigureRoute<IEnumerable<GetEmployeeResponse>, Error>(
                StatusCodes.Status200OK,
                StatusCodes.Status400BadRequest);

        endpoints
            .MapPost("/api/employee", async (CreateEmployeeRequest employee) => await controller.AddEmployee(employee))
            .WithTags("Employee")
            .ConfigureRoute<IEnumerable<CreateEmployeeResponse>, Error>(
                StatusCodes.Status200OK,
                StatusCodes.Status400BadRequest);

        endpoints
            .MapPatch("/api/employee/{id}", async (int id, UpdateEmployeeRequest employee) => await controller.UpdateEmployee(id, employee))
            .WithTags("Employee")
            .ConfigureRoute<IEnumerable<UpdateEmployeeResponse>, Error>(
                StatusCodes.Status200OK,
                StatusCodes.Status400BadRequest);

        endpoints
            .MapDelete("/api/employee/{id}", async (int id) => await controller.DeleteEmployee(id))
            .WithTags("Employee")
            .ConfigureRoute<IEnumerable<DeleteEmployeeResponse>, Error>(
                StatusCodes.Status200OK,
                StatusCodes.Status400BadRequest);
    }
}