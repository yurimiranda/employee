using Bogus;
using Bogus.Extensions.Brazil;
using Employee.Application.Interfaces.UseCases;
using Employee.Application.UseCases.Employee;
using Employee.Application.UseCases.Employee.Requests;
using Employee.Domain.Models;
using Employee.Domain.Pagination;
using Employee.Domain.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.AutoMock;

namespace Employee.UnitTests.UseCases.Employee.Fixtures;

public class EmployeePagedRetrievalUseCaseFixture
{
    public readonly Mock<IValidator<GetEmployeesRequest>> Validator;
    public readonly Mock<IEmployeeRepository> EmployeeRepository;
    public readonly IEmployeePagedRetrievalUseCase UseCase;

    public EmployeePagedRetrievalUseCaseFixture()
    {
        var services = new ServiceCollection();
        var mocker = new AutoMocker();

        Validator = mocker.GetMock<IValidator<GetEmployeesRequest>>();
        EmployeeRepository = mocker.GetMock<IEmployeeRepository>();

        services.AddScoped(_ => Validator.Object);
        services.AddScoped(_ => EmployeeRepository.Object);
        services.AddScoped<IEmployeePagedRetrievalUseCase, EmployeePagedRetrievalUseCase>();

        var provider = services.BuildServiceProvider();
        UseCase = provider.GetRequiredService<IEmployeePagedRetrievalUseCase>();
    }

    public GetEmployeesRequest GenerateRequest()
    {
        return new Faker<GetEmployeesRequest>()
            .RuleFor(r => r.Email, f => f.Internet.Email())
            .RuleFor(r => r.Document, f => f.Person.Cpf(includeFormatSymbols: false))
            .RuleFor(r => r.Page, f => f.Random.Int(1, 10))
            .RuleFor(r => r.PageSize, f => f.Random.Int(1, 10))
            .Generate();
    }

    public void MockValidateRequest(GetEmployeesRequest request, bool isValid)
    {
        Validator.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(isValid ?
                new FluentValidation.Results.ValidationResult { Errors = [] } :
                new FluentValidation.Results.ValidationResult { Errors = [new() { ErrorMessage = "Error message", PropertyName = "InvalidRequest" }] });
    }

    public PagedResult<EmployeeModel> GeneratePagedResult(int page, int pageSize)
    {
        var employess = new Faker<EmployeeModel>()
            .RuleFor(o => o.Id, f => Guid.NewGuid())
            .RuleFor(o => o.Name, f => f.Name.FirstName())
            .RuleFor(o => o.Surname, f => f.Name.LastName())
            .RuleFor(o => o.Email, f => f.Internet.Email())
            .RuleFor(o => o.Document, f => f.Person.Cpf(includeFormatSymbols: false))
            .RuleFor(o => o.ImmediateSupervisor, f => f.Name.FullName())
            .RuleFor(o => o.PositionId, f => f.Random.Int(1, 100))
            .RuleFor(o => o.BirthDate, f => f.Date.Past(30, DateTime.Now.AddYears(-18)))
            .RuleFor(o => o.Phones, (f, c) =>
            [
                new PhoneModel
                {
                    Number = f.Phone.PhoneNumber(),
                    AreaCode = f.Random.Int(11, 99).ToString(),
                    IsPrimary = f.Random.Bool(),
                    Id = f.Random.Int(1, 100),
                    EmployeeId = c.Id,
                    Active = true
                }
            ])
            .Generate((pageSize + 10) * page);

        var paged = employess.GetPagedNumbers(page, pageSize);
        paged.Results = [.. employess.Skip(paged.Skip).Take(pageSize)];
        return paged;
    }

    public void MockGetPaged(GetEmployeesRequest request)
    {
        EmployeeRepository.Setup(x => x.GetPaged(request.Email, request.Document, request.Page, request.PageSize))
            .ReturnsAsync(GeneratePagedResult(request.Page, request.PageSize));
    }

    public void Reset()
    {
        Validator.Reset();
        EmployeeRepository.Reset();
    }
}
