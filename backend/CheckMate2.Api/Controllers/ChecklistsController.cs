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

        var duplicateName = await dbContext.Checklists
            .AsNoTracking()
            .AnyAsync(item => item.Name == trimmedName);

        if (duplicateName)
        {
            return Conflict(new { message = "A checklist with this name already exists." });
        }

        var checklist = new Checklist
        {
            Name = trimmedName
        };

        dbContext.Checklists.Add(checklist);
        await dbContext.SaveChangesAsync();

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

        var duplicateName = await dbContext.Checklists
            .AsNoTracking()
            .AnyAsync(item => item.Id != id && item.Name == trimmedName);

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
            var duplicateNameAfterSaveFailure = await dbContext.Checklists
                .AsNoTracking()
                .AnyAsync(item => item.Id != id && item.Name == trimmedName);

            if (duplicateNameAfterSaveFailure)
            {
                return Conflict(new { message = "A checklist with this name already exists." });
            }

            throw;
        }
        return Ok(checklist);
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
