using CheckMate2.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

builder.Services.AddDbContext<ChecklistDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CheckMate2")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ChecklistDbContext>();
    dbContext.Database.EnsureCreated();
}

app.UseCors();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
