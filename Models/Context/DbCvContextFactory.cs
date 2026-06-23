using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MySql.EntityFrameworkCore.Extensions;

namespace MvcCv.Models.Context;

public sealed class DbCvContextFactory : IDesignTimeDbContextFactory<DbCvContext>
{
    public DbCvContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "server=localhost;port=3306;database=erayguler_cv;user=root;password=;";

        var optionsBuilder = new DbContextOptionsBuilder<DbCvContext>();
        optionsBuilder.UseMySQL(connectionString);

        return new DbCvContext(optionsBuilder.Options);
    }
}
