using CheckMate2.Api.Contracts;
using CheckMate2.Api.Controllers;
using CheckMate2.Api.Data;
using CheckMate2.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheckMate2.Api.Tests;

public class ChecklistsControllerTests
{
    [Fact]
    public async Task Create_ReturnsConflict_WhenNameAlreadyExists()
    {
        await using var dbContext = CreateDbContext();
        dbContext.Checklists.Add(new Checklist { Name = "Daily" });
        await dbContext.SaveChangesAsync();

        var controller = new ChecklistsController(dbContext);

        var result = await controller.Create(new ChecklistRequest { Name = "Daily" });

        Assert.IsType<ConflictObjectResult>(result.Result);
    }

    [Fact]
    public async Task Create_TrimsName_AndPersistsChecklist()
    {
        await using var dbContext = CreateDbContext();
        var controller = new ChecklistsController(dbContext);

        var result = await controller.Create(new ChecklistRequest { Name = "  Weekly  " });

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var checklist = Assert.IsType<Checklist>(createdResult.Value);
        Assert.Equal("Weekly", checklist.Name);

        var savedChecklist = await dbContext.Checklists.SingleAsync();
        Assert.Equal("Weekly", savedChecklist.Name);
    }

    [Fact]
    public async Task Update_ReturnsConflict_WhenAnotherChecklistUsesName()
    {
        await using var dbContext = CreateDbContext();
        dbContext.Checklists.AddRange(
            new Checklist { Name = "Morning" },
            new Checklist { Name = "Evening" });
        await dbContext.SaveChangesAsync();

        var morning = await dbContext.Checklists.SingleAsync(item => item.Name == "Morning");
        var controller = new ChecklistsController(dbContext);

        var result = await controller.Update(morning.Id, new ChecklistRequest { Name = "Evening" });

        Assert.IsType<ConflictObjectResult>(result.Result);
    }

    private static ChecklistDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ChecklistDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ChecklistDbContext(options);
    }
}
