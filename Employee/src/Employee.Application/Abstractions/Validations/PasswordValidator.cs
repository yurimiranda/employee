using Employee.Application.Resources;
using FluentValidation;
using FluentValidation.Validators;
using System.Text;

namespace Employee.Application.Abstractions.Validations;

internal class PasswordValidator<T> : PropertyValidator<T, string>, IPasswordValidator
{
    public override string Name => "PasswordValidator";

    public override bool IsValid(ValidationContext<T> context, string value)
    {
        var isValid = true;
        if (string.IsNullOrWhiteSpace(value))
        {
            context.AddFailure(Messages.NotEmpty);
            isValid = false;
        }

        if (value.Length < 8)
        {
            context.AddFailure(string.Format(Messages.NotLessThan, "8"));
            isValid = false;
        }

        if (value.Length > 20)
        {
            context.AddFailure(string.Format(Messages.NotGreaterThan, "20"));
            isValid = false;
        }

        if (!value.Any(char.IsUpper))
        {
            context.AddFailure(Messages.UpperCaseRequired);
            isValid = false;
        }

        if (!value.Any(char.IsLower))
        {
            context.AddFailure(Messages.LowerCaseRequired);
            isValid = false;
        }

        if (!value.Any(char.IsNumber))
        {
            context.AddFailure(Messages.NumbersRequired);
            isValid = false;
        }

        if (!value.Any(char.IsSymbol) && !value.Any(char.IsPunctuation))
        {
            context.AddFailure(Messages.SymbolsRequired);
            isValid = false;
        }

        return isValid;
    }

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return string.Empty;
    }
}

internal interface IPasswordValidator : IPropertyValidator
{
}