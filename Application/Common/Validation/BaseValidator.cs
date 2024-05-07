using Application.Common.Exceptions;
using FluentValidation;
using FluentValidation.Results;

namespace Application.Common.Validation;
public class BaseValidator<T> : AbstractValidator<T>
{
    public BaseValidator() : base()
    {

    }

    protected override void RaiseValidationException(ValidationContext<T> context, ValidationResult result)
    {
        var ex = new ValidationException(result.Errors);
        throw new BusinessValidationException(ex.Message, ex);
    }
}
