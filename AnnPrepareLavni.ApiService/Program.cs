using AnnPrepareLavni.ApiService;
using AnnPrepareLavni.ApiService.Data;
using AnnPrepareLavni.ApiService.Features.Address.Contracts;
using AnnPrepareLavni.ApiService.Features.Authentication;
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
using AnnPrepareLavni.ApiService.Features.User;
using AnnPrepareLavni.ApiService.Features.User.Contracts;
using AnnPrepareLavni.ApiService.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("WebApiDatabase"))); 

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IMedicalConditionService, MedicalConditionService>();
builder.Services.AddScoped<IMedicationService, MedicationService>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<ITriageService, TriageService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddValidatorsFromAssemblyContaining<PatientRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AddressRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<MedicalConditionRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<MedicationRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<PrescriptionRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TriageRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UserRequestValidator>();

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
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});
var jwtSecretKey = builder.Configuration["JwtSettings:SecretKey"]!;
var key = Encoding.ASCII.GetBytes(jwtSecretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // For local testing without HTTPS
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    context.Database.Migrate();

    var configuration = services.GetRequiredService<IConfiguration>();
    var userService = services.GetRequiredService<IUserService>();
    await DatabaseInitialization.SeedAdminUserAsync(userService, configuration);
}

app.UseMiddleware<GlobalRoutePrefixMiddleware>("/api/v1"); 
app.UsePathBase(new PathString("/api/v1"));

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowAll");
}

app.UseCors("NgrokPolicy");
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
