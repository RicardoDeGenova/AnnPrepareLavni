using FluentValidation.Results;

namespace AnnPrepareLavni.API.Common;

public record ValidationFailed(IEnumerable<ValidationFailure> Errors)
{
    public ValidationFailed(ValidationFailure error) : this([error])
    {
    }
}
