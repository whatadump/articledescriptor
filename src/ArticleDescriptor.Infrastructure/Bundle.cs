namespace ArticleDescriptor.Infrastructure
{
    using Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Options;

    public static class Bundle
    {
        internal static IServiceCollection UseInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options
                .UseLazyLoadingProxies()
                .UseNpgsql(configuration.GetConnectionString("DefaultConnection"), 
                    npgsql => npgsql.MigrationsAssembly("ArticleDescriptor.Migrations")));
            
            services.Configure<ClassifierOptions>(e =>
            {
                e.Endpoint = configuration["ClassifierOptions:Endpoint"];
            });
            return services;
        }
    }
}