using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Context;

/// <summary>
/// SQL Server is the real database and the only one the migrations target. SQLite exists
/// for the test suite and for running the site without a database server; select it with
/// Database:Provider=Sqlite.
/// </summary>
public static class DatabaseProvider
{
    public const string SqlServer = "SqlServer";
    public const string Sqlite = "Sqlite";

    public static bool IsSqlite(string? provider) =>
        string.Equals(provider, Sqlite, StringComparison.OrdinalIgnoreCase);

    public static DbContextOptionsBuilder Configure(DbContextOptionsBuilder options, string? provider, string? connectionString) =>
        IsSqlite(provider)
            ? options.UseSqlite(connectionString)
            : options.UseSqlServer(connectionString);
}
