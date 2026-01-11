using Blog.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public static BlogDbContext CreateSqlServerInMemory()
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
    }
}
