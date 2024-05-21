namespace ArticleDescriptor.Domain.HostedServices;

using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Models;
using Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

//Сервис аналогичен ClassificationHostedService, смотри там
public class OneTimeClassificationHostedService : IHostedService
{
    private readonly ILogger<OneTimeClassificationHostedService> _logger;

    private readonly IServiceProvider _rootProvider;

    private readonly IOptions<ClassifierOptions> _options;

    private const int DefaultDelayTimeMs = 20000;

    public OneTimeClassificationHostedService(
        ILogger<OneTimeClassificationHostedService> logger, 
        IServiceProvider rootProvider, 
        IOptions<ClassifierOptions> options)
    {
        _logger = logger;
        _rootProvider = rootProvider;
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
                var unclassifiedEntries = await context.OneTimeEntries
                    .Where(x => !x.ClassificationCompleted)
                    .Where(x => !x.IsError)
                    .Take(10)
                    .ToArrayAsync(cancellationToken: cancellationToken);

                if (unclassifiedEntries.Length == 0)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(DefaultDelayTimeMs), cancellationToken);
                    continue;
                }

                foreach (var oneTimeEntry in unclassifiedEntries)
                {
                    _logger.LogWarning($"Отправляем запрос на распознавание ID: {oneTimeEntry.Id}");

                    if (string.IsNullOrEmpty(oneTimeEntry.Text))
                    {
                        _logger.LogError($"Пустой текст для EntryID: {oneTimeEntry.Id}. Запись помечена как ошибка");
                        oneTimeEntry.IsError = true;
                        context.OneTimeEntries.Update(oneTimeEntry);
                        await context.SaveChangesAsync(cancellationToken);
                        continue;
                    }
                    
                    var response = await client.PostAsJsonAsync(endpoint, new ClassifyRequestModel
                    {
                        Text = oneTimeEntry.NormalizedText
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

                    oneTimeEntry.ClassificationCompleted = true;
                    oneTimeEntry.ClassificationTime = DateTime.UtcNow;
                    oneTimeEntry.ClassificationResult = result.Result;
                    context.OneTimeEntries.Update(oneTimeEntry);
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
        return Task.CompletedTask;
    }
}