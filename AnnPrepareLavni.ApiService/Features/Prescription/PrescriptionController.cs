using AnnPrepareLavni.ApiService.Features.Prescription.Contracts;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnnPrepareLavni.ApiService.Features.Prescription;

[Authorize]
[ApiController]
[Route("[controller]")]
public class PrescriptionsController : ControllerBase
{
    private readonly IValidator<PrescriptionRequest> _validator;
    private readonly IPrescriptionService _prescriptionService;

    public PrescriptionsController(
        IValidator<PrescriptionRequest> validator,
        IPrescriptionService prescriptionService)
    {
        _validator = validator;
        _prescriptionService = prescriptionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PrescriptionResponse>>> GetPrescriptions()
    {
        var prescriptions = await _prescriptionService.GetAllAsync();
        var prescriptionResponseList = prescriptions.Select(PrescriptionMapper.ToResponse);
        return Ok(prescriptionResponseList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PrescriptionResponse>> GetPrescription(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid prescription ID.");
        }

        var prescription = await _prescriptionService.GetByIdAsync(id);
        if (prescription == null)
        {
            return NotFound();
        }

        var prescriptionResponse = PrescriptionMapper.ToResponse(prescription);
        return Ok(prescriptionResponse);
    }

    [HttpPost]
    public async Task<ActionResult<PrescriptionResponse>> CreatePrescription([FromBody] PrescriptionRequest prescriptionRequest)
    {
        if (prescriptionRequest is null)
        {
            return BadRequest(new { Message = "PrescriptionRequest cannot be null" });
        }

        ValidationResult validationResult = await _validator.ValidateAsync(prescriptionRequest);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var prescription = PrescriptionMapper.ToPrescription(prescriptionRequest);

        try
        {
            await _prescriptionService.CreateAsync(prescription);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the prescription.");
        }

        var prescriptionResponse = PrescriptionMapper.ToResponse(prescription);

        return CreatedAtAction(nameof(GetPrescription), new { id = prescription.Id }, prescriptionResponse);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PrescriptionResponse>> UpdatePrescription(Guid id, [FromBody] PrescriptionRequest prescriptionRequest)
    {
        if (prescriptionRequest is null)
        {
            return BadRequest("PrescriptionRequest cannot be null");
        }

        if (id == Guid.Empty)
        {
            return BadRequest("Invalid prescription ID.");
        }

        ValidationResult validationResult = await _validator.ValidateAsync(prescriptionRequest);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var existingPrescription = await _prescriptionService.GetByIdAsync(id);
        if (existingPrescription == null)
        {
            return NotFound(new { Message = $"Prescription with ID {id} not found." });
        }

        PrescriptionMapper.MapToExistingPrescription(prescriptionRequest, existingPrescription);

        try
        {
            await _prescriptionService.UpdateAsync(existingPrescription);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the prescription.");
        }

        var prescriptionResponse = PrescriptionMapper.ToResponse(existingPrescription);

        return Ok(prescriptionResponse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePrescription(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid prescription ID.");
        }

        var existingPrescription = await _prescriptionService.GetByIdAsync(id);
        if (existingPrescription == null)
        {
            return NotFound();
        }

        try
        {
            await _prescriptionService.DeleteAsync(id);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the prescription.");
        }

        return Ok("Prescription deleted successfully.");
    }
}
