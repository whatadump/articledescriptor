namespace ArticleDescriptor.Domain.Services;

using System.Collections.Concurrent;
using Infrastructure;
using Infrastructure.Entities;
using Infrastructure.Enums;
using Infrastructure.Interfaces;

public class ClassificationService : IClassificationService
{
    // ReSharper disable once MemberCanBePrivate.Global
    internal readonly ConcurrentQueue<long> _feedEntryIds = new();

    internal long CurrentClassifyingEntryId = -1;

    public bool InQueue(long feedEntryId) => _feedEntryIds.Contains(feedEntryId);

    public void AddToQueue(long feedEntryId)
    {
        if (!_feedEntryIds.Contains(feedEntryId))
        {
            _feedEntryIds.Enqueue(feedEntryId);
        }
    }

    public bool IsBeingClassified(long feedEntryId) => CurrentClassifyingEntryId == feedEntryId;
    
    public ClassificationStatus GetClassificationStatus(FeedEntry entry)
    {
        if (entry.ClassificationCompleted)
        {
            return ClassificationStatus.ClassificationCompleted;
        }
        
        if (CurrentClassifyingEntryId == entry.Id)
        {
            return ClassificationStatus.IsBeingClassified;
        }

        if (_feedEntryIds.Contains(entry.Id))
        {
            return ClassificationStatus.InQueue;
        }

        return ClassificationStatus.Idle;
    }
}