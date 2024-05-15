namespace ArticleDescriptor.Infrastructure.Utils;

using Enums;
using Interfaces;

public static class ClassificationUtils
{
    public static ClassificationStatus GetStatus<T>(this T entity) where T : IClassifiable
    {
        if (entity.IsError)
        {
            return ClassificationStatus.ClassificationError;
        }
        
        if (entity.ClassificationCompleted)
        {
            return ClassificationStatus.ClassificationCompleted;
        }

        return ClassificationStatus.Idle;
    }
}