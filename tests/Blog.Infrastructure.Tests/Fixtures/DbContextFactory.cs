using Blog.Infrastructure.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Tests.Fixtures;

public class DbContextFactory
{
    /*public static BlogDbContext CreateInMemory()
    {
        var options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new BlogDbContext(options);
    }*/

/*    public static BlogDbContext CreateSqlServerInMemory()
    {
        var databaseName = $"BlogTestDb_{Guid.NewGuid()}";

        var connectionString =
            $"Server=(localdb)\\mssqllocaldb;"+
            $"Database={databaseName};"+
            $"Trusted_Connection=True;"+
            "MultipleActiveResultSets=true";

        var options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        var context = new BlogDbContext(options);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        return context;
    }*/

    public static BlogDbContext CreateSqliteInMemory()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new BlogDbContext(options);
        context.Database.EnsureCreated();

        return context;
    }

}
