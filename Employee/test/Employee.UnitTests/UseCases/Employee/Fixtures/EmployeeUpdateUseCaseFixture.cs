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

public class EmployeeUpdateUseCaseFixture
{
    public readonly Mock<IValidator<UpdateEmployeeRequest>> Validator;
    public readonly Mock<IEmployeeRepository> EmployeeRepository;
    public readonly Mock<IUserRepository> UserRepository;
    public readonly Mock<IUnitOfWork> UnitOfWork;

    public readonly IEmployeeUpdateUseCase UseCase;

    public EmployeeUpdateUseCaseFixture()
    {
        var services = new ServiceCollection();
        var mocker = new AutoMocker();

        Validator = mocker.GetMock<IValidator<UpdateEmployeeRequest>>();
        EmployeeRepository = mocker.GetMock<IEmployeeRepository>();
        UserRepository = mocker.GetMock<IUserRepository>();
        UnitOfWork = mocker.GetMock<IUnitOfWork>();

        services.AddScoped(_ => Validator.Object);
        services.AddScoped(_ => EmployeeRepository.Object);
        services.AddScoped(_ => UserRepository.Object);
        services.AddScoped(_ => UnitOfWork.Object);
        services.AddScoped<IEmployeeUpdateUseCase, EmployeeUpdateUseCase>();

        var provider = services.BuildServiceProvider();
        UseCase = provider.GetRequiredService<IEmployeeUpdateUseCase>();
    }

    public UpdateEmployeeRequest GenerateRequest()
    {
        var faker = new Faker<UpdateEmployeeRequest>()
            .RuleFor(o => o.Name, f => f.Name.FirstName())
            .RuleFor(o => o.Surname, f => f.Name.LastName())
            .RuleFor(o => o.Email, f => f.Internet.Email())
            .RuleFor(o => o.ImmediateSupervisor, f => f.Name.FullName())
            .RuleFor(o => o.PositionId, f => f.Random.Int(1, 100))
            .RuleFor(o => o.Role, f => f.PickRandom<Role>())
            .RuleFor(o => o.Phones, f =>
            [
                new UpdateEmployeeRequest.UpdatePhoneRequest
                {
                    Number = f.Phone.PhoneNumber(),
                    AreaCode = f.Random.Int(11, 99).ToString(),
                    IsPrimary = f.Random.Bool(),
                    Id = f.Random.Int(1, 100)
                }
            ]);

        return faker.Generate();
    }

    public EmployeeModel GenerateEmployee(Guid id)
    {
        return new Faker<EmployeeModel>()
            .RuleFor(o => o.Id, f => id)
            .RuleFor(o => o.Name, f => f.Name.FirstName())
            .RuleFor(o => o.Surname, f => f.Name.LastName())
            .RuleFor(o => o.Email, f => f.Internet.Email())
            .RuleFor(o => o.Document, f => f.Person.Cpf(includeFormatSymbols: false))
            .RuleFor(o => o.ImmediateSupervisor, f => f.Name.FullName())
            .RuleFor(o => o.PositionId, f => f.Random.Int(1, 100))
            .RuleFor(o => o.BirthDate, f => f.Date.Past(30, DateTime.Now.AddYears(-18)))
            .RuleFor(o => o.Phones, f =>
            [
                new PhoneModel
                {
                    Number = f.Phone.PhoneNumber(),
                    AreaCode = f.Random.Int(11, 99).ToString(),
                    IsPrimary = f.Random.Bool(),
                    Id = f.Random.Int(1, 100),
                    EmployeeId = id,
                    Active = true
                }
            ])
            .Generate();
    }

    public static UserModel GenerateUser(Guid employeeId)
    {
        var faker = new Faker<UserModel>()
            .RuleFor(u => u.Id, f => Guid.NewGuid())
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.EmailConfirmed, f => f.Random.Bool())
            .RuleFor(u => u.EmployeeId, f => employeeId);

        return faker.Generate();
    }

    public void MockValidateRequest(UpdateEmployeeRequest request, bool isValid)
    {
        Validator.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(isValid ?
                new FluentValidation.Results.ValidationResult { Errors = [] } :
                new FluentValidation.Results.ValidationResult { Errors = [new() { ErrorMessage = "Error message", PropertyName = "InvalidRequest" }] });
    }

    public void MockGetEmployee(Guid id, bool found = true)
    {
        EmployeeRepository.Setup(x => x.Get(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(found ? GenerateEmployee(id) : null);
    }

    public void MockGetUser(Guid employeeId)
    {
        UserRepository.Setup(x => x.GetByEmployee(employeeId))
            .ReturnsAsync(GenerateUser(employeeId));
    }

    public void ResetMocks()
    {
        Validator.Reset();
        EmployeeRepository.Reset();
        UserRepository.Reset();
        UnitOfWork.Reset();
    }
}