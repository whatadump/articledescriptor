namespace ArticleDescriptor.Infrastructure.Interfaces;

using System.Collections.Concurrent;
using Entities;
using Enums;

public interface IClassificationService
{
    public bool InQueue(long feedEntryId);

    public void AddToQueue(long feedEntryId);

    public bool IsBeingClassified(long feedEntryId);

    public ClassificationStatus GetClassificationStatus(FeedEntry entry);

    internal ConcurrentQueue<long> GetCurrentQueue();

    internal void SetClassifyingId(long id);
}