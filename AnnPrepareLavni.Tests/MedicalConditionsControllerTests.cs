using AnnPrepareLavni.ApiService.Data;
using AnnPrepareLavni.ApiService.Features.MedicalCondition;
using AnnPrepareLavni.ApiService.Features.MedicalCondition.Contracts;
using AnnPrepareLavni.ApiService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnnPrepareLavni.Tests;

[TestClass]
public class MedicalConditionsControllerTests
{
    private AppDbContext _context;
    private MedicalConditionsController _controller;

    [TestInitialize]
    public void TestInitialize()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _controller = new MedicalConditionsController(_context);

        SeedTestData();
    }

    [TestMethod]
    public async Task GetMedicalConditionsByPatient_ShouldReturnMedicalConditions()
    {
        var patientId = _context.Patients.First().Id;

        var request = await _controller.GetMedicalConditionsByPatient(patientId);
        var result = request.Result as OkObjectResult;
        Assert.IsNotNull(result);

        var medicalConditions = result.Value as IEnumerable<MedicalConditionResponse>;
        Assert.AreEqual(2, medicalConditions?.Count());
    }

    [TestMethod]
    public async Task GetMedicalCondition_ShouldReturnMedicalCondition()
    {
        var medicalConditionId = _context.MedicalConditions.First().Id;

        var request = await _controller.GetMedicalCondition(medicalConditionId);
        var result = request.Result as OkObjectResult;
        Assert.IsNotNull(result);

        var medicalCondition = result.Value as MedicalConditionResponse;
        Assert.AreEqual("Hypertension", medicalCondition?.Name);
    }

    [TestMethod]
    public async Task CreateMedicalCondition_ShouldAddMedicalCondition()
    {
        var patientId = _context.Patients.First().Id;
        var newMedicalConditionRequest = new MedicalConditionRequest
        {
            PatientId = patientId,
            Name = "Asthma",
            ConditionType = MedicalConditionType.Chronic,
            StartedAt = DateTimeOffset.Now.AddYears(-2),
            StoppedAt = DateTimeOffset.Now
        };

        var request = await _controller.CreateMedicalCondition(newMedicalConditionRequest);
        var result = request.Result as CreatedAtActionResult;
        Assert.IsNotNull(result);

        var createdMedicalCondition = result.Value as MedicalConditionResponse;
        Assert.AreEqual("Asthma", createdMedicalCondition?.Name);
        Assert.AreEqual(3, _context.MedicalConditions.Count());
    }

    [TestMethod]
    public async Task UpdateMedicalCondition_ShouldUpdateExistingCondition()
    {
        var existingMedicalCondition = _context.MedicalConditions.First();
        var updatedRequest = new MedicalConditionRequest
        {
            PatientId = existingMedicalCondition.PatientId,
            Name = "Hypertension (Updated)",
            ConditionType = MedicalConditionType.Current,
            StartedAt = existingMedicalCondition.StartedAt,
            StoppedAt = existingMedicalCondition.StoppedAt
        };

        var request = await _controller.UpdateMedicalCondition(existingMedicalCondition.Id, updatedRequest);
        var result = request.Result as OkObjectResult;
        Assert.IsNotNull(result);

        var updatedCondition = result.Value as MedicalConditionResponse;
        Assert.AreEqual("Hypertension (Updated)", updatedCondition?.Name);

        var conditionInDb = await _context.MedicalConditions.FindAsync(existingMedicalCondition.Id);
        Assert.AreEqual("Hypertension (Updated)", conditionInDb?.Name);
    }

    [TestMethod]
    public async Task DeleteMedicalCondition_ShouldRemoveMedicalCondition()
    {
        var medicalConditionId = _context.MedicalConditions.First().Id;

        var result = await _controller.DeleteMedicalCondition(medicalConditionId) as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(1, _context.MedicalConditions.Count());
    }

    private void SeedTestData()
    {
        var patient = new Patient
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Address = new Address
            {
                Id = Guid.NewGuid(),
                Street1 = "123 Main St",
                City = "Springfield",
                State = "IL",
                PostalCode = "62701",
                Country = "USA"
            }
        };

        var medicalConditions = new List<MedicalCondition>
        {
            new MedicalCondition
            {
                Id = Guid.NewGuid(),
                PatientId = patient.Id,
                Name = "Hypertension",
                ConditionType = MedicalConditionType.Chronic,
                StartedAt = DateTimeOffset.Now.AddYears(-3),
                StoppedAt = DateTimeOffset.Now,
                CreatedAt = DateTimeOffset.Now,
                ModifiedAt = DateTimeOffset.Now
            },
            new MedicalCondition
            {
                Id = Guid.NewGuid(),
                PatientId = patient.Id,
                Name = "Diabetes",
                ConditionType = MedicalConditionType.Current,
                StartedAt = DateTimeOffset.Now.AddYears(-5),
                StoppedAt = DateTimeOffset.Now,
                CreatedAt = DateTimeOffset.Now,
                ModifiedAt = DateTimeOffset.Now
            }
        };

        _context.Patients.Add(patient);
        _context.MedicalConditions.AddRange(medicalConditions);
        _context.SaveChanges();
    }
}
