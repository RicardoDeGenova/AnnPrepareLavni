﻿using Riok.Mapperly.Abstractions;

namespace AnnPrepareLavni.ApiService.Features.MedicalCondition.Contracts;

[Mapper]
public static partial class MedicalConditionMapper
{
    public static partial MedicalConditionResponse MapToResponse(Models.MedicalCondition medicalCondition);
    public static partial IEnumerable<MedicalConditionResponse> MapToResponseList(IEnumerable<Models.MedicalCondition> medicalCondition);
    public static partial Models.MedicalCondition MapToMedicalCondition(MedicalConditionRequest medicalConditionRequest);
}