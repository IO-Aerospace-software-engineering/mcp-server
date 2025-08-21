using IO.Astrodynamics;
using IO.MCP.AI;
using IO.MCP.AI.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;


namespace Server.Stdio;

internal abstract class Program
{
    public static async Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose() // Capture all log levels
            .WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "IO-MCP-Server_.log"),
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.Debug()
            .WriteTo.Console(standardErrorFromLevel: Serilog.Events.LogEventLevel.Verbose)
            .CreateLogger();
        try
        {
            Log.Information("Starting IO-Aerospace MCP server...");

            var builder = Host.CreateApplicationBuilder(args);
            
            // Obtenir le répertoire de l'exécutable
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Log.Information("Application base directory: {BaseDirectory}", baseDirectory);
            
            // Ajouter la configuration des fichiers appsettings.json avec chemins absolus
            builder.Configuration
                .AddJsonFile(Path.Combine(baseDirectory, "appsettings.json"), optional: false, reloadOnChange: true)
                .AddJsonFile(Path.Combine(baseDirectory, $"appsettings.{builder.Environment.EnvironmentName}.json"), optional: true, reloadOnChange: true);
            
            builder.Services.AddSerilog();
            builder.Services.AddMcpServer()
                .WithStdioServerTransport().WithToolsFromAssembly(typeof(CelestialBodyTools).Assembly);
            var app = builder.Build();

            Log.Information("IO-Aerospace MCP server started successfully");
            var kernelsPath = builder.Configuration["KernelsPath"];
            if (string.IsNullOrEmpty(kernelsPath))
            {
                Log.Error("KernelsPath is not set in the configuration. Please set it to the directory containing the kernels.");
                return 1;
            }
            
            // Construire le chemin absolu vers les kernels
            var absoluteKernelsPath = Path.IsPathRooted(kernelsPath) 
                ? kernelsPath 
                : Path.Combine(baseDirectory, kernelsPath);
            
            Log.Information("Trying to load kernels from {KernelsPath} (absolute: {AbsoluteKernelsPath})", kernelsPath, absoluteKernelsPath);
            
            if (!Directory.Exists(absoluteKernelsPath))
            {
                Log.Error("Kernels directory does not exist: {AbsoluteKernelsPath}", absoluteKernelsPath);
                return 1;
            }
            
            API.Instance.LoadKernels(new DirectoryInfo(absoluteKernelsPath));
            Log.Information("Kernels loaded successfully from {AbsoluteKernelsPath}", absoluteKernelsPath);
            
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "IO-Aerospace MCP server terminated unexpectedly");
            return 1;
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}