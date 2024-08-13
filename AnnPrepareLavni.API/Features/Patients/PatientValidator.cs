﻿using AnnPrepareLavni.Domain.Abstract.Domain.Entities;
using FluentValidation;

namespace AnnPrepareLavni.API.Endpoints.Patients;

public class PatientValidator : AbstractValidator<IPatient>
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
