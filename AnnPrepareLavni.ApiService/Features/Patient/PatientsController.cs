using AnnPrepareLavni.ApiService.Data;
using AnnPrepareLavni.ApiService.Features.Patient.Contracts;
using AnnPrepareLavni.ApiService.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnnPrepareLavni.ApiService.Features.Patient;

[ApiController]
[Route("[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IValidator<PatientRequest> _validator;
    private readonly AppDbContext _context;

    public PatientsController(IValidator<PatientRequest> validator, AppDbContext context)
    {
        this._validator = validator;
        this._context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientResponse>>> GetPatients()
    {
        var patients = await _context.Patients
                             .Include(p => p.Address)
                             .Include(p => p.MedicalConditions)
                             .ToListAsync();

        var patientResponseList = PatientMapper.MapToResponseList(patients);

        return Ok(patientResponseList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PatientResponse>> GetPatient(Guid id)
    {
        var patient = await _context.Patients
                                    .Include(p => p.Address)
                                    .Include(p => p.MedicalConditions)
                                    .FirstOrDefaultAsync(p => p.Id == id);

        if (patient == null)
        {
            return NotFound();
        }

        var patientResponse = PatientMapper.MapToResponse(patient);

        return Ok(patientResponse);
    }

    [HttpPost]
    public async Task<ActionResult<PatientResponse>> CreatePatient([FromBody] PatientRequest patientRequest)
    {
        if (patientRequest is null)
        {
            return BadRequest(new { Message = "PatientRequest cannot be null" });
        }

        ValidationResult result = await _validator.ValidateAsync(patientRequest);

        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var patient = PatientMapper.MapToPatient(patientRequest);

        patient.Id = Guid.NewGuid();
        patient.CreatedAt = DateTimeOffset.Now;
        patient.ModifiedAt = DateTimeOffset.Now;

        if (patient.Address != null)
        {
            patient.Address.Id = Guid.NewGuid();
            patient.Address.PatientId = patient.Id;
            _context.Addresses.Add(patient.Address);
        }

        _context.Patients.Add(patient);

        await _context.SaveChangesAsync();

        var patientResponse = PatientMapper.MapToResponse(patient);

        return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patientResponse);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PatientResponse>> UpdatePatient(Guid id, [FromBody] PatientRequest patientRequest)
    {
        if (patientRequest is null)
        {
            return BadRequest(new { Message = "PatientRequest cannot be null" });
        }

        ValidationResult result = await _validator.ValidateAsync(patientRequest);

        if (!result.IsValid)
        {
            result.AddToModelState(this.ModelState);
            return BadRequest(ModelState);
        }

        var existingPatient = await _context.Patients
                                            .Include(p => p.Address)
                                            .Include(p => p.MedicalConditions)
                                            .FirstOrDefaultAsync(p => p.Id == id);

        if (existingPatient == null)
        {
            return NotFound(new { Message = $"Patient with ID {id} not found." });
        }

        if (existingPatient.Address is null && patientRequest.Address is not null)
        {
            existingPatient.Address = new Models.Address()
            {
                Id = Guid.NewGuid(),
                Street1 = patientRequest.Address.Street1,
                Street2 = patientRequest.Address.Street2 ?? string.Empty,
                City = patientRequest.Address.City,
                State = patientRequest.Address.State,
                PostalCode = patientRequest.Address.PostalCode,
                Country = patientRequest.Address.Country
            };
        }

        PatientMapper.MapToExistingPatient(patientRequest, existingPatient);

        existingPatient.ModifiedAt = DateTimeOffset.Now;

        if (existingPatient.Address != null && existingPatient.Address.Id == Guid.Empty)
        {
            existingPatient.Address.Id = Guid.NewGuid();
            existingPatient.Address.PatientId = existingPatient.Id;
            _context.Addresses.Add(existingPatient.Address);
        }

        await _context.SaveChangesAsync();

        var patientResponse = PatientMapper.MapToResponse(existingPatient);

        return Ok(patientResponse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient(Guid id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null)
        {
            return NotFound();
        }

        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Patient deleted successfully." });
    }
}
