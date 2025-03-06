using Employee.Application.Resources;
using Employee.Application.UseCases.Employee.Requests;
using Employee.Domain.Models;
using Employee.Domain.Repositories;
using Employee.UnitTests.Extensions;
using Employee.UnitTests.UseCases.Employee.Fixtures;
using FluentAssertions;
using Moq;

namespace Employee.UnitTests.UseCases.Employee;

public class EmployeeUpdateUseCaseTest(EmployeeUpdateUseCaseFixture fixture) : IClassFixture<EmployeeUpdateUseCaseFixture>
{
    [Fact]
    public async Task UpdateEmployee_WhenRequestIsNull_ShouldReturnError()
    {
        // Arrange
        fixture.ResetMocks();
        UpdateEmployeeRequest? request = null;
        var id = Guid.NewGuid();

        // Act
        var result = await fixture.UseCase.UpdateEmployee(id, request);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.GetError().Code.Should().Be("UpdateEmployee.NullRequest");
        result.GetError().ErrorMessages.First().Should().Be(Messages.NullRequest);
        fixture.EmployeeRepository.EnsuresNonPersistence<IEmployeeRepository, EmployeeModel, Guid>();
        fixture.UserRepository.EnsuresNonPersistence<IUserRepository, UserModel, Guid>();
    }

    [Fact]
    public async Task UpdateEmployee_WhenRequestIsInvalid_ShouldReturnError()
    {
        // Arrange
        fixture.ResetMocks();
        var request = fixture.GenerateRequest();
        fixture.MockValidateRequest(request, isValid: false);
        var id = Guid.NewGuid();

        // Act
        var result = await fixture.UseCase.UpdateEmployee(id, request);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.GetError().Code.Should().Be("UpdateEmployee.InvalidRequest");
        result.GetError().ErrorMessages.First().Should().Be("Error message");
        fixture.EmployeeRepository.EnsuresNonPersistence<IEmployeeRepository, EmployeeModel, Guid>();
        fixture.UserRepository.EnsuresNonPersistence<IUserRepository, UserModel, Guid>();
    }

    [Fact]
    public async Task UpdateEmployee_WhenEmployeeNotFound_ShouldReturnError()
    {
        // Arrange
        fixture.ResetMocks();
        var id = Guid.NewGuid();
        var request = fixture.GenerateRequest();
        fixture.MockValidateRequest(request, isValid: true);
        fixture.MockGetEmployee(id, found: false);

        // Act
        var result = await fixture.UseCase.UpdateEmployee(id, request);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.GetError().Code.Should().Be("Employee.NotFound");
        result.GetError().ErrorMessages.First().Should().Be(string.Format(Messages.NotFound, "Funcionário"));
        fixture.EmployeeRepository.EnsuresNonPersistence<IEmployeeRepository, EmployeeModel, Guid>();
        fixture.UserRepository.EnsuresNonPersistence<IUserRepository, UserModel, Guid>();
    }

    [Fact]
    public async Task UpdateEmployee_WhenValid_ShouldReturnSuccess()
    {
        // Arrange
        fixture.ResetMocks();
        var id = Guid.NewGuid();
        var request = fixture.GenerateRequest();
        fixture.MockValidateRequest(request, isValid: true);
        fixture.MockGetEmployee(id, found: true);
        fixture.MockGetUser(id);

        // Act
        var result = await fixture.UseCase.UpdateEmployee(id, request);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().NotBeNull();
        result.GetValue().Id.Should().Be(id);

        fixture.EmployeeRepository.EnsuresNonPersistence<IEmployeeRepository, EmployeeModel, Guid>(update: false);
        fixture.UserRepository.EnsuresNonPersistence<IUserRepository, UserModel, Guid>(update: false);

        fixture.EmployeeRepository.Verify(
            x => x.Update(
                It.Is<EmployeeModel>(employee => EnsureEmployee(employee, request)),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()), Times.Once);

        fixture.UserRepository.Verify(
            x => x.Update(
                It.Is<UserModel>(user => EnsureUser(user, request)),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()), Times.Once);

        fixture.UnitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    private static bool EnsureEmployee(EmployeeModel employee, UpdateEmployeeRequest request)
    {
        employee.Id.Should().NotBeEmpty();
        employee.Active.Should().BeTrue();
        employee.Email.Should().Be(request.Email);
        employee.ImmediateSupervisor.Should().Be(request.ImmediateSupervisor);
        employee.Name.Should().Be(request.Name);
        employee.PositionId.Should().Be(request.PositionId);
        employee.Surname.Should().Be(request.Surname);

        for (int i = 0; i < request.Phones.Count(); i++)
        {
            employee.Phones.ElementAt(i).Id.Should().Be(request.Phones.ElementAt(i).Id);
            employee.Phones.ElementAt(i).Number.Should().Be(request.Phones.ElementAt(i).Number);
            employee.Phones.ElementAt(i).AreaCode.Should().Be(request.Phones.ElementAt(i).AreaCode);
            employee.Phones.ElementAt(i).IsPrimary.Should().Be(request.Phones.ElementAt(i).IsPrimary);
            employee.Phones.ElementAt(i).Active.Should().BeTrue();
        }

        return true;
    }

    private static bool EnsureUser(UserModel user, UpdateEmployeeRequest request)
    {
        user.Id.Should().NotBeEmpty();
        user.Username.Should().Be(request.Email);
        user.Active.Should().BeTrue();
        user.Password.Should().NotBeNullOrWhiteSpace();
        user.Role.Should().Be(request.Role);

        return true;
    }
}
