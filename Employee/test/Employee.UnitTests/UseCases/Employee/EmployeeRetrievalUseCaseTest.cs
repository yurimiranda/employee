using Employee.Application.Resources;
using Employee.UnitTests.UseCases.Employee.Fixtures;
using FluentAssertions;

namespace Employee.UnitTests.UseCases.Employee;

public class EmployeeRetrievalUseCaseTest(EmployeeRetrievalUseCaseFixture fixture) : IClassFixture<EmployeeRetrievalUseCaseFixture>
{
    [Fact]
    public async Task GetEmployee_WhenEmployeeNotFound_ShouldReturnError()
    {
        // Arrange
        fixture.Reset();
        var id = Guid.NewGuid();
        fixture.MockGetEmployee(id, found: false);

        // Act
        var result = await fixture.UseCase.GetEmployee(id);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.GetError().Code.Should().Be("Employee.NotFound");
        result.GetError().ErrorMessages.First().Should().Be(string.Format(Messages.NotFound, "Funcionário"));
    }

    [Fact]
    public async Task GetEmployee_WhenValid_ShouldReturnSuccess()
    {
        // Arrange
        fixture.Reset();
        var id = Guid.NewGuid();
        fixture.MockGetEmployee(id, found: true);

        // Act
        var result = await fixture.UseCase.GetEmployee(id);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().NotBeNull();
        result.GetValue().Id.Should().Be(id);
    }
}