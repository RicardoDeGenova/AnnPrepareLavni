using FluentValidation;

namespace AnnPrepareLavni.ApiService.Features.Prescription.Contracts;

public class PrescriptionRequestValidator : AbstractValidator<PrescriptionRequest>
{
    public PrescriptionRequestValidator()
    {
        RuleFor(p => p.PatientId)
            .NotEmpty().WithMessage("PatientId is required.");

        RuleFor(p => p.PrescriberId)
            .NotEmpty().WithMessage("PrescriberId is required.");

        RuleFor(p => p.Quantity)
            .NotEmpty().WithMessage("Prescription quantity is required.");

        RuleFor(p => p.Notes)
            .MaximumLength(500).WithMessage("Prescription notes cannot be longer than 500 characters.")
            .When(p => !string.IsNullOrEmpty(p.Notes));
    }
}
