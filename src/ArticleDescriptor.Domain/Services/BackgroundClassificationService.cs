namespace ArticleDescriptor.Domain.Services;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class BackgroundClassificationService : IHostedService
{
    private readonly ILogger<BackgroundClassificationService> _logger;

    private readonly IServiceProvider _serviceProvider;

    public BackgroundClassificationService(
        ILogger<BackgroundClassificationService> logger, 
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}