using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sprint1.Infrastructure.Data;

namespace Sprint1.IntegrationTests.Fixtures;

public class WebApplicationFactoryFixture : WebApplicationFactory<Program>
{
    private string _databaseName = Guid.NewGuid().ToString();

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
        });

        builder.UseEnvironment("Testing");
    }

    public HttpClient CreateClientWithDatabase(Action<ApplicationDbContext> seedAction)
    {
        // Generate a unique database name for this test
        _databaseName = Guid.NewGuid().ToString();
        
        // Create a new client which will use the new database name
        var client = CreateClient();
        
        using (var scope = Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.EnsureCreated();
            seedAction(db);
            db.SaveChanges();
        }

        return client;
    }
}

// Made with Bob
