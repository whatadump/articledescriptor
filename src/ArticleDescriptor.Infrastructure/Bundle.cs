namespace ArticleDescriptor.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Options;

    public static class Bundle
    {
        public static IServiceCollection UseInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Подключаем контекст БД
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