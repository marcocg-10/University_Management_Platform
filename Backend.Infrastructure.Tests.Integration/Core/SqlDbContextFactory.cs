using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Integration.Core;

internal static class SqliteDbContextFactory
{
    public static (AppDbContext Context, SqliteConnection Connection) Create()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .EnableSensitiveDataLogging()
            .Options;

        var ctx = new AppDbContext(options);
        ctx.Database.EnsureCreated();
        return (ctx, connection);
    }
}