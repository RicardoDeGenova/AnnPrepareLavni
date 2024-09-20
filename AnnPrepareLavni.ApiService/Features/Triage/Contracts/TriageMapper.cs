using Riok.Mapperly.Abstractions;

namespace AnnPrepareLavni.ApiService.Features.Triage.Contracts;

[Mapper]
public static partial class TriageMapper
{
    public static partial TriageResponse ToResponse(Models.Triage entity);
    public static partial void MapToExistingTriage(TriageRequest request, Models.Triage entity);
    public static partial Models.Triage ToTriage(TriageRequest request);
}
