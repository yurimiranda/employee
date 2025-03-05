using Employee.Application.Resources;
using Employee.Application.UseCases.User.Requests;
using FluentValidation;

namespace Employee.Application.UseCases.User.Validations;

public class UserSignInValidator : AbstractValidator<UserSignInRequest>
{
    public UserSignInValidator()
    {
        RuleFor(r => r.Username)
            .NotEmpty().WithMessage(Messages.NotEmpty);

        RuleFor(r => r.Password)
            .NotEmpty().WithMessage(Messages.NotEmpty);
    }
}