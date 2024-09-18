using AnnPrepareLavni.ApiService.Models.Enums;

namespace AnnPrepareLavni.ApiService.Models;

public class Medication
{
    public Guid Id { get; set; }
    public Guid? PatientId { get; set; }
    public Patient? Patient { get; set; }
    public Guid? MedicationNameId { get; set; }
    public MedicationName? MedicationName { get; set; }
    public MedicationType MedicationType { get; set; }
    public MedicationStrengthType StrengthType { get; set; }
    public double Strength { get; set; }
    public MedicationFrequencyType FrequencyType { get; set; }
    public int FrequencyWhenAtRegularIntervals { get; set; }
    public IEnumerable<DayOfWeek> FrequencyWhenOnSpecificDays { get; set; }
    public IEnumerable<TimeOnly> TimesToTakeMedicine { get; set; }
    public User Prescriber { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}