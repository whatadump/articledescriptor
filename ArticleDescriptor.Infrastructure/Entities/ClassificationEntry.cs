namespace ArticleDescriptor.Infrastructure.Entities;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

[PrimaryKey(nameof(Id))]
[Table("classification_entries")]
public class ClassificationEntry
{
    [Column("id")]
    [Required]
    public long Id { get; set; }
    
    [Required]
    [Column("classification_source_id")]
    public virtual ClassificationSource ClassificationSource { get; set; }
    
    [Column("source_article_id")]
    [Required]
    public string SourceArticleId { get; set; }
    
    [Column("classification_completed")]
    [DefaultValue(false)]
    public bool ClassificationCompleted { get; set; }
    
    [Column("classification_result")]
    [DefaultValue(null)]
    public int[]? ClassificationResult { get; set; }
    
    [Column("classification_time")]
    [DefaultValue(null)]
    public DateTime? ClassificationTime { get; set; }
    
}