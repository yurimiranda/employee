using Employee.UnitTests.UseCases.Employee.Fixtures;
using FluentAssertions;

namespace Employee.UnitTests.UseCases.Employee;

public class EmployeePagedRetrievalUseCaseTest(EmployeePagedRetrievalUseCaseFixture fixture) : IClassFixture<EmployeePagedRetrievalUseCaseFixture>
{
    [Fact]
    public async Task GetEmployees_WhenRequestIsInvalid_ShouldReturnError()
    {
        // Arrange
        fixture.Reset();
        var request = fixture.GenerateRequest();
        fixture.MockValidateRequest(request, isValid: false);

        // Act
        var result = await fixture.UseCase.GetEmployees(request);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.GetError().Code.Should().Be("GetEmployee.InvalidRequest");
    }

    [Fact]
    public async Task GetEmployees_WhenValid_ShouldReturnSuccess()
    {
        // Arrange
        fixture.Reset();
        var request = fixture.GenerateRequest();
        fixture.MockValidateRequest(request, isValid: true);
        fixture.MockGetPaged(request);

        // Act
        var result = await fixture.UseCase.GetEmployees(request);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Results.Should().HaveCount(request.PageSize);
    }
}