namespace ArticleDescriptor.Domain.Services;

using System.Collections.Frozen;
using System.Text.RegularExpressions;
using CodeHollow.FeedReader;
using Ganss.Xss;
using Infrastructure;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public partial class FeedService : IFeedService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<FeedService> _logger;
    private readonly IClassificationService _classificationService;
    private readonly IHtmlSanitizer _sanitizer;

    private const int DefaultTextWordsCount = 200;
    
    public FeedService(
        ApplicationDbContext context, 
        ILogger<FeedService> logger, 
        IClassificationService classificationService, 
        IHtmlSanitizer sanitizer)
    {
        _context = context;
        _logger = logger;
        _classificationService = classificationService;
        _sanitizer = sanitizer;
    }

    public async Task<IReadOnlyCollection<FeedSource>> GetUserFeeds(ApplicationUser? user)
    {
        if (user is null)
        {
            return [];
        }

        return await _context.FeedSources
            .Where(x => x.User.Id == user.Id)
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
            var feed = await FeedReader.ReadAsync(path);
            var items = feed.Items.Count;
            _logger.LogInformation($"Удалось распознать {items} элементов фида");
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<ICollection<FeedEntry>> GetExistingFeedEntries(FeedSource? source)
    {
        if (source is null)
        {
            return [];
        }

        return await _context.FeedEntries
            .Where(x => x.FeedSource.Id == source.Id)
            .ToListAsync();
    }

    public async Task<ICollection<FeedEntry>> LoadEntriesForFeed(FeedSource? source, int count = 3)
    {
        if (source is null)
        {
            return [];
        }
        
        var existingLinks = (await GetExistingFeedEntries(source))
            .Select(x => x.SourceArticleId)
            .ToFrozenSet();
        
        var feed = await FeedReader.ReadAsync(source.RssSource);
        _logger.LogInformation($"Получены {feed.Items.Count} записей");

        var addableFeedItems = feed.Items
            .Where(item => !existingLinks.Contains(item.Link))
            .Where(item => !string.IsNullOrEmpty(item.Description ?? item.Content))
            .Take(count);

        var newEntries = new List<FeedEntry>(count);

        foreach (var item in addableFeedItems)
        {
            var newFeedEntry = new FeedEntry
            {
                SourceArticleId = item.Link,
                Title = item.Title,
                ClassificationTime = DateTime.UtcNow,
                FeedSource = source
            };
            
            var sanitizedText =
                _sanitizer
                    .Sanitize(item.Description ?? item.Content);
            
            var tokens = StringFilteringRegex()
                    .Replace(sanitizedText, string.Empty)
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Take(DefaultTextWordsCount);

            newFeedEntry.Text = string.Join(" ", tokens);

            await _context.FeedEntries.AddAsync(newFeedEntry);
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

    [GeneratedRegex(@"[^\w ]", RegexOptions.Compiled)]
    private static partial Regex StringFilteringRegex();
}