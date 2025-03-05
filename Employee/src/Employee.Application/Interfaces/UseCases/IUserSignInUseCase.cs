using Employee.Application.UseCases.User.Requests;
using Employee.Application.UseCases.User.Responses;
using Employee.Domain.ResultPattern;

namespace Employee.Application.Interfaces.UseCases;

public interface IUserSignInUseCase
{
    Task<Result<UserSignInResponse, Error>> SignIn(UserSignInRequest request);
}