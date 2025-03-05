using Employee.Application.Abstractions;
using Employee.Application.Interfaces.UseCases;
using Employee.Application.Resources;
using Employee.Application.UseCases.User.Requests;
using Employee.Application.UseCases.User.Responses;
using Employee.Domain.Repositories;
using Employee.Domain.ResultPattern;
using Employee.Domain.Services;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Employee.Application.UseCases.User;

public class UserSignInUseCase(
    ILogger<UserSignInUseCase> logger,
    IValidator<UserSignInRequest> validator,
    IUserRepository userRepository,
    ITokenService tokenService,
    IMemoryCache cache) : UseCaseBase, IUserSignInUseCase
{
    public async Task<Result<UserSignInResponse, Error>> SignIn(UserSignInRequest request)
    {
        logger.LogInformation("UserSignInUseCase.SignIn {@Request}", request);
        var validationResult = await ValidateRequest(validator, request, errorCodePrefix: "SignIn.Validation");
        if (validationResult.IsError)
            return validationResult.GetError();

        var user = await userRepository.Get(request.Username);
        if (user is null)
            return Error.Throw("SignIn.InvalidCredentials", Messages.InvalidCredentials);

        if (!user.VerifyPassword(request.Password))
            return Error.Throw("SignIn.InvalidCredentials", Messages.InvalidCredentials);

        var token = await tokenService.GenerateToken(user);

        cache.Remove("User" + user.Id);

        return new UserSignInResponse
        {
            Name = user.Employee.Name,
            Token = token
        };
    }
}