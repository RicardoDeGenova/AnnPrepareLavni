using AnnPrepareLavni.ApiService.Data;
using AnnPrepareLavni.ApiService.Features.Patient.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnnPrepareLavni.ApiService.Features.Patient;

[Route("api/[controller]")]
[ApiController]
public class PatientsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PatientsController(AppDbContext context)
    {
        this._context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientResponse>>> GetPatients()
    {
        var patients = await _context.Patients
                             .Include(p => p.Address)
                             .Include(p => p.MedicalConditions)
                             .ToListAsync();

        var patientResponse = PatientMapper.MapToResponseList(patients);

        return Ok(patientResponse);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Models.Patient>> GetPatient(Guid id)
    {
        var patient = await _context.Patients
                                    .Include(p => p.Address)
                                    .Include(p => p.MedicalConditions)
                                    .FirstOrDefaultAsync(p => p.Id == id);

        if (patient == null)
        {
            return NotFound();
        }

        return Ok(patient);
    }

    [HttpPost]
    public async Task<ActionResult<PatientResponse>> CreatePatient([FromBody] PatientRequest patientRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var patient = PatientMapper.MapToPatient(patientRequest);

        patient.Id = Guid.NewGuid();
        patient.CreatedAt = DateTimeOffset.Now;
        patient.ModifiedAt = DateTimeOffset.Now;

        if (patient.Address != null)
        {
            patient.Address.Id = Guid.NewGuid();
        }

        _context.Patients.Add(patient);

        await _context.SaveChangesAsync();

        var patientResponse = PatientMapper.MapToResponse(patient);

        return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patientResponse);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PatientResponse>> UpdatePatient(Guid id, [FromBody] PatientRequest patientRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var patient = PatientMapper.MapToPatient(patientRequest);

        var existingPatient = await _context.Patients
                                            .Include(p => p.Address)
                                            .Include(p => p.MedicalConditions)
                                            .FirstOrDefaultAsync(p => p.Id == id);

        if (existingPatient == null)
        {
            return NotFound(new { Message = $"Patient with ID {id} not found." });
        }

        patient.ModifiedAt = DateTimeOffset.Now;

        if (patient.Address != null && patient.Address.Id == Guid.Empty)
        {
            patient.Address.Id = Guid.NewGuid();
        }

        await _context.SaveChangesAsync();

        var patientResponse = PatientMapper.MapToResponse(patient);

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
