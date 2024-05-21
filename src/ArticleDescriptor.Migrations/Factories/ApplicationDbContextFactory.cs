namespace ArticleDescriptor.Migrations.Factories
{
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Фабрика контекста. Нужна для того, чтобы можно было создавать контекст бд и накатывать миграции в режиме dotnet ef
    /// </summary>
    internal class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseNpgsql(config.GetConnectionString("DefaultConnection"));

            return new ApplicationDbContext(builder.Options);
        }
    }
}