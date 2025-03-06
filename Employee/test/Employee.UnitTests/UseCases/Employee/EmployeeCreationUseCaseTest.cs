using Employee.Application.Resources;
using Employee.Application.UseCases.Employee.Requests;
using Employee.Domain.Extensions;
using Employee.Domain.Models;
using Employee.Domain.Repositories;
using Employee.UnitTests.Extensions;
using Employee.UnitTests.UseCases.Employee.Fixtures;
using FluentAssertions;
using Moq;

namespace Employee.UnitTests.UseCases.Employee;

public class EmployeeCreationUseCaseTest(EmployeeCreationUseCaseFixture fixture) : IClassFixture<EmployeeCreationUseCaseFixture>
{
    [Fact]
    public async Task AddEmployee_WhenRequestIsNull_ShouldReturnError()
    {
        // Arrange
        fixture.ResetMocks();
        CreateEmployeeRequest? request = null;

        // Act
        var result = await fixture.UseCase.CreateEmployee(request);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.GetError().Code.Should().Be("CreateEmployee.NullRequest");
        result.GetError().ErrorMessages.First().Should().Be(Messages.NullRequest);
        fixture.EmployeeRepository.EnsuresNonPersistence<IEmployeeRepository, EmployeeModel, Guid>();
        fixture.UserRepository.EnsuresNonPersistence<IUserRepository, UserModel, Guid>();
    }

    [Fact]
    public async Task AddEmployee_WhenRequestIsInvalid_ShouldReturnError()
    {
        // Arrange
        fixture.ResetMocks();
        var request = fixture.GenerateRequest(underage: false);
        fixture.MockValidateRequest(request, isValid: false);

        // Act
        var result = await fixture.UseCase.CreateEmployee(request);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.GetError().Code.Should().Be("CreateEmployee.InvalidRequest");
        result.GetError().ErrorMessages.First().Should().Be("Error message");
        fixture.EmployeeRepository.EnsuresNonPersistence<IEmployeeRepository, EmployeeModel, Guid>();
        fixture.UserRepository.EnsuresNonPersistence<IUserRepository, UserModel, Guid>();
    }

    [Fact]
    public async Task AddEmployee_WhenUnderage_ShouldReturnError()
    {
        // Arrange
        fixture.ResetMocks();
        var request = fixture.GenerateRequest(underage: true);
        fixture.MockValidateRequest(request, isValid: true);

        // Act
        var result = await fixture.UseCase.CreateEmployee(request);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.GetError().Code.Should().Be("Employee.Underage");
        result.GetError().ErrorMessages.First().Should().Be(Messages.EmployeeUnderage);
        fixture.EmployeeRepository.EnsuresNonPersistence<IEmployeeRepository, EmployeeModel, Guid>();
        fixture.UserRepository.EnsuresNonPersistence<IUserRepository, UserModel, Guid>();
    }

    [Fact]
    public async Task AddEmployee_WhenValid_ShouldReturnSuccess()
    {
        // Arrange
        fixture.ResetMocks();
        var request = fixture.GenerateRequest(underage: false);
        fixture.MockValidateRequest(request, isValid: true);

        // Act
        var result = await fixture.UseCase.CreateEmployee(request);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().NotBeNull();
        result.GetValue().Id.Should().NotBeEmpty();
        result.GetValue().FullName.Should().NotBeNullOrWhiteSpace();
        result.GetValue().Email.Should().NotBeNullOrWhiteSpace();

        fixture.EmployeeRepository.EnsuresNonPersistence<IEmployeeRepository, EmployeeModel, Guid>(insert: false);
        fixture.UserRepository.EnsuresNonPersistence<IUserRepository, UserModel, Guid>(insert: false);

        fixture.EmployeeRepository.Verify(
            x => x.Insert(
                It.Is<EmployeeModel>(employee => EnsureEmployee(employee, request)),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()), Times.Once);

        fixture.UserRepository.Verify(
            x => x.Insert(
                It.Is<UserModel>(user => EnsureUser(user, request)),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()), Times.Once);

        fixture.UnitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    private static bool EnsureUser(UserModel user, CreateEmployeeRequest request)
    {
        user.Id.Should().NotBeEmpty();
        user.Username.Should().Be(request.Email);
        user.EmailConfirmed.Should().BeTrue();
        user.Active.Should().BeTrue();
        user.EmployeeId.Should().NotBeEmpty();
        user.Password.Should().NotBeNullOrWhiteSpace();
        user.Password.Should().Contain(";");
        user.Role.Should().Be(request.Role);

        var userPassword = user.Password.Split(';');
        userPassword.Should().HaveCount(2);
        userPassword[0].IsBase64().Should().BeTrue();
        userPassword[1].IsBase64().Should().BeTrue();

        user.VerifyPassword(request.Password).Should().BeTrue();

        return true;
    }

    private static bool EnsureEmployee(EmployeeModel employee, CreateEmployeeRequest request)
    {
        employee.Id.Should().NotBeEmpty();
        employee.Active.Should().BeTrue();
        employee.BirthDate.Should().Be(request.BirthDate);
        employee.Document.Should().Be(request.Document);
        employee.Email.Should().Be(request.Email);
        employee.ImmediateSupervisor.Should().Be(request.ImmediateSupervisor);
        employee.Name.Should().Be(request.Name);
        employee.PositionId.Should().Be(request.PositionId);
        employee.Surname.Should().Be(request.Surname);

        for (int i = 0; i < request.Phones.Count(); i++)
        {
            employee.Phones.ElementAt(i).Number.Should().Be(request.Phones.ElementAt(i).Number);
            employee.Phones.ElementAt(i).AreaCode.Should().Be(request.Phones.ElementAt(i).AreaCode);
            employee.Phones.ElementAt(i).IsPrimary.Should().Be(request.Phones.ElementAt(i).IsPrimary);
            employee.Phones.ElementAt(i).Active.Should().BeTrue();
        }

        return true;
    }
}