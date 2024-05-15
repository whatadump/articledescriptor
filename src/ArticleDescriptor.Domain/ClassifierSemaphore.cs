namespace ArticleDescriptor.Domain;

internal static class ClassifierSemaphore
{
    public static SemaphoreSlim Semaphore = new(0, 1);
}