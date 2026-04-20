using CheckMate2.Api.Data;
using CheckMate2.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var useInMemoryDatabase = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");

builder.Services.AddControllers();
builder.Services.AddOpenApi();
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? [];

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    {
        if (allowedOrigins.Length == 0)
        {
            return;
        }

        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    }));

if (useInMemoryDatabase)
{
    builder.Services.AddDbContext<ChecklistDbContext>(options =>
        options.UseInMemoryDatabase("CheckMate2"));
}
else
{
    var connectionString = builder.Configuration.GetConnectionString("CheckMate2")
        ?? throw new InvalidOperationException("Connection string 'CheckMate2' not found.");

    builder.Services.AddDbContext<ChecklistDbContext>(options =>
        options.UseSqlServer(connectionString));
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (useInMemoryDatabase)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ChecklistDbContext>();
    dbContext.Database.EnsureCreated();
}
else
{
    var connectionString = builder.Configuration.GetConnectionString("CheckMate2")!;
    DbUpRunner.Run(connectionString);
}

app.UseCors();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
