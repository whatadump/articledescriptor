namespace ArticleDescriptor.Infrastructure.Interfaces;

using Entities;

public interface IOneTimeService
{
    public Task<string?> SaveOneTimeEntry(string htmlText, ApplicationUser? user);
}