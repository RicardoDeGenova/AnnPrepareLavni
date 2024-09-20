using Riok.Mapperly.Abstractions;

namespace AnnPrepareLavni.ApiService.Features.Medication.Contracts;

[Mapper]
public static partial class MedicationMapper
{
    public static partial MedicationResponse ToResponse(Models.Medication medication);
    public static partial Models.Medication ToMedication(MedicationRequest medicationRequest);
    public static partial void MapToExistingMedication(MedicationRequest medicationRequest, Models.Medication medication);
}