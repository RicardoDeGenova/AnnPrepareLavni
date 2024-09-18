using AnnPrepareLavni.ApiService;
using AnnPrepareLavni.ApiService.Data;
using AnnPrepareLavni.ApiService.Features.Address.Contracts;
using AnnPrepareLavni.ApiService.Features.MedicalCondition.Contracts;
using AnnPrepareLavni.ApiService.Features.Patient.Contracts;
using AnnPrepareLavni.ApiService.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("WebApiDatabase")));

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

builder.Services.AddValidatorsFromAssemblyContaining<PatientRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AddressRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<MedicalConditionRequestValidator>();

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
