namespace ArticleDescriptor.Infrastructure.Interfaces;

using Enums;

public interface IClassifiable
{
    public bool ClassificationCompleted { get; set; }

    public ClassificationResult? ClassificationResult { get; set; }
    
    public DateTime? ClassificationTime { get; set; }
    
    public bool IsError { get; set; }
}