using FluentValidation.Results;

namespace AnnPrepareLavni.API.Util;

public record ValidationFailed(IEnumerable<ValidationFailure> Errors)
{
    public ValidationFailed(ValidationFailure error) : this([error])
    {
    }
}
