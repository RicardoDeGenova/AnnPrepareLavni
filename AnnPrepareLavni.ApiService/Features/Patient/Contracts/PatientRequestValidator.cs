using AnnPrepareLavni.ApiService.Features.Address.Contracts;
using AnnPrepareLavni.ApiService.Features.MedicalCondition.Contracts;
using AnnPrepareLavni.ApiService.Utils.Extensions;
using FluentValidation;

namespace AnnPrepareLavni.ApiService.Features.Patient.Contracts;

public class PatientRequestValidator : AbstractValidator<PatientRequest>
{
    public PatientRequestValidator()
    {
        RuleFor(p => p.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .Length(3, 50).WithMessage("First name must be between 3 and 50 characters.");

        RuleFor(p => p.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .Length(3, 100).WithMessage("Last name must be between 3 and 100 characters.");

        RuleFor(p => p.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .Must(BeAValidAge).WithMessage("Age must be between 0 and 120.");

        RuleFor(p => p.Gender)
            .NotNull().WithMessage("Gender is required.")
            .IsInEnum().WithMessage("Invalid gender.");

        // Optional fields with length validation
        RuleFor(p => p.HeightInMeters)
            .InclusiveBetween(0.3f, 2.4f).WithMessage("Height should be between 0.3 and 2.4 meters.")
            .When(p => p.HeightInMeters is not null);

        RuleFor(p => p.WeightInKilograms)
            .InclusiveBetween(1.5f, 500f).WithMessage("Weight should be between 1.5 and 500 kilograms.")
            .When(p => p.WeightInKilograms is not null);

        RuleFor(p => p.FamilySize)
            .InclusiveBetween(1, 100).WithMessage("Family size must be between 1 and 100.")
            .When(p => p.FamilySize is not null);

        RuleFor(p => p.HighestEducation)
            .IsInEnum().WithMessage("Highest education must be a valid enum value.")
            .When(p => p.HighestEducation is not null);

        RuleFor(p => p.SurgicalProceduresNotes)
            .MaximumLength(1000).WithMessage("Surgical procedures notes cannot be longer than 1000 characters.")
            .When(p => !string.IsNullOrEmpty(p.SurgicalProceduresNotes));

        RuleFor(p => p.FamilyHistoryNotes)
            .MaximumLength(1000).WithMessage("Family history notes cannot be longer than 1000 characters.")
            .When(p => !string.IsNullOrEmpty(p.FamilyHistoryNotes));

        RuleFor(p => p.AllergiesNotes)
            .MaximumLength(1000).WithMessage("Allergies notes cannot be longer than 1000 characters.")
            .When(p => !string.IsNullOrEmpty(p.AllergiesNotes));

        RuleFor(p => p.ProfileNotes)
            .MaximumLength(1000).WithMessage("Profile notes cannot be longer than 1000 characters.")
            .When(p => !string.IsNullOrEmpty(p.ProfileNotes));

        RuleFor(p => p.Address)
            .SetValidator(new AddressRequestValidator()!)
            .When(p => p.Address != null);

        RuleForEach(p => p.MedicalConditions)
            .SetValidator(new MedicalConditionRequestValidator())
            .When(p => p.MedicalConditions != null && p.MedicalConditions.Count != 0);
    }

    private bool BeAValidAge(DateTime dateOfBirth)
    {
        var age = dateOfBirth.GetAge();
        return age >= 0 && age <= 120;
    }
}
