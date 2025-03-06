using Employee.Application.Resources;
using Employee.Domain.Models;
using Employee.Domain.Repositories;
using Employee.UnitTests.Extensions;
using Employee.UnitTests.UseCases.Employee.Fixtures;
using FluentAssertions;
using Moq;

namespace Employee.UnitTests.UseCases.Employee;

public class EmployeeDeletionUseCaseTest(EmployeeDeletionUseCaseFixture fixture) : IClassFixture<EmployeeDeletionUseCaseFixture>
{
    [Fact]
    public async Task DeleteEmployee_WhenEmployeeNotFound_ShouldReturnError()
    {
        // Arrange
        fixture.Reset();
        var id = Guid.NewGuid();
        fixture.MockGetEmployee(id, found: false);

        // Act
        var result = await fixture.UseCase.DeleteEmployee(id);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.GetError().Code.Should().Be("Employee.NotFound");
        result.GetError().ErrorMessages.First().Should().Be(string.Format(Messages.NotFound, "Funcionário"));

        fixture.EmployeeRepository.EnsuresNonPersistence<IEmployeeRepository, EmployeeModel, Guid>();
        fixture.UserRepository.EnsuresNonPersistence<IUserRepository, UserModel, Guid>();
        fixture.PhoneRepository.EnsuresNonPersistence<IPhoneRepository, PhoneModel, int>();
    }

    [Fact]
    public async Task DeleteEmployee_WhenValid_ShouldReturnSuccess()
    {
        // Arrange
        fixture.Reset();
        var id = Guid.NewGuid();
        fixture.MockGetEmployee(id, found: true);
        fixture.MockGetUser(id, found: true);
        fixture.MockGetPhones(id, found: true);

        // Act
        var result = await fixture.UseCase.DeleteEmployee(id);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().NotBeNull();
        result.GetValue().Id.Should().Be(id);

        fixture.EmployeeRepository.EnsuresNonPersistence<IEmployeeRepository, EmployeeModel, Guid>(delete: false);
        fixture.UserRepository.EnsuresNonPersistence<IUserRepository, UserModel, Guid>(delete: false);
        fixture.PhoneRepository.EnsuresNonPersistence<IPhoneRepository, PhoneModel, int>(deleteAll: false);

        fixture.EmployeeRepository.Verify(x => x.Delete(It.IsAny<EmployeeModel>(), It.IsAny<CancellationToken>()), Times.Once);
        fixture.UserRepository.Verify(x => x.Delete(It.IsAny<UserModel>(), It.IsAny<CancellationToken>()), Times.Once);
        fixture.PhoneRepository.Verify(x => x.DeleteAll(It.IsAny<IEnumerable<PhoneModel>>(), It.IsAny<CancellationToken>()), Times.Once);
        fixture.UnitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteEmployee_WhenUserNotFound_ShouldReturnSuccess()
    {
        // Arrange
        fixture.Reset();
        var id = Guid.NewGuid();
        fixture.MockGetEmployee(id, found: true);
        fixture.MockGetUser(id, found: false);
        fixture.MockGetPhones(id, found: true);
        // Act
        var result = await fixture.UseCase.DeleteEmployee(id);
        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().NotBeNull();
        result.GetValue().Id.Should().Be(id);

        fixture.UserRepository.EnsuresNonPersistence<IUserRepository, UserModel, Guid>();
        fixture.EmployeeRepository.EnsuresNonPersistence<IEmployeeRepository, EmployeeModel, Guid>(delete: false);
        fixture.PhoneRepository.EnsuresNonPersistence<IPhoneRepository, PhoneModel, int>(deleteAll: false);

        fixture.EmployeeRepository.Verify(x => x.Delete(It.IsAny<EmployeeModel>(), It.IsAny<CancellationToken>()), Times.Once);
        fixture.PhoneRepository.Verify(x => x.DeleteAll(It.IsAny<IEnumerable<PhoneModel>>(), It.IsAny<CancellationToken>()), Times.Once);
        fixture.UnitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteEmployee_WhenPhonesNotFound_ShouldReturnSuccess()
    {
        // Arrange
        fixture.Reset();
        var id = Guid.NewGuid();
        fixture.MockGetEmployee(id, found: true);
        fixture.MockGetUser(id, found: true);
        fixture.MockGetPhones(id, found: false);
        // Act
        var result = await fixture.UseCase.DeleteEmployee(id);
        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().NotBeNull();
        result.GetValue().Id.Should().Be(id);

        fixture.UserRepository.EnsuresNonPersistence<IUserRepository, UserModel, Guid>(delete: false);
        fixture.EmployeeRepository.EnsuresNonPersistence<IEmployeeRepository, EmployeeModel, Guid>(delete: false);
        fixture.PhoneRepository.EnsuresNonPersistence<IPhoneRepository, PhoneModel, int>();

        fixture.EmployeeRepository.Verify(x => x.Delete(It.IsAny<EmployeeModel>(), It.IsAny<CancellationToken>()), Times.Once);
        fixture.UserRepository.Verify(x => x.Delete(It.IsAny<UserModel>(), It.IsAny<CancellationToken>()), Times.Once);
        fixture.UnitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }
}