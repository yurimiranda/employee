using Employee.Application.Resources;
using Employee.UnitTests.UseCases.User.Fixtures;
using FluentAssertions;

namespace Employee.UnitTests.UseCases.User;

public class UserSignInUseCaseTest(UserSignInUseCaseFixture fixture) : IClassFixture<UserSignInUseCaseFixture>
{
    [Fact]
    public async Task SignIn_WhenRequestIsInvalid_ShouldReturnError()
    {
        // Arrange
        fixture.Reset();
        var request = fixture.GenerateRequest();
        fixture.MockValidateRequest(request, isValid: false);

        // Act
        var result = await fixture.UseCase.SignIn(request);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.GetError().Code.Should().Be("SignIn.Validation.InvalidRequest");
    }

    [Fact]
    public async Task SignIn_WhenUserNotFound_ShouldReturnError()
    {
        // Arrange
        fixture.Reset();
        var request = fixture.GenerateRequest();
        fixture.MockValidateRequest(request, isValid: true);
        fixture.MockGetUser(request.Username, request.Password, found: false);

        // Act
        var result = await fixture.UseCase.SignIn(request);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.GetError().Code.Should().Be("SignIn.InvalidCredentials");
        result.GetError().ErrorMessages.First().Should().Be(Messages.InvalidCredentials);
    }

    [Fact]
    public async Task SignIn_WhenPasswordIsInvalid_ShouldReturnError()
    {
        // Arrange
        fixture.Reset();
        var request = fixture.GenerateRequest();
        fixture.MockValidateRequest(request, isValid: true);
        fixture.MockGetUser(request.Username, password: "123!321@AaBb", found: true);

        // Act
        var result = await fixture.UseCase.SignIn(request);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.GetError().Code.Should().Be("SignIn.InvalidCredentials");
        result.GetError().ErrorMessages.First().Should().Be(Messages.InvalidCredentials);
    }

    [Fact]
    public async Task SignIn_WhenValid_ShouldReturnSuccess()
    {
        // Arrange
        fixture.Reset();
        var request = fixture.GenerateRequest();
        fixture.MockValidateRequest(request, isValid: true);
        var user = fixture.MockGetUser(request.Username, request.Password, found: true);
        var token = fixture.MockGenerateToken(user);

        // Act
        var result = await fixture.UseCase.SignIn(request);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().NotBeNull();
        result.GetValue().Name.Should().Be(user!.Employee.Name);
        result.GetValue().Token.Should().Be(token);
    }
}