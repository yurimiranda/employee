using Bogus;
using Bogus.Extensions.Brazil;
using Employee.Application.Interfaces.UseCases;
using Employee.Application.UseCases.Employee;
using Employee.Domain.Abstractions.Interfaces;
using Employee.Domain.Models;
using Employee.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.AutoMock;

namespace Employee.UnitTests.UseCases.Employee.Fixtures;

public class EmployeeDeletionUseCaseFixture
{
    public readonly Mock<IEmployeeRepository> EmployeeRepository;
    public readonly Mock<IUserRepository> UserRepository;
    public readonly Mock<IPhoneRepository> PhoneRepository;
    public readonly Mock<IUnitOfWork> UnitOfWork;

    public readonly IEmployeeDeletionUseCase UseCase;

    public EmployeeDeletionUseCaseFixture()
    {
        var services = new ServiceCollection();
        var mocker = new AutoMocker();

        EmployeeRepository = mocker.GetMock<IEmployeeRepository>();
        UserRepository = mocker.GetMock<IUserRepository>();
        PhoneRepository = mocker.GetMock<IPhoneRepository>();
        UnitOfWork = mocker.GetMock<IUnitOfWork>();

        services.AddScoped(_ => EmployeeRepository.Object);
        services.AddScoped(_ => UserRepository.Object);
        services.AddScoped(_ => PhoneRepository.Object);
        services.AddScoped(_ => UnitOfWork.Object);
        services.AddScoped<IEmployeeDeletionUseCase, EmployeeDeletionUseCase>();

        var provider = services.BuildServiceProvider();
        UseCase = provider.GetRequiredService<IEmployeeDeletionUseCase>();
    }

    private static EmployeeModel GenerateEmployee(Guid id)
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
            .Generate();
    }

    private static ICollection<PhoneModel> GetPhoneModels(Guid employeeId)
    {
        return new Faker<PhoneModel>()
            .RuleFor(o => o.Id, f => f.Random.Int(1, 100))
            .RuleFor(o => o.EmployeeId, f => employeeId)
            .RuleFor(o => o.AreaCode, f => f.Random.Int(11, 99).ToString())
            .RuleFor(o => o.Number, f => f.Phone.PhoneNumber())
            .RuleFor(o => o.IsPrimary, f => f.Random.Bool())
            .RuleFor(o => o.Active, f => true)
            .Generate(3);
    }

    private static UserModel GenerateUser(Guid employeeId)
    {
        return new Faker<UserModel>()
            .RuleFor(u => u.Id, f => Guid.NewGuid())
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.EmailConfirmed, f => f.Random.Bool())
            .RuleFor(u => u.EmployeeId, f => employeeId)
            .Generate();
    }

    public void MockGetEmployee(Guid id, bool found = true)
    {
        EmployeeRepository.Setup(x => x.Get(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(found ? GenerateEmployee(id) : null);
    }

    public void MockGetUser(Guid employeeId, bool found = true)
    {
        UserRepository.Setup(x => x.GetByEmployee(employeeId))
            .ReturnsAsync(found ? GenerateUser(employeeId) : null);
    }

    public void MockGetPhones(Guid employeeId, bool found = true)
    {
        PhoneRepository.Setup(x => x.GetByEmployee(employeeId))
            .ReturnsAsync(found ? GetPhoneModels(employeeId) : []);
    }

    public void Reset()
    {
        EmployeeRepository.Reset();
        UserRepository.Reset();
        PhoneRepository.Reset();
        UnitOfWork.Reset();
    }
}
