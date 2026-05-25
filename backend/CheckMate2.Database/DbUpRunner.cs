using Microsoft.Data.SqlClient;

namespace CheckMate2.Database;

public static class DbUpRunner
{
    private const int ConnectionTimeoutSeconds = 120;
    private const int MaxRetries = 5;
    private const int RetryDelaySeconds = 15;

    private static readonly int[] TransientSqlErrorNumbers =
    [
        233,    // No process at the other end of the pipe
        4060,   // Cannot open database
        40197,  // The service has encountered an error processing your request
        40501,  // The service is currently busy
        40613,  // Database not currently available
        49918,  // Not enough resources to process request
    ];

    public static void Run(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        builder.ConnectTimeout = Math.Max(builder.ConnectTimeout, ConnectionTimeoutSeconds);
        var extendedConnectionString = builder.ConnectionString;

        RunWithRetry(extendedConnectionString);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Database migration completed successfully.");
        Console.ResetColor();
    }

    private static void RunWithRetry(string connectionString)
    {
        for (var attempt = 1; ; attempt++)
        {
            try
            {
                DbUp.EnsureDatabase.For.SqlDatabase(connectionString);

                var upgrader = DbUp.DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(typeof(DbUpRunner).Assembly)
                    .LogToConsole()
                    .Build();

                var result = upgrader.PerformUpgrade();

                if (!result.Successful)
                {
                    throw result.Error;
                }

                return;
            }
            catch (Exception ex) when (attempt <= MaxRetries && IsTransientException(ex))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[Retry {attempt}/{MaxRetries}] Transient database error: {ex.Message}");
                Console.WriteLine($"Waiting {RetryDelaySeconds} seconds before retrying...");
                Console.ResetColor();
                Thread.Sleep(TimeSpan.FromSeconds(RetryDelaySeconds));
            }
        }
    }

    private static bool IsTransientException(Exception ex)
    {
        var sqlEx = ex as SqlException ?? ex.InnerException as SqlException;

        if (sqlEx is null)
        {
            return false;
        }

        foreach (SqlError error in sqlEx.Errors)
        {
            if (Array.IndexOf(TransientSqlErrorNumbers, error.Number) >= 0)
            {
                return true;
            }
        }

        return false;
    }
}
