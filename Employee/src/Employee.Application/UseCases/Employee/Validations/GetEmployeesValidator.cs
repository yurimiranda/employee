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
            .NotEmpty().WithMessage(Messages.NotEmpty)
            .EmailAddress();

        RuleFor(r => r.Document)
            .NotEmpty().WithMessage(Messages.NotEmpty)
            .Length(11).WithMessage(string.Format(Messages.Length, "11"))
            .Must(CpfValidation.Validate).WithMessage(Messages.InvalidValue);

        RuleFor(r => r.Page)
            .NotEmpty().WithMessage(Messages.NotEmpty);

        RuleFor(r => r.PageSize)
            .NotEmpty().WithMessage(Messages.NotEmpty)
            .LessThan(500);
    }
}