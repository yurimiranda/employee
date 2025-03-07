using Bogus;
using Bogus.Extensions.Brazil;
using Employee.Application.Interfaces.UseCases;
using Employee.Application.UseCases.Employee;
using Employee.Domain.Models;
using Employee.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.AutoMock;

namespace Employee.UnitTests.UseCases.Employee.Fixtures;

public class EmployeeRetrievalUseCaseFixture
{
    public readonly Mock<IEmployeeRepository> EmployeeRepository;
    public readonly Mock<IPositionRepository> PositionRepository;
    public readonly Mock<IPhoneRepository> PhoneRepository;
    public readonly IEmployeeRetrievalUseCase UseCase;

    public EmployeeRetrievalUseCaseFixture()
    {
        var services = new ServiceCollection();
        var mocker = new AutoMocker();

        EmployeeRepository = mocker.GetMock<IEmployeeRepository>();
        PositionRepository = mocker.GetMock<IPositionRepository>();
        PhoneRepository = mocker.GetMock<IPhoneRepository>();

        services.AddScoped(_ => EmployeeRepository.Object);
        services.AddScoped(_ => PositionRepository.Object);
        services.AddScoped(_ => PhoneRepository.Object);
        services.AddScoped<IEmployeeRetrievalUseCase, EmployeeRetrievalUseCase>();

        var provider = services.BuildServiceProvider();
        UseCase = provider.GetRequiredService<IEmployeeRetrievalUseCase>();
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
            .Generate();
    }

    public void MockGetEmployee(Guid id, bool found = true)
    {
        EmployeeRepository.Setup(x => x.Get(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(found ? GenerateEmployee(id) : null);
    }

    public void Reset()
    {
        EmployeeRepository.Reset();
        PositionRepository.Reset();
        PhoneRepository.Reset();
    }
}
