using Microsoft.OpenApi.Models;
using Sprint1.Domain.Repositories;
using Sprint1.Infrastructure.Data;
using Sprint1.Infrastructure.Data.UseCase;
using Sprint1.Utils;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

public partial class Program
{
    public static void Main(string[] args)
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "SOSLocaliza.API")
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
            .WriteTo.File(
                path: "logs/soslocaliza-.log",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}",
                retainedFileCountLimit: 30)
            .CreateLogger();

        try
        {
            Log.Information("Starting SOSLocaliza API");

            var builder = WebApplication.CreateBuilder(args);
            
            // Add Serilog
            builder.Host.UseSerilog();
            
            var swaggerConfig = builder
                .Configuration
                .GetSection("Swagger")
                .Get<SwaggerConfig>();

            // Add Health Checks
            builder.Services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>(
                    name: "database",
                    tags: new[] { "db", "oracle" })
                .AddCheck("api", () =>
                    Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("API is running"),
                    tags: new[] { "api" });

            // Add OpenTelemetry
            builder.Services.AddOpenTelemetry()
                .ConfigureResource(resource => resource
                    .AddService("SOSLocaliza.API"))
                .WithTracing(tracing => tracing
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = true;
                        options.Filter = (httpContext) =>
                        {
                            // Don't trace health check endpoints
                            return !httpContext.Request.Path.StartsWithSegments("/health");
                        };
                    })
                    .AddEntityFrameworkCoreInstrumentation(options =>
                    {
                        options.SetDbStatementForText = true;
                        options.SetDbStatementForStoredProcedure = true;
                    })
                    .AddConsoleExporter())
                .WithMetrics(metrics => metrics
                    .AddAspNetCoreInstrumentation()
                    .AddConsoleExporter());
            
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(swagger =>
                {
                    swagger.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = swaggerConfig.Title,
                        Version = "v1",
                        Description = swaggerConfig.Description,
                        Contact = swaggerConfig.Contact
                    });
                    
                    swagger.EnableAnnotations();

                    foreach (var server in swaggerConfig.Servers)
                    {
                        swagger.AddServer(new OpenApiServer
                            {
                             Url   = server.Url,
                             Description = server.Name
                            });
                    }
                }
                );

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseOracle(builder.Configuration.GetConnectionString("OracleDb"))
            );
            
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddScoped<IUsuarioUseCase, UsuarioUseCase>();
            builder.Services.AddScoped<TestConnectionUseCase>();
            
            var app = builder.Build();
            
            // Add request logging middleware
            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                    diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                    diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
                };
            });

            // Map Health Check endpoints
            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.MapHealthChecks("/health/ready", new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains("db"),
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.MapHealthChecks("/health/live", new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains("api"),
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(ui =>
                    {
                        ui.SwaggerEndpoint("/swagger/v1/swagger.json", "Sprint1.API v1");
                        ui.RoutePrefix = string.Empty;
                    }
                );
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseRouting();
            app.MapControllers();
            
            Log.Information("SOSLocaliza API started successfully");
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
 