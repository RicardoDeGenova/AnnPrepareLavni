using Riok.Mapperly.Abstractions;

namespace AnnPrepareLavni.ApiService.Features.MedicalCondition.Contracts;

[Mapper]
public static partial class MedicalConditionMapper
{
    public static partial MedicalConditionResponse ToResponse(Models.MedicalCondition medicalCondition);
    public static partial Models.MedicalCondition ToMedicalCondition(MedicalConditionRequest medicalConditionRequest);
    public static partial void MapToExistingMedicalCondition(MedicalConditionRequest medicalConditionRequest, Models.MedicalCondition medicalCondition);
}
