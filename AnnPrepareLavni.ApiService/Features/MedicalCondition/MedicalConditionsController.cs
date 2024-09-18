using AnnPrepareLavni.ApiService.Data;
using AnnPrepareLavni.ApiService.Features.MedicalCondition.Contracts;
using AnnPrepareLavni.ApiService.Features.Patient.Contracts;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnnPrepareLavni.ApiService.Features.MedicalCondition;

[Route("[controller]")]
[ApiController]
public class MedicalConditionsController : ControllerBase
{
    private readonly IValidator<MedicalConditionRequest> _validator;
    private readonly AppDbContext _context;

    public MedicalConditionsController(IValidator<MedicalConditionRequest> validator, AppDbContext context)
    {
        _validator = validator;
        _context = context;
    }

    [HttpGet("patient/{patientId}")]
    public async Task<ActionResult<IEnumerable<MedicalConditionResponse>>> GetMedicalConditionsByPatient(Guid patientId)
    {
        var patient = await _context.Patients
                                    .Include(p => p.MedicalConditions)
                                    .FirstOrDefaultAsync(p => p.Id == patientId);

        if (patient == null)
        {
            return NotFound(new { Message = $"Patient with ID {patientId} not found." });
        }

        var medicalConditions = patient.MedicalConditions;
        var medicalConditionResponses = MedicalConditionMapper.MapToResponseList(medicalConditions);

        return Ok(medicalConditionResponses);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MedicalConditionResponse>> GetMedicalCondition(Guid id)
    {
        var medicalCondition = await _context.MedicalConditions
                                             .Include(mc => mc.Patient)
                                             .FirstOrDefaultAsync(mc => mc.Id == id);

        if (medicalCondition == null)
        {
            return NotFound(new { Message = $"Medical condition with ID {id} not found." });
        }

        var medicalConditionResponse = MedicalConditionMapper.MapToResponse(medicalCondition);

        return Ok(medicalConditionResponse);
    }

    [HttpPost]
    public async Task<ActionResult<MedicalConditionResponse>> CreateMedicalCondition([FromBody] MedicalConditionRequest medicalConditionRequest)
    {
        if (medicalConditionRequest is null)
        {
            return BadRequest(new { Message = "MedicalConditionRequest cannot be null" });
        }

        ValidationResult result = await _validator.ValidateAsync(medicalConditionRequest);

        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var patient = await _context.Patients
                                    .Include(p => p.MedicalConditions)
                                    .FirstOrDefaultAsync(p => p.Id == medicalConditionRequest.PatientId);

        if (patient == null)
        {
            return NotFound(new { Message = $"Patient with ID {medicalConditionRequest.PatientId} not found." });
        }

        var medicalCondition = MedicalConditionMapper.MapToMedicalCondition(medicalConditionRequest);
        medicalCondition.Id = Guid.NewGuid();
        medicalCondition.CreatedAt = DateTimeOffset.Now;
        medicalCondition.ModifiedAt = DateTimeOffset.Now;

        _context.MedicalConditions.Add(medicalCondition);
        await _context.SaveChangesAsync();

        var medicalConditionResponse = MedicalConditionMapper.MapToResponse(medicalCondition);

        return CreatedAtAction(nameof(GetMedicalCondition), new { id = medicalCondition.Id }, medicalConditionResponse);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MedicalConditionResponse>> UpdateMedicalCondition(Guid id, [FromBody] MedicalConditionRequest medicalConditionRequest)
    {
        if (medicalConditionRequest is null)
        {
            return BadRequest(new { Message = "MedicalConditionRequest cannot be null" });
        }

        ValidationResult result = await _validator.ValidateAsync(medicalConditionRequest);

        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var existingMedicalCondition = await _context.MedicalConditions
                                                     .Include(mc => mc.Patient)
                                                     .FirstOrDefaultAsync(mc => mc.Id == id);

        if (existingMedicalCondition == null)
        {
            return NotFound(new { Message = $"Medical condition with ID {id} not found." });
        }

        MedicalConditionMapper.MapToExistingMedicalCondition(medicalConditionRequest, existingMedicalCondition);

        existingMedicalCondition.ModifiedAt = DateTimeOffset.Now;

        await _context.SaveChangesAsync();

        var medicalConditionResponse = MedicalConditionMapper.MapToResponse(existingMedicalCondition);

        return Ok(medicalConditionResponse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMedicalCondition(Guid id)
    {
        var medicalCondition = await _context.MedicalConditions.FindAsync(id);
        if (medicalCondition == null)
        {
            return NotFound(new { Message = $"Medical condition with ID {id} not found." });
        }

        _context.MedicalConditions.Remove(medicalCondition);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Medical condition deleted successfully." });
    }
}
