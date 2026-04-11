using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sprint1.Infrastructure.Data;

namespace Sprint1.IntegrationTests.Fixtures;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName;
    private readonly Action<ApplicationDbContext>? _seedAction;

    public CustomWebApplicationFactory(string? databaseName = null, Action<ApplicationDbContext>? seedAction = null)
    {
        _databaseName = databaseName ?? Guid.NewGuid().ToString();
        _seedAction = seedAction;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing DbContext registration
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));

            // Add DbContext using in-memory database for testing
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase(_databaseName);
            });

            // Build the service provider
            var sp = services.BuildServiceProvider();

            // Seed the database if action is provided
            if (_seedAction != null)
            {
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureCreated();
                _seedAction(db);
                db.SaveChanges();
            }
        });

        builder.UseEnvironment("Testing");
    }
}

// Made with Bob
