using AnnPrepareLavni.ApiService.Features.MedicalCondition.Contracts;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace AnnPrepareLavni.ApiService.Features.MedicalCondition;

[ApiController]
[Route("[controller]")]
public class MedicalConditionsController : ControllerBase
{
    private readonly IValidator<MedicalConditionRequest> _validator;
    private readonly IMedicalConditionService _medicalConditionService;

    public MedicalConditionsController(
        IValidator<MedicalConditionRequest> validator,
        IMedicalConditionService medicalConditionService)
    {
        _validator = validator;
        _medicalConditionService = medicalConditionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicalConditionResponse>>> GetMedicalConditions()
    {
        var medicalConditions = await _medicalConditionService.GetAllAsync();
        var medicalConditionResponseList = medicalConditions.Select(MedicalConditionMapper.ToResponse);
        return Ok(medicalConditionResponseList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MedicalConditionResponse>> GetMedicalCondition(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid medical condition ID.");
        }

        var medicalCondition = await _medicalConditionService.GetByIdAsync(id);
        if (medicalCondition == null)
        {
            return NotFound();
        }

        var medicalConditionResponse = MedicalConditionMapper.ToResponse(medicalCondition);
        return Ok(medicalConditionResponse);
    }

    [HttpGet("byPatient/{patientId}")]
    public async Task<ActionResult<IEnumerable<MedicalConditionResponse>>> GetMedicalConditionsByPatientId(Guid patientId)
    {
        if (patientId == Guid.Empty)
        {
            return BadRequest("Invalid patient ID.");
        }

        var medicalConditions = await _medicalConditionService.GetByPatientIdAsync(patientId);
        var medicalConditionResponseList = medicalConditions.Select(MedicalConditionMapper.ToResponse);
        return Ok(medicalConditionResponseList);
    }

    [HttpPost]
    public async Task<ActionResult<MedicalConditionResponse>> CreateMedicalCondition([FromBody] MedicalConditionRequest medicalConditionRequest)
    {
        if (medicalConditionRequest is null)
        {
            return BadRequest(new { Message = "MedicalConditionRequest cannot be null" });
        }

        ValidationResult validationResult = await _validator.ValidateAsync(medicalConditionRequest);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var medicalCondition = MedicalConditionMapper.ToMedicalCondition(medicalConditionRequest);

        try
        {
            await _medicalConditionService.CreateAsync(medicalCondition);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the medical condition.");
        }

        var medicalConditionResponse = MedicalConditionMapper.ToResponse(medicalCondition);

        return CreatedAtAction(nameof(GetMedicalCondition), new { id = medicalCondition.Id }, medicalConditionResponse);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MedicalConditionResponse>> UpdateMedicalCondition(Guid id, [FromBody] MedicalConditionRequest medicalConditionRequest)
    {
        if (medicalConditionRequest is null)
        {
            return BadRequest("MedicalConditionRequest cannot be null");
        }

        if (id == Guid.Empty)
        {
            return BadRequest("Invalid medical condition ID.");
        }

        ValidationResult validationResult = await _validator.ValidateAsync(medicalConditionRequest);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var existingMedicalCondition = await _medicalConditionService.GetByIdAsync(id);
        if (existingMedicalCondition == null)
        {
            return NotFound(new { Message = $"Medical condition with ID {id} not found." });
        }

        MedicalConditionMapper.MapToExistingMedicalCondition(medicalConditionRequest, existingMedicalCondition);

        try
        {
            await _medicalConditionService.UpdateAsync(existingMedicalCondition);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the medical condition.");
        }

        var medicalConditionResponse = MedicalConditionMapper.ToResponse(existingMedicalCondition);

        return Ok(medicalConditionResponse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMedicalCondition(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid medical condition ID.");
        }

        var existingMedicalCondition = await _medicalConditionService.GetByIdAsync(id);
        if (existingMedicalCondition == null)
        {
            return NotFound();
        }

        try
        {
            await _medicalConditionService.DeleteAsync(id);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the medical condition.");
        }

        return Ok("Medical condition deleted successfully.");
    }
}
