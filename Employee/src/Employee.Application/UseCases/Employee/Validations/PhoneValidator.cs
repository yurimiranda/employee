using Employee.Application.Resources;
using Employee.Application.UseCases.Employee.Base;
using FluentValidation;

namespace Employee.Application.UseCases.Employee.Validations;

public class PhoneValidator<TPhoneRequest> : AbstractValidator<TPhoneRequest> where TPhoneRequest : Phone
{
    public PhoneValidator()
    {
        RuleFor(r => r.AreaCode)
            .NotEmpty().WithMessage(Messages.NotEmpty)
            .Length(2).WithMessage(string.Format(Messages.Length, "2"));

        RuleFor(r => r.Number)
            .NotEmpty().WithMessage(Messages.NotEmpty)
            .Length(8, 9).WithMessage(string.Format(Messages.LengthRange, "8", "9"));
    }
}