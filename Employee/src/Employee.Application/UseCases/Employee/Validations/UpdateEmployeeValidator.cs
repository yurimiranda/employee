using Employee.Application.Resources;
using Employee.Application.UseCases.Employee.Requests;
using Employee.Domain.Enums;
using Employee.Domain.Repositories;
using Employee.Domain.Services;
using FluentValidation;

namespace Employee.Application.UseCases.Employee.Validations;

public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeRequest>
{
    public UpdateEmployeeValidator(
        IPositionRepository roleRepository,
        IPhoneRepository phoneRepository,
        IUserContextAccessor userContext)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(r => r.Name)
            .NotEmpty().WithMessage(Messages.NotEmpty);

        RuleFor(r => r.Surname)
            .NotEmpty().WithMessage(Messages.NotEmpty);

        RuleFor(r => r.Email)
            .NotEmpty().WithMessage(Messages.NotEmpty)
            .EmailAddress();

        RuleFor(r => r.ImmediateSupervisor)
            .NotEmpty().WithMessage(Messages.NotEmpty);

        RuleFor(r => r.PositionId)
            .NotEmpty().WithMessage(Messages.NotEmpty)
            .MustAsync(roleRepository.Exists).WithMessage(Messages.NotExists);

        RuleFor(r => r.Role)
            .IsInEnum()
            .Must((req, role) => HasLowerPermissions(role, userContext)).WithMessage(Messages.EmployeeInvalidRole);

        RuleFor(r => r.Phones).Cascade(CascadeMode.Stop)
            .Must(p => p.Any(x => x.IsPrimary)).WithMessage(Messages.PrimaryPhoneRequired)
            .Must(p => p.Count(x => x.IsPrimary) == 1).WithMessage(Messages.MoreThanOnePrimaryPhone);

        RuleForEach(r => r.Phones).Cascade(CascadeMode.Stop)
            .SetValidator(new UpdatePhoneValidator(phoneRepository));
    }

    private static bool HasLowerPermissions(Role employeeRole, IUserContextAccessor userContext)
    {
        var userRole = userContext.UserContext.Role;

        if (employeeRole < userRole)
            return false;

        return true;
    }

    private sealed class UpdatePhoneValidator : PhoneValidator<UpdateEmployeeRequest.UpdatePhoneRequest>
    {
        public UpdatePhoneValidator(IPhoneRepository repository)
        {
            RuleFor(r => r.Id)
                .NotEmpty().WithMessage(Messages.NotEmpty)
                .MustAsync(repository.Exists).WithMessage(Messages.NotExists);
        }
    }
}