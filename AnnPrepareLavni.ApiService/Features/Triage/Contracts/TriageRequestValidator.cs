using FluentValidation;

namespace AnnPrepareLavni.ApiService.Features.Triage.Contracts;

public class TriageRequestValidator : AbstractValidator<TriageRequest>
{
    public TriageRequestValidator()
    {
        RuleFor(t => t.PatientId)
            .NotEmpty().WithMessage("PatientId is required.");

        RuleFor(t => t.NurseId)
            .NotEmpty().WithMessage("NurseId is required.");

        RuleFor(t => t.Complaint)
            .NotEmpty().WithMessage("Complaint cannot be empty.")
            .MaximumLength(1000).WithMessage("Complaint cannot be longer than 1000 characters.");

        RuleFor(t => t.Notes)
            .MaximumLength(1000).WithMessage("Triage notes cannot be longer than 1000 characters.")
            .When(p => !string.IsNullOrEmpty(p.Notes));

        RuleFor(t => t.TemperatureInCelsius)
            .InclusiveBetween(15.0, 60.0).WithMessage("Temperature in celsius must be between 15°C and 60°C")
            .When(t => t.TemperatureInCelsius != 0d);

        RuleFor(t => t.OxygenSaturationLevel)
            .InclusiveBetween(80.0, 120.0).WithMessage("Oxygen saturationl evel must be between 80% and 120%")
            .When(t => t.TemperatureInCelsius != 0d);
    }
}
