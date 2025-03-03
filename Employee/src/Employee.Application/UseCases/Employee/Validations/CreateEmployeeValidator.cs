using Employee.Application.Resources;
using Employee.Application.UseCases.Employee.Requests;
using FluentValidation;

namespace Employee.Application.UseCases.Employee.Validations;

public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeRequest>
{
    public CreateEmployeeValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .WithMessage(string.Format(Messages.NotEmpty, "Nome"));

        RuleFor(r => r.Surname)
            .NotEmpty()
            .WithMessage(string.Format(Messages.NotEmpty, "Sobrenome"));

        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage(string.Format(Messages.NotEmpty, "Email"));

        RuleFor(r => r.Document)
            .NotEmpty()
            .WithMessage(string.Format(Messages.NotEmpty, "Documento"));
    }
}