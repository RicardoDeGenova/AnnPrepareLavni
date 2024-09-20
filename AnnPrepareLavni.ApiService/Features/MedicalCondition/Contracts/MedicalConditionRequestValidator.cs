using FluentValidation;

namespace AnnPrepareLavni.ApiService.Features.MedicalCondition.Contracts;

public class MedicalConditionRequestValidator : AbstractValidator<MedicalConditionRequest>
{
    public MedicalConditionRequestValidator()
    {
        RuleFor(mc => mc.Name)
            .NotEmpty().WithMessage("Condition name is required.")
            .MaximumLength(150).WithMessage("Condition name cannot be longer than 150 characters.");

        RuleFor(mc => mc.Description)
            .MaximumLength(500).WithMessage("Condition name cannot be longer than 500 characters.")
            .When(mc => mc.Description is not null);

        RuleFor(mc => mc.ConditionType)
            .IsInEnum().WithMessage("Condition type is required.");

        RuleFor(mc => mc.StartedAt)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(mc => mc.StoppedAt)
            .GreaterThanOrEqualTo(mc => mc.StartedAt)
            .WithMessage("StoppedAt must be after StartedAt.")
            .When(mc => mc.StoppedAt is not null);
    }
}
