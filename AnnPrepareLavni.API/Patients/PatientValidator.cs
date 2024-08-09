using FluentValidation;

namespace AnnPrepareLavni.API.Patients;

public class PatientValidator : AbstractValidator<Patient>
{
    public PatientValidator()
    {
        RuleFor(x => x.Id)
               .NotEmpty();

        RuleFor(x => x.FirstName)
               .NotEmpty();
        
        RuleFor(x => x.LastName)
               .NotEmpty();

        RuleFor(x => x.DateOfBirth)
               .LessThanOrEqualTo(DateTimeOffset.UtcNow);

        RuleFor(x => x.Gender)
            .NotEmpty();

        RuleFor(x => x.HeightInMeters)
            .NotEmpty();
        
        RuleFor(x => x.WeightInKilograms)
            .NotEmpty();
    }
}
