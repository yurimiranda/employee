using Bogus;
using Employee.Application.Interfaces.UseCases;
using Employee.Application.UseCases.User;
using Employee.Application.UseCases.User.Requests;
using Employee.Domain.Models;
using Employee.Domain.Repositories;
using Employee.Domain.Services;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Moq.AutoMock;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Employee.UnitTests.UseCases.User.Fixtures;

public class UserSignInUseCaseFixture
{
    public readonly Mock<IValidator<UserSignInRequest>> Validator;
    public readonly Mock<IUserRepository> UserRepository;
    public readonly Mock<ITokenService> TokenService;
    public readonly Mock<IMemoryCache> Cache;
    public readonly IUserSignInUseCase UseCase;

    public UserSignInUseCaseFixture()
    {
        var services = new ServiceCollection();
        var mocker = new AutoMocker();

        Validator = mocker.GetMock<IValidator<UserSignInRequest>>();
        UserRepository = mocker.GetMock<IUserRepository>();
        TokenService = mocker.GetMock<ITokenService>();
        Cache = mocker.GetMock<IMemoryCache>();

        services.AddScoped(_ => Validator.Object);
        services.AddScoped(_ => UserRepository.Object);
        services.AddScoped(_ => TokenService.Object);
        services.AddScoped(_ => Cache.Object);
        services.AddScoped<IUserSignInUseCase, UserSignInUseCase>();

        var provider = services.BuildServiceProvider();
        UseCase = provider.GetRequiredService<IUserSignInUseCase>();
    }

    public UserSignInRequest GenerateRequest()
    {
        return new Faker<UserSignInRequest>()
            .RuleFor(r => r.Username, f => f.Internet.UserName())
            .RuleFor(r => r.Password, f => f.Internet.Password())
            .Generate();
    }

    public UserModel GenerateUser(string username, string password)
    {
        return new Faker<UserModel>()
            .RuleFor(u => u.Id, f => Guid.NewGuid())
            .RuleFor(u => u.Username, f => username)
            .RuleFor(u => u.Password, f => password)
            .RuleFor(u => u.EmailConfirmed, f => f.Random.Bool())
            .RuleFor(u => u.Employee, f => new EmployeeModel
            {
                Id = Guid.NewGuid(),
                Name = f.Person.FullName,
                BirthDate = f.Person.DateOfBirth,
            })
            .Generate();
    }

    public string MockGenerateToken(UserModel? user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Guid.NewGuid().ToString());
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, user!.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            ]),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        TokenService.Setup(x => x.GenerateToken(user))
            .ReturnsAsync(tokenString);
        return tokenString;
    }

    public void MockValidateRequest(UserSignInRequest request, bool isValid)
    {
        Validator.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(isValid ?
                new FluentValidation.Results.ValidationResult { Errors = [] } :
                new FluentValidation.Results.ValidationResult { Errors = [new() { ErrorMessage = "Error message", PropertyName = "InvalidRequest" }] });
    }

    public UserModel? MockGetUser(string username, string password, bool found = true)
    {
        var user = found ? GenerateUser(username, password) : null;
        UserRepository.Setup(x => x.Get(username))
            .ReturnsAsync(user);
        return user;
    }

    public void Reset()
    {
        Validator.Reset();
        UserRepository.Reset();
        TokenService.Reset();
        Cache.Reset();
    }
}