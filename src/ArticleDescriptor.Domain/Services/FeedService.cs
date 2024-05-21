namespace ArticleDescriptor.Domain.Services;

using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeHollow.FeedReader;
using Ganss.Xss;
using Infrastructure;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

/// <summary>
/// Сервис для работы с лентой
/// </summary>
public partial class FeedService : IFeedService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<FeedService> _logger;
    private readonly IHtmlSanitizer _sanitizer;

    private const int DefaultTextWordsCount = 200;
    
    public FeedService(
        ApplicationDbContext context, // Контекст базы (подключение)
        ILogger<FeedService> logger, // Логгер
        IHtmlSanitizer sanitizer) // Очиститель HTML
    {
        _context = context;
        _logger = logger;
        _sanitizer = sanitizer;
    }

    public async Task<IReadOnlyCollection<FeedSource>> GetUserFeeds(ApplicationUser? user)
    {
        if (user is null)
        {
            return [];
        }

        return await _context.FeedSources
            .Where(x => x.User.Id == user.Id) // Достаем все ленты для данного пользователя
            .ToArrayAsync();
    }
    

    public async Task<bool> AddFeed(ApplicationUser? user, string name, string path)
    {
        if (user is null)
        {
            return false;
        }

        if (!await IsFeedUrlValid(path))
        {
            return false;
        }

        var feed = await FeedReader.ReadAsync(path);

        await _context.FeedSources.AddAsync(new FeedSource()
        {
            User = user,
            Name = name,
            RssSource = path,
            PublicLink = feed.Link
        });

        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> IsFeedUrlValid(string path)
    {
        try
        {
            var feed = await FeedReader.ReadAsync(path); // Читаем из ленты
            var items = feed.Items.Count;
            _logger.LogInformation($"Удалось распознать {items} элементов фида");
            return true; // Если дошло до сюда, лента валидна
        }
        catch (Exception)
        {
            return false; // Если упало исключение - лента не валидна
        }
    }

    public async Task<ICollection<FeedEntry>> GetExistingFeedEntries(FeedSource? source)
    {
        if (source is null)
        {
            return [];
        }

        return await _context.FeedEntries
            .Where(x => x.FeedSource.Id == source.Id) // Достаем из базы все записи конкретной ленты
            .ToListAsync();
    }

    public async Task<ICollection<FeedEntry>> LoadEntriesForFeed(FeedSource? source, int count = 3)
    {
        if (source is null)
        {
            return [];
        }
        
        var existingLinks = (await GetExistingFeedEntries(source)) // Получаем все записи для выбранной ленты, помещаем в "замороженную" кучу
            .Select(x => x.SourceArticleId)
            .ToFrozenSet();
        
        var feed = await FeedReader.ReadAsync(source.RssSource); // Загружаем все записи ленты
        _logger.LogInformation($"Получены {feed.Items.Count} записей");

        var addableFeedItems = feed.Items
            .Where(item => !existingLinks.Contains(item.Link)) // Выбираем те из них которые еще не обработаны
            .Where(item => !string.IsNullOrEmpty(item.Description ?? item.Content)) // И не содержат пустой контент
            .Take(count); // Выбираем указанное количество (3)

        var newEntries = new List<FeedEntry>(count);

        foreach (var item in addableFeedItems)
        {
            var newFeedEntry = new FeedEntry
            {
                SourceArticleId = item.Link,
                Title = item.Title,
                FeedSource = source
            };
            
            var sanitizedText = // Тут будет лежать чистый текст
                _sanitizer
                    .Sanitize(item.Description ?? item.Content);
            
            var tokens = StringFilteringRegex()
                    .Replace(sanitizedText, string.Empty)
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Take(DefaultTextWordsCount); // Разделяем на слова 

            newFeedEntry.Text = string.Join(" ", tokens); // и собираем обратно

            await _context.FeedEntries.AddAsync(newFeedEntry); // Сохраняем запись ленты
            newEntries.Add(newFeedEntry);
        }
        
        await _context.SaveChangesAsync();

        return newEntries;
    }

    public async Task<bool> DeleteEntry(FeedEntry? entry)
    {
        if (entry is null)
        {
            return false;
        }

        _context.FeedEntries.Remove(entry);
        await _context.SaveChangesAsync();

        return true;
    }

    [GeneratedRegex(@"[^\w ]", RegexOptions.Compiled)] // '\w ' означает - буквы и пробел 
    private static partial Regex StringFilteringRegex();
}