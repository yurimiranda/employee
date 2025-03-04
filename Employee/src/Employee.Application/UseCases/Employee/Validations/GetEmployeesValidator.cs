using DocumentValidator;
using Employee.Application.Resources;
using Employee.Application.UseCases.Employee.Requests;
using FluentValidation;

namespace Employee.Application.UseCases.Employee.Validations;

public class GetEmployeesValidator : AbstractValidator<GetEmployeesRequest>
{
    public GetEmployeesValidator()
    {
        RuleFor(r => r.Email)
            .EmailAddress()
                .When(r => !string.IsNullOrEmpty(r.Email), ApplyConditionTo.CurrentValidator);

        RuleFor(r => r.Document)
            .Length(11).WithMessage(string.Format(Messages.Length, "11"))
            .Must(CpfValidation.Validate).WithMessage(Messages.InvalidValue)
                .When(r => !string.IsNullOrEmpty(r.Document));

        RuleFor(r => r.Page)
            .NotEmpty().WithMessage(Messages.NotEmpty);

        RuleFor(r => r.PageSize)
            .NotEmpty().WithMessage(Messages.NotEmpty)
            .LessThan(500);
    }
}