namespace ArticleDescriptor.Domain.HostedServices;

using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
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

    private readonly IServiceProvider _rootProvider;

    private readonly IOptions<ClassifierOptions> _options;

    private const int DefaultDelayTimeMs = 20000;

    public ClassificationHostedService(
        ILogger<ClassificationHostedService> logger, 
        IServiceProvider rootProvider, 
        IOptions<ClassifierOptions> options)
    {
        _logger = logger; // Логгер
        _rootProvider = rootProvider; // Поставщик услуг
        _options = options; // Параметры приложения
        
        _logger.LogWarning($"Данные конфигурации: {_options.Value.Endpoint}");
    }

    private async Task Watch(CancellationToken cancellationToken)
    {
        using var client = new HttpClient();
        using var scope = _rootProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(); // Получаем контекст базы из поставщика нового скоупа (через конструктор встроить нельзя)
        var endpoint = _options.Value.Endpoint;
        if (string.IsNullOrEmpty(endpoint))
        {
            _logger.LogError("Не удалось получить Endpoint из конфигурации");
            return;
        }
        
        _logger.LogInformation("Сервис распознавания начал наблюдение");
        
        while (!cancellationToken.IsCancellationRequested) // Если не запросили остановку приложения
        {
            try
            {
                var unclassifiedEntries = await context.FeedEntries
                    .Where(x => !x.ClassificationCompleted)
                    .Where(x => !x.IsError)
                    .Take(10)
                    .ToArrayAsync(cancellationToken: cancellationToken); // Получаем все неклассифицированные записи (без ошибки)

                if (unclassifiedEntries.Length == 0)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(DefaultDelayTimeMs), cancellationToken); // Если таковых нет, засыпаем на 20 секунд
                    continue;
                }

                foreach (var feedEntry in unclassifiedEntries) // Если есть, то для каждой
                {
                    _logger.LogWarning($"Отправляем запрос на распознавание ID: {feedEntry.Id}");

                    if (string.IsNullOrEmpty(feedEntry.Text))
                    {
                        _logger.LogError($"Пустой текст для EntryID: {feedEntry.Id}. Запись помечена как ошибка");
                        feedEntry.IsError = true; // В базе лежит пустой текст для записи ленты, сохраняем запись как ошибочную
                        context.FeedEntries.Update(feedEntry);
                        await context.SaveChangesAsync(cancellationToken);
                        continue;
                    }
                     
                    var response = await client.PostAsJsonAsync(endpoint, new ClassifyRequestModel // Отправляем запрос на классификатор
                    {
                        Text = feedEntry.Text
                    }, cancellationToken: cancellationToken);
                    
                    _logger.LogInformation("Распознавание завершено");

                    var result =
                        await response.Content.ReadFromJsonAsync<ClassifyResponseModel>(
                            cancellationToken: cancellationToken); // Считываем результат

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
                    feedEntry.ClassificationResult = result.Result; // Сохраняем полученный результат
                    context.FeedEntries.Update(feedEntry);
                    await context.SaveChangesAsync(cancellationToken);
                }
            }
            catch (TaskCanceledException) // Если сервис заставили завершиться
            {
                _logger.LogWarning(message: "Сервис распознавания завершает работу");
            }
            catch (Exception e) // Если упало что-то еще
            {
                _logger.LogError(message: "Ошибка в сервисе распознавания", exception: e);
            }
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(() => Watch(cancellationToken), cancellationToken: cancellationToken); // Запускаем фоновую задачу
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}