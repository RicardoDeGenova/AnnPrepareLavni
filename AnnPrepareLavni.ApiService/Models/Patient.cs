using AnnPrepareLavni.ApiService.Models.Enums;
using AnnPrepareLavni.ApiService.Utils.Extensions;
using System.ComponentModel.DataAnnotations;

namespace AnnPrepareLavni.ApiService.Models;

public class Patient
{
    [Required]
    public Guid Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "PatientNo must be a positive number.")]
    public int PatientNo { get; set; }
    
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(100, ErrorMessage = "Last name cannot be longer than 50 characters.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of birth is required.")]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(Patient), nameof(ValidateDateOfBirth))]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Gender is required.")]
    public Gender Gender { get; set; }

    [Range(0.3, 2.4, ErrorMessage = "Height should be between 0.3 and 2.4 meters.")]
    public float HeightInMeters { get; set; }

    [Range(1.5, 500, ErrorMessage = "Weight should be between 1.5 and 500 kilograms.")]
    public float WeightInKilograms { get; set; }

    [Range(1, 100, ErrorMessage = "Family size must be between 1 and 100.")]
    public int FamilySize { get; set; }

    [Required(ErrorMessage = "Highest education is required.")]
    public HighestEducation HighestEducation { get; set; }

    [StringLength(1000, ErrorMessage = "Surgical procedures notes cannot be longer than 1000 characters.")]
    public string? SurgicalProceduresNotes { get; set; }

    [StringLength(1000, ErrorMessage = "Family history notes cannot be longer than 1000 characters.")]
    public string? FamilyHistoryNotes { get; set; }

    [StringLength(1000, ErrorMessage = "Allergies notes cannot be longer than 1000 characters.")]
    public string? AllergiesNotes { get; set; }

    [StringLength(1000, ErrorMessage = "Profile notes cannot be longer than 1000 characters.")]
    public string? ProfileNotes { get; set; }

    public Address? Address { get; set; }
    public ICollection<MedicalCondition>? MedicalConditions { get; set; }

    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public string FullName => $"{FirstName} {LastName}";
    public int Age => DateOfBirth.GetAge();

    public static ValidationResult ValidateDateOfBirth(DateTime dateOfBirth, ValidationContext context)
    {
        var age = dateOfBirth.GetAge();

        return (age < 0 || age > 120)
            ? new ValidationResult("Invalid date of birth. Age must be between 0 and 120.")
            : ValidationResult.Success!;
    }
}
