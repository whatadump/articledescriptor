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

    private long CurrentClassifyingEntryId = -1;
    public ConcurrentQueue<long> GetCurrentQueue() => _feedEntryIds;
    public void SetClassifyingId(long id) => CurrentClassifyingEntryId = id;

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
        if (entry.IsError)
        {
            return ClassificationStatus.ClassificationError;
        }
        
        if (entry.ClassificationCompleted)
        {
            return ClassificationStatus.ClassificationCompleted;
        }

        return ClassificationStatus.Idle;
    }
}