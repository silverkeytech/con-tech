using ConTech.Migration;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using System.CommandLine.Invocation;
using ConTech.Migration._100;

namespace ConTech.Migration;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        var upOption = new Option<bool>("--up", description: "Migrate Up", getDefaultValue: () => false);
        var downOption = new Option<long>("--down", description: "Rollback database to a version", getDefaultValue: () => -1);
        var snapOption = new Option<short>("--snap", description: "Set Snapshot", getDefaultValue: () => -1);

        var rootCommand = new RootCommand("TMD Fluent Migrator Runner")
        {
            upOption,
            downOption,
            snapOption
        };

        rootCommand.Handler = CommandHandler.Create<bool, long, short>((up, down, snap) =>
        {
            var serviceProvider = CreateServices();

            using (var scope = serviceProvider.CreateScope())
            {
                if (up)
                    UpdateDatabase(scope.ServiceProvider);

                if (down > -1)
                    RollbackDatabase(scope.ServiceProvider, down);

                if (snap > -1)
                    SwitchSnap(scope.ServiceProvider, snap);
            }
        });

        return await rootCommand.InvokeAsync(args);
    }

    /// <summary>
    /// Configure the dependency injection services
    /// </sumamry>
    private static IServiceProvider CreateServices()
    {
        var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
        if (!String.IsNullOrEmpty(env))
        {
            Console.WriteLine($"Running on {env} environment...");
        }

        var config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true)
           .AddJsonFile($"appsettings.{env}.json", true, true)
           .Build();


        var conn = config["connectionString"];

        return new ServiceCollection()
            // Add common FluentMigrator services
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                // Add Postgres support to FluentMigrator
                .AddSqlServer2016()
                // Set the connection string
                .WithGlobalConnectionString(conn)
                // Define the assembly containing the migrations
                .ScanIn(typeof(_0001_UserTable).Assembly).For.Migrations())
            // Enable logging to console in the FluentMigrator way
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            // Build the service provider
            .BuildServiceProvider(false);
    }

    private static void UpdateDatabase(IServiceProvider serviceProvider)
    {
        Console.WriteLine("Going up...");
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }

    private static void RollbackDatabase(IServiceProvider serviceProvider, long rollbackVersion)
    {
        Console.WriteLine("Going down...");
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateDown(rollbackVersion);
    }

    private static void SwitchSnap(IServiceProvider serviceProvider, short snap)
    {
        Console.WriteLine("Set Snapshot...");
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        if (snap == 1)
        {
            runner.Processor.Execute("ALTER DATABASE TMD SET ALLOW_SNAPSHOT_ISOLATION ON;");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Successfully allowed snapshot !!!");
        }
        else if (snap == 0)
        {
            runner.Processor.Execute("ALTER DATABASE Tahmm SET ALLOW_SNAPSHOT_ISOLATION OFF;");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Successfully denied snapshot !!!");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Can't change snapshot. Wrong parameter value !!!");
        }

        Console.ResetColor();
    }
}