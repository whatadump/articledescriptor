namespace ArticleDescriptor.Application;

using Domain;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class Bundle
{
    public static IServiceCollection UseBusinessApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.UseInfrastructureServices(configuration);
        services.UseDomainServices(configuration);

        return services;
    }
    
}