using FluentValidation.Results;

namespace AnnPrepareLavni.API.Validation;

public record ValidationFailed(IEnumerable<ValidationFailure> Errors)
{
    public ValidationFailed(ValidationFailure error) : this([error])
    {
    }
}
