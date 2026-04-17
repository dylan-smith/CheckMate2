using CheckMate2.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CheckMate2.Api.Data;

public class ChecklistDbContext(DbContextOptions<ChecklistDbContext> options) : DbContext(options)
{
    public DbSet<Checklist> Checklists => Set<Checklist>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Checklist>()
            .HasIndex(checklist => checklist.Name)
            .IsUnique();

        modelBuilder.Entity<Checklist>()
            .Property(checklist => checklist.Name)
            .HasMaxLength(200)
            .IsRequired();
    }
}
