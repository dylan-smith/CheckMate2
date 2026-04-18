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

        if (trimmedName.Length == 0)
        {
            ModelState.AddModelError(nameof(request.Name), "Checklist name is required.");
            return ValidationProblem(ModelState);
        }

        var duplicateName = await HasDuplicateNameAsync(trimmedName);

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
            var isDuplicateName = await HasDuplicateNameAsync(trimmedName);

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

        if (trimmedName.Length == 0)
        {
            ModelState.AddModelError(nameof(request.Name), "Checklist name is required.");
            return ValidationProblem(ModelState);
        }

        var duplicateName = await HasDuplicateNameAsync(trimmedName, id);

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
            var isDuplicateName = await HasDuplicateNameAsync(trimmedName, id);

            if (isDuplicateName)
            {
                return Conflict(new { message = "A checklist with this name already exists." });
            }

            throw;
        }
        return Ok(checklist);
    }

    private Task<bool> HasDuplicateNameAsync(string name, int? excludeId = null)
    {
        var query = dbContext.Checklists.AsNoTracking();

        if (excludeId is int id)
        {
            query = query.Where(item => item.Id != id);
        }

        if (dbContext.Database.IsSqlServer())
        {
            return query.AnyAsync(item => item.Name == name);
        }

        var normalizedName = name.ToUpperInvariant();
        return query.AnyAsync(item => item.Name.ToUpper() == normalizedName);
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
