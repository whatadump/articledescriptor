namespace ArticleDescriptor.Domain.HostedServices;

using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class MigratorHostedService(IServiceProvider rootProvider, ILogger<MigratorHostedService> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = rootProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        logger.LogWarning("Начинаем накатывать миграции");
        
        await context.Database.MigrateAsync(cancellationToken: cancellationToken);
        
        logger.LogInformation("Накатка миграций завершена");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}