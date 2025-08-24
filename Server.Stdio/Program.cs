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

            // Application base directory (next to the executable)
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Log.Information("Application base directory: {BaseDirectory}", baseDirectory);

            // Only use environment variables for config (no appsettings)
            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddSerilog();
            builder.Services.AddMcpServer()
                .WithStdioServerTransport().WithToolsFromAssembly(typeof(CelestialBodyTools).Assembly);
            var app = builder.Build();

            // Parse CLI for kernels path: --kernels-path <path> | --kernels <path> | -k <path>
            string? kernelsPathArg = null;
            for (int i = 0; i < args.Length; i++)
            {
                var a = args[i];
                if (a == "-k" || a == "--kernels" || a == "--kernels-path")
                {
                    if (i + 1 < args.Length)
                    {
                        kernelsPathArg = args[i + 1];
                        i++;
                    }
                    else
                    {
                        Log.Error("Missing value for {Arg}. Usage: -k <path>", a);
                        return 1;
                    }
                }
            }

            // Priority: CLI arg > IO_DATA_DIR env
            var kernelsPath = kernelsPathArg ?? Environment.GetEnvironmentVariable("IO_DATA_DIR");

            if (string.IsNullOrWhiteSpace(kernelsPath))
            {
                Log.Error("Kernels path not provided. Use -k|--kernels-path <path> or set IO_DATA_DIR environment variable.");
                Log.Information("Example: ./Server.Stdio -k /path/to/your/spice/kernels");
                return 1;
            }

            // Build absolute path; if relative, resolve next to the executable directory
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