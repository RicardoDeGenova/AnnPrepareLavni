using AnnPrepareLavni.ApiService;
using AnnPrepareLavni.ApiService.Data;
using AnnPrepareLavni.ApiService.Features.Address.Contracts;
using AnnPrepareLavni.ApiService.Features.MedicalCondition;
using AnnPrepareLavni.ApiService.Features.MedicalCondition.Contracts;
using AnnPrepareLavni.ApiService.Features.Medication;
using AnnPrepareLavni.ApiService.Features.Medication.Contracts;
using AnnPrepareLavni.ApiService.Features.Patient;
using AnnPrepareLavni.ApiService.Features.Patient.Contracts;
using AnnPrepareLavni.ApiService.Features.Prescription;
using AnnPrepareLavni.ApiService.Features.Prescription.Contracts;
using AnnPrepareLavni.ApiService.Features.Triage;
using AnnPrepareLavni.ApiService.Features.Triage.Contracts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("WebApiDatabase")));
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IMedicalConditionService, MedicalConditionService>();
builder.Services.AddScoped<IMedicationService, MedicationService>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<ITriageService, TriageService>();

builder.Services.AddValidatorsFromAssemblyContaining<PatientRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AddressRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<MedicalConditionRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<MedicationRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<PrescriptionRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TriageRequestValidator>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("NgrokPolicy", builder =>
        builder.WithOrigins("https://monarch-valued-explicitly.ngrok-free.app")
               .AllowAnyMethod()
               .AllowAnyHeader());
});

var app = builder.Build();
app.UseMiddleware<GlobalRoutePrefixMiddleware>("/api/v1"); 
app.UsePathBase(new PathString("/api/v1"));

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("NgrokPolicy");
app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
