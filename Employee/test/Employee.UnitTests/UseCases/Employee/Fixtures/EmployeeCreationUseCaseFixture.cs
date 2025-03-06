using Bogus;
using Bogus.Extensions.Brazil;
using Employee.Application.Interfaces.UseCases;
using Employee.Application.UseCases.Employee;
using Employee.Application.UseCases.Employee.Requests;
using Employee.Domain.Abstractions.Interfaces;
using Employee.Domain.Enums;
using Employee.Domain.Models;
using Employee.Domain.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.AutoMock;

namespace Employee.UnitTests.UseCases.Employee.Fixtures;

public class EmployeeCreationUseCaseFixture
{
    public readonly Mock<IValidator<CreateEmployeeRequest>> Validator;
    public readonly Mock<IEmployeeRepository> EmployeeRepository;
    public readonly Mock<IUserRepository> UserRepository;
    public readonly Mock<IUnitOfWork> UnitOfWork;

    public readonly IEmployeeCreationUseCase UseCase;

    public EmployeeCreationUseCaseFixture()
    {
        var services = new ServiceCollection();
        var mocker = new AutoMocker();

        Validator = mocker.GetMock<IValidator<CreateEmployeeRequest>>();
        EmployeeRepository = mocker.GetMock<IEmployeeRepository>();
        UserRepository = mocker.GetMock<IUserRepository>();
        UnitOfWork = mocker.GetMock<IUnitOfWork>();

        services.AddScoped(_ => Validator.Object);
        services.AddScoped(_ => EmployeeRepository.Object);
        services.AddScoped(_ => UserRepository.Object);
        services.AddScoped(_ => UnitOfWork.Object);
        services.AddScoped<IEmployeeCreationUseCase, EmployeeCreationUseCase>();

        var provider = services.BuildServiceProvider();
        UseCase = provider.GetRequiredService<IEmployeeCreationUseCase>();
    }

    public CreateEmployeeRequest GenerateRequest(bool underage)
    {
        var faker = new Faker<CreateEmployeeRequest>()
            .RuleFor(o => o.Name, f => f.Name.FirstName())
            .RuleFor(o => o.Surname, f => f.Name.LastName())
            .RuleFor(o => o.Email, f => f.Internet.Email())
            .RuleFor(o => o.Document, f => f.Person.Cpf(includeFormatSymbols: false))
            .RuleFor(o => o.ImmediateSupervisor, f => f.Name.FullName())
            .RuleFor(o => o.PositionId, f => f.Random.Int(1, 100))
            .RuleFor(o => o.Role, f => f.PickRandom<Role>())
            .RuleFor(o => o.BirthDate, f => f.Date.Past(15, underage ? DateTime.Now : DateTime.Now.AddYears(-18)))
            .RuleFor(o => o.Phones, f =>
            [
                new CreateEmployeeRequest.CreatePhoneRequest
                {
                    Number = f.Phone.PhoneNumber(),
                    AreaCode = f.Random.Int(11, 99).ToString(),
                    IsPrimary = f.Random.Bool()
                }
            ])
            .RuleFor(o => o.Password, f => f.Internet.Password());

        return faker.Generate();
    }

    public void MockValidateRequest(CreateEmployeeRequest request, bool isValid)
    {
        Validator.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(isValid ?
                new FluentValidation.Results.ValidationResult { Errors = [] } :
                new FluentValidation.Results.ValidationResult { Errors = [new() { ErrorMessage = "Error message", PropertyName = "InvalidRequest" }] });
    }

    public void ResetMocks()
    {
        Validator.Reset();
        EmployeeRepository.Reset();
        UserRepository.Reset();
        UnitOfWork.Reset();
    }
}