using Employee.Application.Abstractions.Validations;

namespace FluentValidation;

internal static class PasswordValidatorExtensions
{
    internal static IRuleBuilderOptions<T, string> EnsureValidPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
        => ruleBuilder.SetValidator(new PasswordValidator<T>());
}