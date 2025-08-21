using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using IO.Astrodynamics;
using IO.MCP.AI;
using IO.MCP.AI.Tools;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Configuration.Json;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Serilog;

namespace Server.Sse;

public abstract class Program
{
    public static int Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
        var builder = WebApplication.CreateBuilder(args);
        
        // Add environment variables to configuration
        builder.Configuration.AddEnvironmentVariables();
        
        builder.Host.UseSerilog();
        // Configure JSON options for the entire application including System.Text.Json defaults
        builder.Services.ConfigureHttpJsonOptions(options => { options.SerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals; });

        builder.Services.Configure<JsonOptions>(options => { options.SerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals; });

        // Configure System.Text.Json defaults
        builder.Services.PostConfigure<JsonSerializerOptions>(options => { options.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals; });

        builder.Services.AddSingleton<JsonSerializerOptions>(sp =>
        {
            Console.Error.WriteLine("✅ JsonOptions created with NumberHandling=" +
                                    JsonNumberHandling.AllowNamedFloatingPointLiterals);
            return new JsonSerializerOptions { NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals };
        });

        builder.Logging.AddConsole(consoleLogOptions =>
        {
            // Configure all logs to go to stderr
            consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
        });


        builder.Services.AddMcpServer(opt =>
            {
                opt.ServerInfo = new Implementation { Name = "IO Aerospace -- MCP Server", Title = "IO Aerospace - MCP Server", Version = "0.1.0" };
            })
            .WithHttpTransport()
            .WithToolsFromAssembly(typeof(CelestialBodyTools).Assembly);
        builder.Services.AddOpenTelemetry()
            .WithTracing(b => b.AddSource("*")
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation())
            .WithMetrics(b => b.AddMeter("*")
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation())
            .WithLogging()
            .UseOtlpExporter();

        try
        {
            var app = builder.Build();

            if (app.Environment.IsProduction())
            {
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";
                        var error = new { message = "An unexpected error occurred." };
                        await context.Response.WriteAsJsonAsync(error);
                    });
                });
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            app.MapMcp();

            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("IO-Aerospace MCP server starting...");
            
            // Check for environment variable override first, then fall back to configuration
            var kernelsPath = Environment.GetEnvironmentVariable("IO_DATA_DIR") ?? builder.Configuration["KernelsPath"];
            
            logger.LogInformation("Trying to load kernels from {KernelsPath}", kernelsPath);
            if (string.IsNullOrEmpty(kernelsPath))
            {
                Log.Error("KernelsPath is not set. Please set it in appsettings.json or use IO_DATA_DIR environment variable.");
                return 1;
            }
            
            // Construire le chemin absolu vers les kernels
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var absoluteKernelsPath = Path.IsPathRooted(kernelsPath) 
                ? kernelsPath 
                : Path.Combine(baseDirectory, kernelsPath);
            
            if (!Directory.Exists(absoluteKernelsPath))
            {
                logger.LogError("Kernels directory does not exist: {AbsoluteKernelsPath}", absoluteKernelsPath);
                return 1;
            }

            API.Instance.LoadKernels(new DirectoryInfo(absoluteKernelsPath));
            logger.LogInformation("Kernels loaded successfully from {KernelsPath}", kernelsPath);
            logger.LogInformation("IO-Aerospace MCP server started successfully");
            app.Run();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "IO-Aerospace MCP server terminated unexpectedly");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}