namespace CheckMate2.Database;

public static class DbUpRunner
{
    public static void Run(string connectionString)
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

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Database migration completed successfully.");
        Console.ResetColor();
    }
}
