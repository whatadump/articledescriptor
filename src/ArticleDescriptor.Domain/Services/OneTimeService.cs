namespace ArticleDescriptor.Domain.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ganss.Xss;
using Infrastructure;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
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
        
        var sanitizedText = _sanitizer.Sanitize(htmlText); // Очищаем html текст
            
        var tokens = StringFilteringRegex()
            .Replace(sanitizedText, string.Empty)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Take(DefaultTextWordsCount); // Разделяем на слова

        var filteredText = string.Join(" ", tokens); // Собираем строку из слов

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
        await _context.OneTimeEntries.AddAsync(entry); // Сохраняем запись
        await _context.SaveChangesAsync();
        return null;
    }

    Task<IReadOnlyCollection<OneTimeClassificationEntry>> IOneTimeService.GetAllEntries()
    {
        return GetAllEntries();
    }

    public async Task<OneTimeClassificationEntry?> GetEntryById(long? id)
    {
        if (id is null)
        {
            return null;
        }

        return await _context.OneTimeEntries.SingleOrDefaultAsync(x => x.Id == id);
    }

    Task<string?> IOneTimeService.SaveOneTimeEntry(string htmlText, ApplicationUser? user)
    {
        return SaveOneTimeEntry(htmlText, user);
    }

    public async Task<IReadOnlyCollection<OneTimeClassificationEntry>> GetAllEntries()
    {
        return await _context.OneTimeEntries.ToArrayAsync();
    }

    [GeneratedRegex(@"[^\w ]", RegexOptions.Compiled)]
    private static partial Regex StringFilteringRegex();
}