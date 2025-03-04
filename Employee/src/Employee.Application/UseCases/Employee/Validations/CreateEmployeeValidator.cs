using DocumentValidator;
using Employee.Application.Resources;
using Employee.Application.UseCases.Employee.Requests;
using Employee.Domain.Enums;
using Employee.Domain.Repositories;
using Employee.Domain.Services;
using FluentValidation;

namespace Employee.Application.UseCases.Employee.Validations;

public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeRequest>
{
    public CreateEmployeeValidator(IPositionRepository roleRepository, IUserContextAccessor userContext)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(r => r.Name)
            .NotEmpty().WithMessage(Messages.NotEmpty);

        RuleFor(r => r.Surname)
            .NotEmpty().WithMessage(Messages.NotEmpty);

        RuleFor(r => r.Email)
            .NotEmpty().WithMessage(Messages.NotEmpty)
            .EmailAddress();

        RuleFor(r => r.Document)
            .NotEmpty().WithMessage(Messages.NotEmpty)
            .Length(11).WithMessage(string.Format(Messages.Length, "11"))
            .Must(CpfValidation.Validate).WithMessage(Messages.InvalidValue)
                .When(r => !string.IsNullOrEmpty(r.Document), ApplyConditionTo.CurrentValidator);

        RuleFor(r => r.ImmediateSupervisor)
            .NotEmpty().WithMessage(Messages.NotEmpty);

        RuleFor(r => r.PositionId)
            .NotEmpty().WithMessage(Messages.NotEmpty)
            .MustAsync(roleRepository.Exists).WithMessage(Messages.NotExists);

        RuleFor(r => r.BirthDate).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Messages.NotEmpty)
            .NotEqual(DateTime.MinValue).WithMessage(Messages.NotEmpty);

        RuleFor(r => r.Password)
            .EnsureValidPassword();

        RuleFor(r => r.Role)
            .IsInEnum()
            .Must((req, role) => HasLowerPermissions(role, userContext)).WithMessage(Messages.EmployeeInvalidRole);

        RuleFor(r => r.Phones).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Messages.NotEmpty)
            .Must(p => p.Any(x => x.IsPrimary)).WithMessage(Messages.PrimaryPhoneRequired)
            .Must(p => p.Count(x => x.IsPrimary) == 1).WithMessage(Messages.MoreThanOnePrimaryPhone);

        RuleForEach(r => r.Phones).Cascade(CascadeMode.Stop)
            .SetValidator(new CreatePhoneValidator());
    }

    private static bool HasLowerPermissions(Role employeeRole, IUserContextAccessor userContext)
    {
        var userRole = userContext.UserContext?.Role ?? Role.Admin;

        if (employeeRole < userRole)
            return false;

        return true;
    }

    private sealed class CreatePhoneValidator : PhoneValidator<CreateEmployeeRequest.CreatePhoneRequest>
    {
    }
}