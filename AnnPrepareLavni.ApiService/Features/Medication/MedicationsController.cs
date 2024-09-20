using AnnPrepareLavni.ApiService.Features.Medication.Contracts;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace AnnPrepareLavni.ApiService.Features.Medication;

[ApiController]
[Route("[controller]")]
public class MedicationsController : ControllerBase
{
    private readonly IValidator<MedicationRequest> _validator;
    private readonly IMedicationService _medicationService;

    public MedicationsController(
        IValidator<MedicationRequest> validator,
        IMedicationService medicationService)
    {
        _validator = validator;
        _medicationService = medicationService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicationResponse>>> GetMedications()
    {
        var medications = await _medicationService.GetAllAsync();
        var medicationResponseList = medications.Select(MedicationMapper.ToResponse);
        return Ok(medicationResponseList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MedicationResponse>> GetMedication(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid medication ID.");
        }

        var medication = await _medicationService.GetByIdAsync(id);
        if (medication == null)
        {
            return NotFound();
        }

        var medicationResponse = MedicationMapper.ToResponse(medication);
        return Ok(medicationResponse);
    }

    [HttpGet("byName/{name}")]
    public async Task<ActionResult<IEnumerable<MedicationResponse>>> GetMedicationsByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Medication name cannot be null or empty.");
        }

        var medications = await _medicationService.GetByNameAsync(name);
        var medicationResponseList = medications.Select(MedicationMapper.ToResponse);
        return Ok(medicationResponseList);
    }

    [HttpPost]
    public async Task<ActionResult<MedicationResponse>> CreateMedication([FromBody] MedicationRequest medicationRequest)
    {
        if (medicationRequest is null)
        {
            return BadRequest(new { Message = "MedicationRequest cannot be null" });
        }

        ValidationResult validationResult = await _validator.ValidateAsync(medicationRequest);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var medication = MedicationMapper.ToMedication(medicationRequest);

        try
        {
            await _medicationService.CreateAsync(medication);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the medication.");
        }

        var medicationResponse = MedicationMapper.ToResponse(medication);

        return CreatedAtAction(nameof(GetMedication), new { id = medication.Id }, medicationResponse);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MedicationResponse>> UpdateMedication(Guid id, [FromBody] MedicationRequest medicationRequest)
    {
        if (medicationRequest is null)
        {
            return BadRequest("MedicationRequest cannot be null");
        }

        if (id == Guid.Empty)
        {
            return BadRequest("Invalid medication ID.");
        }

        ValidationResult validationResult = await _validator.ValidateAsync(medicationRequest);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var existingMedication = await _medicationService.GetByIdAsync(id);
        if (existingMedication == null)
        {
            return NotFound(new { Message = $"Medication with ID {id} not found." });
        }

        MedicationMapper.MapToExistingMedication(medicationRequest, existingMedication);

        try
        {
            await _medicationService.UpdateAsync(existingMedication);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the medication.");
        }

        var medicationResponse = MedicationMapper.ToResponse(existingMedication);

        return Ok(medicationResponse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMedication(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid medication ID.");
        }

        var existingMedication = await _medicationService.GetByIdAsync(id);
        if (existingMedication == null)
        {
            return NotFound();
        }

        try
        {
            await _medicationService.DeleteAsync(id);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the medication.");
        }

        return Ok("Medication deleted successfully.");
    }
}
