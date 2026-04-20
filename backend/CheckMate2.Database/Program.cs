using CheckMate2.Database;

if (args.Length == 0)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Usage: CheckMate2.Database <connection-string>");
    Console.ResetColor();
    return -1;
}

DbUpRunner.Run(args[0]);
return 0;
