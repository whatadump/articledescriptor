namespace ArticleDescriptor.Infrastructure.Interfaces;

using Entities;

public interface IFeedService
{
    public Task<IReadOnlyCollection<FeedSource>> GetUserFeeds(ApplicationUser? user);

    public Task<bool> AddFeed(ApplicationUser? user, string name, string path);

    public Task<bool> IsFeedUrlValid(string path);

    public Task<ICollection<FeedEntry>> GetExistingFeedEntries(FeedSource? source);

    public Task<ICollection<FeedEntry>> LoadEntriesForFeed(FeedSource? source, int count = 3);
}