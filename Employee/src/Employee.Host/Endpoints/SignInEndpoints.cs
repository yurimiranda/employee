using Carter;
using Employee.Application.Interfaces.UseCases;
using Employee.Application.UseCases.User.Requests;
using Employee.Application.UseCases.User.Responses;
using Employee.Domain.ResultPattern;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.AspNetCore.Routing;

public sealed class SignInEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/signin", async (
            [FromServices] IUserSignInUseCase useCase,
            [FromBody] UserSignInRequest request) =>
        {
            var result = await useCase.SignIn(request);
            return result.Match(
                response => Results.Ok(response),
                error => Results.BadRequest(error));
        })
        .WithTags("Login")
        .ConfigureRoute<UserSignInResponse, Error>(
            StatusCodes.Status200OK,
            StatusCodes.Status400BadRequest,
            requireAuthorization: false);
    }
}