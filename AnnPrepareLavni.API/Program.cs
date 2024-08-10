using FluentValidation;
using FastEndpoints;
using AnnPrepareLavni.API.Database;
using AnnPrepareLavni.API.Endpoints.Patients;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
//config.AddEnvironmentVariables("");

builder.Services.AddFastEndpoints();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDbConnectionFactory>(_ => new DbConnectionFactory(config["Database:ConnectionString"]!));
builder.Services.AddSingleton(_ => new DatabaseInitializer(config["Database:ConnectionString"]!));

builder.Services.AddSingleton<IPatientService, PatientService>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseFastEndpoints();

var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await databaseInitializer.InitializeAsync();

app.Run();
