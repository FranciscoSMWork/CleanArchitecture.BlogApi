using Blog.Application.Abstractions.Services;
using Blog.Application.Services;
using Blog.Domain.Interfaces.Repositories;
using Blog.Infrastructure.Context;
using Blog.Infrastructure.Persistence;
using Blog.Infrastructure.Repositories.Implementations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.API.Tests.Fixtures;

//Precisa de:
//Microsoft.AspNetCore.Mvc.Testing
//Microsoft.EntityFrameworkCore.Sqlite

public class ApiTestFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"BlogDb_Test_{Guid.NewGuid()}";
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<BlogDbContext>)
            );

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<BlogDbContext>(options =>
            {
                options.UseSqlServer(
                    $"Data Source=(localdb)\\MSSQLLocalDB;" +
                    $"Initial Catalog={_databaseName};" +
                    $"Integrated Security=True;" +
                    $"Encrypt=False;"
                );
            });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BlogDbContext>();

            db.Database.Migrate();

        });   

        builder.UseEnvironment("Development");
    }
    public override async ValueTask DisposeAsync()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
        await db.Database.EnsureDeletedAsync();
        await base.DisposeAsync();
    }

}
