using AnnPrepareLavni.ApiService.Features.Address.Contracts;
using AnnPrepareLavni.ApiService.Features.Patient.Contracts;
using AnnPrepareLavni.ApiService.Models;
using AnnPrepareLavni.ApiService.Models.Enums;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnnPrepareLavni.ApiService.Features.Patient;

[Authorize]
[ApiController]
[Route("[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IValidator<PatientRequest> _validator;
    private readonly IPatientService _patientService;

    public PatientsController(
        IValidator<PatientRequest> validator, 
        IPatientService patientService)
    {
        _validator = validator;
        _patientService = patientService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientResponse>>> GetPatients()
    {
        var patients = await _patientService.GetAllAsync();
        var patientResponseList = patients.Select(PatientMapper.ToResponse);
        return Ok(patientResponseList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PatientResponse>> GetPatient(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid patient ID.");
        }

        var patient = await _patientService.GetByIdAsync(id);
        if (patient == null)
        {
            return NotFound();
        }

        var patientResponse = PatientMapper.ToResponse(patient);
        return Ok(patientResponse);
    }

    [HttpGet("byPatientNo/{patientNo}")]
    public async Task<ActionResult<PatientResponse>> GetPatient(int patientNo)
    {
        if (patientNo == 0)
        {
            return BadRequest("patientNo cannot be 0.");
        }
        
        var patient = await _patientService.GetByPatientNo(patientNo);
        if (patient == null)
        {
            return NotFound();
        }

        var patientResponse = PatientMapper.ToResponse(patient);
        return Ok(patientResponse);
    }

    [HttpPost]
    public async Task<ActionResult<PatientResponse>> CreatePatient([FromBody] PatientRequest patientRequest)
    {
        if (patientRequest is null)
        {
            return BadRequest(new { Message = "PatientRequest cannot be null" });
        }

        ValidationResult validationResult = await _validator.ValidateAsync(patientRequest);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var patient = PatientMapper.ToPatient(patientRequest);

        try
        {
            await _patientService.CreateAsync(patient);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the patient.");
        }

        var patientResponse = PatientMapper.ToResponse(patient);

        return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patientResponse);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PatientResponse>> UpdatePatient(Guid id, [FromBody] PatientRequest patientRequest)
    {
        if (patientRequest is null)
        {
            return BadRequest("PatientRequest cannot be null");
        }

        if (id == Guid.Empty)
        {
            return BadRequest("Invalid patient ID.");
        }

        ValidationResult validationResult = await _validator.ValidateAsync(patientRequest);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var existingPatient = await _patientService.GetByIdAsync(id);
        if (existingPatient == null)
        {
            return NotFound(new { Message = $"Patient with ID {id} not found." });
        }

        if (existingPatient.Address is null && patientRequest.Address is not null)
        {
            var address = AddressMapper.ToAddress(patientRequest.Address);
            await _patientService.CreateNewAddressAsync(address, id);
        }

        PatientMapper.MapToExistingPatient(patientRequest, existingPatient);

        try
        {
            await _patientService.UpdateAsync(existingPatient);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the patient.");
        }

        var patientResponse = PatientMapper.ToResponse(existingPatient);

        return Ok(patientResponse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid patient ID.");
        }

        var existingPatient = await _patientService.GetByIdAsync(id);
        if (existingPatient == null)
        {
            return NotFound();
        }

        try
        {
            await _patientService.DeleteAsync(id);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the patient.");
        }

        return Ok("Patient deleted successfully.");
    }
}