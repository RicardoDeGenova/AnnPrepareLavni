using AnnPrepareLavni.ApiService.Features.Triage.Contracts;
using AnnPrepareLavni.ApiService.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace AnnPrepareLavni.ApiService.Features.Triage;

[ApiController]
[Route("[controller]")]
public class TriagesController : ControllerBase
{
    private readonly IValidator<TriageRequest> _validator;
    private readonly ITriageService _triageService;

    public TriagesController(
        IValidator<TriageRequest> validator,
        ITriageService triageService)
    {
        _validator = validator;
        _triageService = triageService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TriageResponse>>> GetTriages()
    {
        var triages = await _triageService.GetAllAsync();
        var triageResponseList = triages.Select(TriageMapper.ToResponse);
        return Ok(triageResponseList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TriageResponse>> GetTriage(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid triage ID.");
        }

        var triage = await _triageService.GetByIdAsync(id);
        if (triage == null)
        {
            return NotFound();
        }

        var triageResponse = TriageMapper.ToResponse(triage);
        return Ok(triageResponse);
    }

    [HttpPost]
    public async Task<ActionResult<TriageResponse>> CreateTriage([FromBody] TriageRequest triageRequest)
    {
        if (triageRequest is null)
        {
            return BadRequest(new { Message = "TriageRequest cannot be null" });
        }

        ValidationResult validationResult = await _validator.ValidateAsync(triageRequest);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var triage = TriageMapper.ToTriage(triageRequest);

        try
        {
            var result = await _triageService.CreateAsync(triage);
            if (!result)
            {
                return BadRequest("Unable to create triage. Check if NurseId is valid.");
            }
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the triage.");
        }

        var triageResponse = TriageMapper.ToResponse(triage);

        return CreatedAtAction(nameof(GetTriage), new { id = triage.Id }, triageResponse);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TriageResponse>> UpdateTriage(Guid id, [FromBody] TriageRequest triageRequest)
    {
        if (triageRequest is null)
        {
            return BadRequest("TriageRequest cannot be null");
        }

        if (id == Guid.Empty)
        {
            return BadRequest("Invalid triage ID.");
        }

        ValidationResult validationResult = await _validator.ValidateAsync(triageRequest);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var existingTriage = await _triageService.GetByIdAsync(id);
        if (existingTriage == null)
        {
            return NotFound(new { Message = $"Triage with ID {id} not found." });
        }

        TriageMapper.MapToExistingTriage(triageRequest, existingTriage);

        try
        {
            var result = await _triageService.UpdateAsync(existingTriage);
            if (!result)
            {
                return BadRequest("Unable to update triage. Check if Nurse ID is valid.");
            }
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the triage.");
        }

        var triageResponse = TriageMapper.ToResponse(existingTriage);

        return Ok(triageResponse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTriage(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid triage ID.");
        }

        var existingTriage = await _triageService.GetByIdAsync(id);
        if (existingTriage == null)
        {
            return NotFound();
        }

        try
        {
            await _triageService.DeleteAsync(id);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the triage.");
        }

        return Ok("Triage deleted successfully.");
    }
}
