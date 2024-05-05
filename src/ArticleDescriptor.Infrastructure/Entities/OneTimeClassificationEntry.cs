namespace ArticleDescriptor.Infrastructure.Entities;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

[PrimaryKey(nameof(Id))]
[Table("onetime_classification_entries")]
public class OneTimeClassificationEntry
{
    [Column("id")]
    [Required]
    public long Id { get; set; }
    
    [Column("user_id")]
    [Required]
    public virtual ApplicationUser User { get; set; }
    
    [Column("text")]
    [Required]
    public string Text { get; set; }
    
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