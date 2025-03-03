using Employee.Application.Resources;
using Employee.Application.UseCases.Employee.Requests;
using Employee.Domain.Repositories;
using FluentValidation;

namespace Employee.Application.UseCases.Employee.Validations;

public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeRequest>
{
    public CreateEmployeeValidator(IPositionRoleRepository roleRepository)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(r => r.Name)
            .NotEmpty().WithMessage(Messages.NotEmpty);

        RuleFor(r => r.Surname)
            .NotEmpty().WithMessage(Messages.NotEmpty);

        RuleFor(r => r.Email)
            .NotEmpty().WithMessage(Messages.NotEmpty);

        RuleFor(r => r.Document)
            .NotEmpty().WithMessage(Messages.NotEmpty);

        RuleFor(r => r.ImmediateSupervisor)
            .NotEmpty().WithMessage(Messages.NotEmpty);

        RuleFor(r => r.PositionRoleId)
            .NotEmpty().WithMessage(Messages.NotEmpty)
            .MustAsync(roleRepository.Exists).WithMessage(Messages.NotExists);

        RuleFor(r => r.BirthDate).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Messages.NotEmpty)
            .NotEqual(DateTime.MinValue).WithMessage(Messages.NotEmpty);

        RuleFor(r => r.Password)
            .EnsureValidPassword();

        RuleForEach(r => r.Phones).Cascade(CascadeMode.Stop)
            .SetValidator(new CreatePhoneValidator());

        RuleFor(r => r.Phones).Cascade(CascadeMode.Stop)
            .Must(p => p.Any(x => x.IsPrimary)).WithMessage(Messages.PrimaryPhoneRequired)
            .Must(p => p.Count(x => x.IsPrimary) == 1).WithMessage(Messages.MoreThanOnePrimaryPhone);
    }
}

public class CreatePhoneValidator : AbstractValidator<CreateEmployeeRequest.CreatePhoneRequest>
{
    public CreatePhoneValidator()
    {
        RuleFor(r => r.AreaCode)
            .NotEmpty().WithMessage(Messages.NotEmpty)
            .Length(2).WithMessage(string.Format(Messages.Length, "2"));

        RuleFor(r => r.Number)
            .NotEmpty().WithMessage(Messages.NotEmpty)
            .Length(8, 9).WithMessage(string.Format(Messages.LengthRange, "8", "9"));
    }
}