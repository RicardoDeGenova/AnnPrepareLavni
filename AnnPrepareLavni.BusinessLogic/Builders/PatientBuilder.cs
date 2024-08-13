using AnnPrepareLavni.Domain.Abstract.Domain.Entities;
using AnnPrepareLavni.Domain.Abstract.Enums;
using AnnPrepareLavni.Domain.Implementation.Entities;

namespace AnnPrepareLavni.BusinessLogic.Builders;

public static class PatientBuilder
{
    public static IPatient CreateWith(
        PatientId id, 
        string firstName, 
        string lastName, 
        DateTimeOffset dateOfBirth, 
        Gender gender, 
        float heightInMeters, 
        float heightInKilograms)
    {
        return new Patient
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Gender = gender,
                HeightInMeters = heightInMeters,
                WeightInKilograms = heightInKilograms,
            };
    }
}