namespace AnnPrepareLavni.API;

public enum DosageType
{
    g,
    Mg,
    Ml,
    Drops,
}

public class Medication
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Dosage { get; set; }
    public DosageType DosageType { get; set; }
}
