using Employee.Domain.ResultPattern;
using FluentValidation;

namespace Employee.Application.Abstractions;

public abstract class UseCaseBase
{
    protected static async Task<Result<TRequest, Error>> ValidateRequest<TRequest>(IValidator<TRequest> validator, TRequest request)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var groupedErrors = validationResult.Errors.GroupBy(e => e.PropertyName);
            if (groupedErrors.Any())
            {
                var error = groupedErrors.First();
                return Error.Throw(error.Key, error.Where(e => !string.IsNullOrWhiteSpace(e.ErrorMessage)).Select(e => e.ErrorMessage));
            }

            return Error.Throw("ValidationError", validationResult.Errors.Select(e => e.ErrorMessage));
        }

        return request;
    }
}