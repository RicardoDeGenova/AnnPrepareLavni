using AnnPrepareLavni.API.Contracts.Responses;
using AnnPrepareLavni.API.Contracts;
using FastEndpoints;
using AnnPrepareLavni.API.Endpoints.Patients;

namespace AnnPrepareLavni.API.Features.Patients.GetPatient;

public class GetPatientsEndpoint : EndpointWithoutRequest<PatientsResponse>
{
    private readonly IPatientService _patientService;

    public GetPatientsEndpoint(IPatientService movieService)
    {
        _patientService = movieService;
    }

    public override void Configure()
    {
        Get("/patients");
        AllowAnonymous();
    }

    public override async Task<PatientsResponse> ExecuteAsync(CancellationToken ct)
    {
        var patients = await _patientService.GetAllAsync();
        return patients.MapToResponse();
    }

    // public override async Task HandleAsync(CancellationToken ct)
    // {
    //     var movies = await _movieService.GetAllAsync();
    //     var response = movies.MapToResponse();
    //     await SendOkAsync(response, ct);
    // }
}