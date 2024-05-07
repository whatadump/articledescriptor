namespace ArticleDescriptor.Infrastructure.Entities;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Enums;
using Microsoft.EntityFrameworkCore;

[PrimaryKey(nameof(Id))]
[Table("classification_entries")]
public class FeedEntry
{
    [Column("id")]
    [Required]
    public long Id { get; set; }
    
    [Required]
    [Column("classification_source_id")]
    public virtual FeedSource FeedSource { get; set; }
    
    [Required]
    [Column("title")]
    public string Title { get; set; }
    
    [Column("source_article_id")]
    [Required]
    public string SourceArticleId { get; set; }
    
    [Column("text")]
    [Required]
    public string Text { get; set; }
    
    [Column("classification_completed")]
    [DefaultValue(false)]
    public bool ClassificationCompleted { get; set; }
    
    [Column("classification_result")]
    [DefaultValue(null)]
    public ClassificationResult? ClassificationResult { get; set; }
    
    [Column("classification_time")]
    [DefaultValue(null)]
    public DateTime? ClassificationTime { get; set; }
    
    [Column("is_error")]
    [DefaultValue(false)]
    public bool IsError { get; set; }
}