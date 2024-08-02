namespace AnnPrepareLavni.API.Patients;

public class MedicalCondition
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateOfReport { get; set; }
}
