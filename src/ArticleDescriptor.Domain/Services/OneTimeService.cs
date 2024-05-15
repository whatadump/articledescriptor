namespace ArticleDescriptor.Domain.Services;

using System.Text.RegularExpressions;
using Ganss.Xss;
using Infrastructure;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

public partial class OneTimeService : IOneTimeService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<FeedService> _logger;
    private readonly IHtmlSanitizer _sanitizer;
    
    private const int DefaultTextWordsCount = 200;

    public OneTimeService(ApplicationDbContext context, ILogger<FeedService> logger, IHtmlSanitizer sanitizer)
    {
        _context = context;
        _logger = logger;
        _sanitizer = sanitizer;
    }

    public async Task<string?> SaveOneTimeEntry(string htmlText, ApplicationUser? user)
    {
        if (string.IsNullOrWhiteSpace(htmlText) || string.IsNullOrEmpty(htmlText))
        {
            return "Ошибка: передана пустая строка";
        }
        
        var sanitizedText = _sanitizer.Sanitize(htmlText);
            
        var tokens = StringFilteringRegex()
            .Replace(sanitizedText, string.Empty)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Take(DefaultTextWordsCount);

        var filteredText = string.Join(" ", tokens);

        if (string.IsNullOrEmpty(filteredText))
        {
            return "Ошибка: после очистки текста ничего не осталось";
        }

        var entry = new OneTimeClassificationEntry
        {
            User = user,
            Text = htmlText,
            NormalizedText = filteredText
        };
        await _context.OneTimeEntries.AddAsync(entry);
        await _context.SaveChangesAsync();
        return null;
    }
    
    [GeneratedRegex(@"[^\w ]", RegexOptions.Compiled)]
    private static partial Regex StringFilteringRegex();
}