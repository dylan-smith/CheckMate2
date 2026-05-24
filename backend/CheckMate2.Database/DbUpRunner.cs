using Microsoft.Data.SqlClient;

namespace CheckMate2.Database;

public static class DbUpRunner
{
    private const int TimeoutSeconds = 120;

    public static void Run(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString)
        {
            ConnectTimeout = TimeoutSeconds,
        };
        var extendedConnectionString = builder.ConnectionString;

        DbUp.EnsureDatabase.For.SqlDatabase(extendedConnectionString);

        var upgrader = DbUp.DeployChanges.To
            .SqlDatabase(extendedConnectionString)
            .WithScriptsEmbeddedInAssembly(typeof(DbUpRunner).Assembly)
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            throw result.Error;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Database migration completed successfully.");
        Console.ResetColor();
    }
}
