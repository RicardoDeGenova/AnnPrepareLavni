using FluentValidation;

namespace AnnPrepareLavni.ApiService.Features.Medication.Contracts;

public class MedicationRequestValidator : AbstractValidator<MedicationRequest>
{
    public MedicationRequestValidator()
    {
        RuleFor(m => m.Name)
            .NotEmpty().WithMessage("Medication name is required.")
            .MaximumLength(150).WithMessage("Medication name cannot be longer than 150 characters.");

        RuleFor(mc => mc.DosageForm)
            .IsInEnum().WithMessage("Medication type is required.");

        RuleFor(mc => mc.StrengthUnit)
            .IsInEnum().WithMessage("Medication type is required.");
    }
}
