using Riok.Mapperly.Abstractions;

namespace AnnPrepareLavni.ApiService.Features.Prescription.Contracts;

[Mapper]
public static partial class PrescriptionMapper
{
    public static partial PrescriptionResponse ToResponse(Models.Prescription prescription);
    public static partial Models.Prescription ToPrescription(PrescriptionRequest prescriptionRequest);
    public static partial void MapToExistingPrescription(PrescriptionRequest prescriptionRequest, Models.Prescription prescription);
}
