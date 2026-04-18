using CheckMate2.Api.Data;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
bool useInMemoryDatabase = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");

builder.Services.AddControllers();
builder.Services.AddOpenApi();
string[] allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? [];

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    {
        if (allowedOrigins.Length == 0)
        {
            return;
        }

        _ = policy.WithOrigins(allowedOrigins)
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
    builder.Services.AddDbContext<ChecklistDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("CheckMate2")));
}

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using (IServiceScope scope = app.Services.CreateScope())
{
    ChecklistDbContext dbContext = scope.ServiceProvider.GetRequiredService<ChecklistDbContext>();
    dbContext.Database.EnsureCreated();
}

app.UseCors();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
