using CheckMate2.Api.Contracts;
using CheckMate2.Api.Data;
using CheckMate2.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheckMate2.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChecklistsController(ChecklistDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Checklist>>> GetAll()
    {
        var checklists = await dbContext.Checklists
            .AsNoTracking()
            .OrderBy(checklist => checklist.Name)
            .ToListAsync();

        return Ok(checklists);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Checklist>> GetById(int id)
    {
        var checklist = await dbContext.Checklists
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id);

        if (checklist is null)
        {
            return NotFound();
        }

        return Ok(checklist);
    }

    [HttpPost]
    public async Task<ActionResult<Checklist>> Create([FromBody] ChecklistRequest request)
    {
        var trimmedName = request.Name.Trim();
        var normalizedName = trimmedName.ToUpperInvariant();

        if (trimmedName.Length == 0)
        {
            ModelState.AddModelError(nameof(request.Name), "Checklist name is required.");
            return ValidationProblem(ModelState);
        }

        var duplicateName = await HasDuplicateNameAsync(normalizedName);

        if (duplicateName)
        {
            return Conflict(new { message = "A checklist with this name already exists." });
        }

        var checklist = new Checklist
        {
            Name = trimmedName
        };

        dbContext.Checklists.Add(checklist);
        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            var isDuplicateName = await HasDuplicateNameAsync(normalizedName);

            if (isDuplicateName)
            {
                return Conflict(new { message = "A checklist with this name already exists." });
            }

            throw;
        }

        return CreatedAtAction(nameof(GetById), new { id = checklist.Id }, checklist);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Checklist>> Update(int id, [FromBody] ChecklistRequest request)
    {
        var checklist = await dbContext.Checklists.FirstOrDefaultAsync(item => item.Id == id);

        if (checklist is null)
        {
            return NotFound();
        }

        var trimmedName = request.Name.Trim();
        var normalizedName = trimmedName.ToUpperInvariant();

        if (trimmedName.Length == 0)
        {
            ModelState.AddModelError(nameof(request.Name), "Checklist name is required.");
            return ValidationProblem(ModelState);
        }

        var duplicateName = await HasDuplicateNameAsync(normalizedName, id);

        if (duplicateName)
        {
            return Conflict(new { message = "A checklist with this name already exists." });
        }

        checklist.Name = trimmedName;

        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            var isDuplicateName = await HasDuplicateNameAsync(normalizedName, id);

            if (isDuplicateName)
            {
                return Conflict(new { message = "A checklist with this name already exists." });
            }

            throw;
        }
        return Ok(checklist);
    }

    private Task<bool> HasDuplicateNameAsync(string normalizedName, int? excludeId = null)
    {
        var query = dbContext.Checklists
            .AsNoTracking()
            .Where(item => item.Name.ToUpper() == normalizedName);

        if (excludeId is int id)
        {
            query = query.Where(item => item.Id != id);
        }

        return query.AnyAsync();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var checklist = await dbContext.Checklists.FirstOrDefaultAsync(item => item.Id == id);

        if (checklist is null)
        {
            return NotFound();
        }

        dbContext.Checklists.Remove(checklist);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}
