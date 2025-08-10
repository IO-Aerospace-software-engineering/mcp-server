using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using IO.Astrodynamics;
using IO.MCP.AI;
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
            .WithToolsFromAssembly(typeof(AIServices).Assembly);
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
            
            var kernelsPath = builder.Configuration["KernelsPath"];
            logger.LogInformation("Trying to load kernels from {KernelsPath}", kernelsPath);
            if (string.IsNullOrEmpty(kernelsPath))
            {
                Log.Error("KernelsPath is not set in the configuration. Please set it to the directory containing the kernels.");
                return 1;
            }

            API.Instance.LoadKernels(new DirectoryInfo(kernelsPath!));
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