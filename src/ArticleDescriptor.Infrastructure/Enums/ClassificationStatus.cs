namespace ArticleDescriptor.Infrastructure.Enums;

using Attributes;

public enum ClassificationStatus
{
    [MemberName("В очереди")]
    Idle = 0,
    
    [MemberName("Распознано")]
    ClassificationCompleted = 2,
    
    [MemberName("Ошибка")]
    ClassificationError = 3
}