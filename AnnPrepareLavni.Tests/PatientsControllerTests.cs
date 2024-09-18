using AnnPrepareLavni.ApiService.Data;
using AnnPrepareLavni.ApiService.Features.Address.Contracts;
using AnnPrepareLavni.ApiService.Features.Patient;
using AnnPrepareLavni.ApiService.Features.Patient.Contracts;
using AnnPrepareLavni.ApiService.Models;
using AnnPrepareLavni.ApiService.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnnPrepareLavni.Tests;

[TestClass]
public class PatientsControllerTests
{
    private ApplicationDbContext _context;
    private PatientsController _controller;

    [TestInitialize]
    public void TestInitialize()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _controller = new PatientsController(new PatientRequestValidator(), _context);

        SeedTestData();
    }

    #region HAPPY PATH

    [TestMethod]
    public async Task GetPatients_ShouldReturnAllPatients()
    {
        var result = await _controller.GetPatients();
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);

        var patients = okResult.Value as IEnumerable<PatientResponse>;
        Assert.AreEqual(2, patients?.Count());
    }

    [TestMethod]
    public async Task GetPatient_ShouldReturnPatientById()
    {
        var patientId = _context.Patients.First().Id;

        var result = await _controller.GetPatient(patientId);
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);

        var patient = okResult.Value as PatientResponse;
        Assert.AreEqual("John", patient?.FirstName);
    }

    [TestMethod]
    public async Task CreatePatient_ShouldAddNewPatient()
    {
        var newPatientRequest = new PatientRequest
        {
            FirstName = "Alice",
            LastName = "Smith",
            DateOfBirth = new DateTime(1985, 6, 15),
            Gender = Gender.Female,
            Address = new AddressRequest
            {
                Street1 = "456 Main St",
                City = "Springfield",
                State = "IL",
                PostalCode = "62704",
                Country = "USA"
            }
        };

        var result = await _controller.CreatePatient(newPatientRequest);
        var createdResult = result.Result as CreatedAtActionResult;
        Assert.IsNotNull(createdResult);

        var createdPatient = createdResult.Value as PatientResponse;
        Assert.AreEqual("Alice", createdPatient?.FirstName);
        Assert.AreEqual(3, _context.Patients.Count());
    }

    [TestMethod]
    public async Task UpdatePatient_ShouldModifyExistingPatient()
    {
        var existingPatient = _context.Patients.First();

        var updatedRequest = new PatientRequest
        {
            FirstName = "John",
            LastName = "Doe Updated",
            DateOfBirth = existingPatient.DateOfBirth,
            Gender = Gender.Male,    
        };

        if (existingPatient.Address is not null)
        {
            updatedRequest.Address = new AddressRequest
            {
                Street1 = existingPatient.Address.Street1,
                City = existingPatient.Address.City,
                State = existingPatient.Address.State,
                PostalCode = existingPatient.Address.PostalCode,
                Country = existingPatient.Address.Country
            };
        }

        var result = await _controller.UpdatePatient(existingPatient.Id, updatedRequest);
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);

        var updatedPatient = okResult.Value as PatientResponse;
        Assert.AreEqual("Doe Updated", updatedPatient?.LastName);

        var patientInDb = await _context.Patients.FindAsync(existingPatient.Id);
        Assert.AreEqual("Doe Updated", patientInDb?.LastName);
    }

    [TestMethod]
    public async Task DeletePatient_ShouldRemovePatient()
    {
        var patientId = _context.Patients.First().Id;

        var result = await _controller.DeletePatient(patientId);
        var okResult = result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(1, _context.Patients.Count());
    }

    #endregion

    #region SAD PATH

    [TestMethod]
    public async Task GetPatient_ShouldReturnNotFound_ForNonExistingPatient()
    {
        var result = await _controller.GetPatient(Guid.NewGuid());
        var notFoundResult = result.Result as NotFoundResult;
        Assert.IsNotNull(notFoundResult);
    }

    [TestMethod]
    public async Task CreatePatient_ShouldReturnBadRequest_WhenRequiredFieldsAreMissing()
    {
        var invalidPatientRequest = new PatientRequest
        {
            Gender = Gender.Male,
            Address = new AddressRequest
            {
                Street1 = "456 Main St",
                City = "Springfield",
                State = "IL",
                PostalCode = "62704",
                Country = "USA"
            }
        };

        var result = await _controller.CreatePatient(invalidPatientRequest);
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);

        var modelState = badRequestResult.Value as SerializableError;
        Assert.IsTrue(modelState?.ContainsKey(nameof(PatientRequest.FirstName)));
        Assert.IsTrue(modelState?.ContainsKey(nameof(PatientRequest.LastName)));
        Assert.IsTrue(modelState?.ContainsKey(nameof(PatientRequest.DateOfBirth)));

        Assert.IsFalse(modelState?.ContainsKey(nameof(PatientRequest.Gender)));
    }

    [TestMethod]
    public async Task CreatePatient_ShouldReturnBadRequest_WhenRequiredFieldsAreMissing2()
    {
        var invalidPatientRequest = new PatientRequest
        {
            FirstName = "Alice",
            LastName = "Smith",
            DateOfBirth = new DateTime(1990, 1, 1),
        };

        var result = await _controller.CreatePatient(invalidPatientRequest);
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);

        var modelState = badRequestResult.Value as SerializableError;
        Assert.IsTrue(modelState?.ContainsKey(nameof(PatientRequest.Gender)));

        Assert.IsFalse(modelState?.ContainsKey(nameof(PatientRequest.FirstName)));
        Assert.IsFalse(modelState?.ContainsKey(nameof(PatientRequest.LastName)));
        Assert.IsFalse(modelState?.ContainsKey(nameof(PatientRequest.DateOfBirth)));
    }

    [TestMethod]
    public async Task CreatePatient_ShouldReturnBadRequest_WhenFieldsAreInvalid()
    {
        var invalidPatientRequest = new PatientRequest
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = DateTime.Now.AddYears(1),
            Gender = (Gender)99,
        };

        var result = await _controller.CreatePatient(invalidPatientRequest);
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);

        var modelState = badRequestResult.Value as SerializableError;
        Assert.IsTrue(modelState?.ContainsKey(nameof(PatientRequest.DateOfBirth)));
        Assert.IsTrue(modelState?.ContainsKey(nameof(PatientRequest.Gender)));
    }

    [TestMethod]
    public async Task CreatePatient_ShouldReturnBadRequest_WhenInputsExceedMaxLength()
    {
        var invalidPatientRequest = new PatientRequest
        {
            FirstName = new string('A', 51),  // 51 characters, exceeding limit
            LastName = new string('B', 101),  // 101 characters, exceeding limit
            DateOfBirth = new DateTime(1990, 1, 1),
            Gender = Gender.Male
        };

        var result = await _controller.CreatePatient(invalidPatientRequest);
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);

        var modelState = badRequestResult.Value as SerializableError;
        Assert.IsTrue(modelState?.ContainsKey(nameof(PatientRequest.FirstName)));
        Assert.IsTrue(modelState?.ContainsKey(nameof(PatientRequest.LastName)));
    }

    [TestMethod]
    public async Task CreatePatient_ShouldReturnBadRequest_WhenPayloadIsEmpty()
    {
        var result = await _controller.CreatePatient(null);
        var badRequestResult = result.Result as BadRequestObjectResult;

        Assert.IsNotNull(badRequestResult);
    }


    #endregion

    #region SEED

    private void SeedTestData()
    {
        var patients = new List<Patient>
        {
            new Patient
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Male,
                Address = new Address
                {
                    Id = Guid.NewGuid(),
                    Street1 = "123 Main St",
                    City = "Springfield",
                    State = "IL",
                    PostalCode = "62701",
                    Country = "USA"
                },
                CreatedAt = DateTimeOffset.Now,
                ModifiedAt = DateTimeOffset.Now
            },
            new Patient
            {
                Id = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Doe",
                DateOfBirth = new DateTime(1992, 1, 1),
                Gender = Gender.Female,
                Address = new Address
                {
                    Id = Guid.NewGuid(),
                    Street1 = "123 Main St",
                    City = "Springfield",
                    State = "IL",
                    PostalCode = "62701",
                    Country = "USA"
                },
                CreatedAt = DateTimeOffset.Now,
                ModifiedAt = DateTimeOffset.Now
            }
        };

        _context.Patients.AddRange(patients);
        _context.SaveChanges();
    }

    #endregion
}
