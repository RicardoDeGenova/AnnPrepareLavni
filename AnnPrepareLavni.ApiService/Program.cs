using AnnPrepareLavni.ApiService.Data;
using AnnPrepareLavni.ApiService.Features.Patient;
using AnnPrepareLavni.ApiService.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("NgrokPolicy", builder =>
        builder.WithOrigins("https://monarch-valued-explicitly.ngrok-free.app")
               .AllowAnyMethod()
               .AllowAnyHeader());
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("AnnPrepareLavniDb"));

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
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
