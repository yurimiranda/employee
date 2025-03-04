using Employee.Application.Resources;
using Employee.Application.UseCases.Employee.Requests;
using Employee.Domain.Repositories;
using FluentValidation;

namespace Employee.Application.UseCases.Employee.Validations;

public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeRequest>
{
    public UpdateEmployeeValidator(
        IPositionRoleRepository roleRepository,
        IPhoneRepository phoneRepository)
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

        RuleFor(r => r.PositionRoleId)
            .NotEmpty().WithMessage(Messages.NotEmpty)
            .MustAsync(roleRepository.Exists).WithMessage(Messages.NotExists);

        RuleFor(r => r.Phones).Cascade(CascadeMode.Stop)
            .Must(p => p.Any(x => x.IsPrimary)).WithMessage(Messages.PrimaryPhoneRequired)
            .Must(p => p.Count(x => x.IsPrimary) == 1).WithMessage(Messages.MoreThanOnePrimaryPhone);

        RuleForEach(r => r.Phones).Cascade(CascadeMode.Stop)
            .SetValidator(new UpdatePhoneValidator(phoneRepository));
    }

    public class UpdatePhoneValidator : PhoneValidator<UpdateEmployeeRequest.UpdatePhoneRequest>
    {
        public UpdatePhoneValidator(IPhoneRepository repository)
        {
            RuleFor(r => r.Id)
                .NotEmpty().WithMessage(Messages.NotEmpty)
                .MustAsync(repository.Exists).WithMessage(Messages.NotExists);
        }
    }
}