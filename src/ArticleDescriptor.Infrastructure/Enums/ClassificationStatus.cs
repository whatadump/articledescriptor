namespace ArticleDescriptor.Infrastructure.Enums;

using Attributes;

public enum ClassificationStatus
{
    [MemberName("Не распознается")]
    Idle = 0,
    
    [MemberName("В очереди")]
    InQueue = 1,
    
    [MemberName("Распознается")]
    IsBeingClassified = 2,
    
    [MemberName("Распознано")]
    ClassificationCompleted = 3
}