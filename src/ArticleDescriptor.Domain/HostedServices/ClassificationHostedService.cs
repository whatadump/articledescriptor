namespace ArticleDescriptor.Domain.HostedServices;

using System.Net.Http.Json;
using Infrastructure;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class ClassificationHostedService : IHostedService
{
    private readonly ILogger<ClassificationHostedService> _logger;

    private readonly IClassificationService _classificationService;

    private readonly IServiceProvider _rootProvider;

    private readonly IOptions<ClassifierOptions> _options;

    private const int DefaultDelayTimeMs = 20000;

    public ClassificationHostedService(
        ILogger<ClassificationHostedService> logger, 
        IServiceProvider rootProvider, 
        IClassificationService classificationService, 
        IOptions<ClassifierOptions> options)
    {
        _logger = logger;
        _rootProvider = rootProvider;
        _classificationService = classificationService;
        _options = options;
        
        _logger.LogWarning($"Данные конфигурации: {_options.Value.Endpoint}");
    }

    private async Task Watch(CancellationToken cancellationToken)
    {
        using var client = new HttpClient();
        using var scope = _rootProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var endpoint = _options.Value.Endpoint;
        if (string.IsNullOrEmpty(endpoint))
        {
            _logger.LogError("Не удалось получить Endpoint из конфигурации");
            return;
        }
        
        _logger.LogInformation("Сервис распознавания начал наблюдение");
        
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var unclassifiedEntries = await context.FeedEntries
                    .Where(x => !x.ClassificationCompleted)
                    .Where(x => !x.IsError)
                    .Take(10)
                    .ToArrayAsync(cancellationToken: cancellationToken);

                if (unclassifiedEntries.Length == 0)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(DefaultDelayTimeMs), cancellationToken);
                    continue;
                }

                foreach (var feedEntry in unclassifiedEntries)
                {
                    _logger.LogWarning($"Отправляем запрос на распознавание ID: {feedEntry.Id}");

                    if (string.IsNullOrEmpty(feedEntry.Text))
                    {
                        _logger.LogError($"Пустой текст для EntryID: {feedEntry.Id}. Запись помечена как ошибка");
                        feedEntry.IsError = true;
                        context.FeedEntries.Update(feedEntry);
                        await context.SaveChangesAsync(cancellationToken);
                        continue;
                    }
                    
                    var response = await client.PostAsJsonAsync(endpoint, new ClassifyRequestModel
                    {
                        Text = feedEntry.Text
                    }, cancellationToken: cancellationToken);

                    _logger.LogInformation("Распознавание завершено");

                    var result =
                        await response.Content.ReadFromJsonAsync<ClassifyResponseModel>(
                            cancellationToken: cancellationToken);

                    if (result is null)
                    {
                        _logger.LogError("Произошла ошибка распознавания: ответ пришел null");
                        continue;
                    }

                    if (!string.IsNullOrEmpty(result.Error))
                    {
                        _logger.LogError($"Произошла ошибка распознавания: {result.Error}");
                        continue;
                    }

                    if (result is { Result: null })
                    {
                        _logger.LogError($"Произошла ошибка распознавания: не удалось спарсить результат распознавания");
                        continue;
                    }

                    feedEntry.ClassificationCompleted = true;
                    feedEntry.ClassificationTime = DateTime.UtcNow;
                    feedEntry.ClassificationResult = result.Result;
                    context.FeedEntries.Update(feedEntry);
                    await context.SaveChangesAsync(cancellationToken);
                }
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning(message: "Сервис распознавания завершает работу");
            }
            catch (Exception e)
            {
                _logger.LogError(message: "Ошибка в сервисе распознавания", exception: e);
            }
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(() => Watch(cancellationToken), cancellationToken: cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _classificationService.SetClassifyingId(-1);
        return Task.CompletedTask;
    }
}