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
        if (string.IsNullOrWhiteSpace(value))
        {
            context.AddFailure(Messages.NotEmpty);
            return false;
        }

        if (value.Length < 8)
        {
            context.AddFailure(string.Format(Messages.NotLessThan, "8"));
            return false;
        }

        if (value.Length > 20)
        {
            context.AddFailure(string.Format(Messages.NotGreaterThan, "20"));
            return false;
        }

        if (!value.Any(char.IsUpper))
        {
            context.AddFailure(Messages.UpperCaseRequired);
            return false;
        }

        if (!value.Any(char.IsLower))
        {
            context.AddFailure(Messages.LowerCaseRequired);
            return false;
        }

        if (!value.Any(char.IsNumber))
        {
            context.AddFailure(Messages.NumbersRequired);
            return false;
        }

        if (!value.Any(char.IsSymbol))
        {
            context.AddFailure(Messages.SymbolsRequired);
            return false;
        }

        return true;
    }

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return null;
    }
}

internal interface IPasswordValidator : IPropertyValidator
{
}