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
using Sprint1.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Sprint1.Application.Services;
using Sprint1.Application.UseCase;
using MongoDB.Driver;
using Sprint1.Infrastructure.Repositories;

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

                    // Configurar autenticação JWT no Swagger
                    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Insira o token JWT no formato: Bearer {seu token}"
                    });

                    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                        }
                    });

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

            // Configure MongoDB
            builder.Services.AddSingleton<IMongoClient>(sp =>
            {
                var mongoSettings = builder.Configuration.GetSection("MongoDB");
                var connectionString = mongoSettings["ConnectionString"] ?? "mongodb://localhost:27017";
                return new MongoClient(connectionString);
            });

            builder.Services.AddScoped<IMongoDatabase>(sp =>
            {
                var mongoSettings = builder.Configuration.GetSection("MongoDB");
                var databaseName = mongoSettings["DatabaseName"] ?? "SOSLocalizaDB";
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(databaseName);
            });
            
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddScoped<IUsuarioUseCase, UsuarioUseCase>();
            builder.Services.AddScoped<TestConnectionUseCase>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<ILoginAuditRepository, LoginAuditRepository>();
            builder.Services.AddScoped<IAuthUseCase, AuthUseCase>();
            builder.Services.AddHttpContextAccessor();

            // Configure JWT Authentication
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
            
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddAuthorization();
            
            var app = builder.Build();
            
            // Add global exception handler middleware
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
            
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
                        // ui.RoutePrefix = string.Empty; // Removed to use default /swagger route
                    }
                );
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
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
 