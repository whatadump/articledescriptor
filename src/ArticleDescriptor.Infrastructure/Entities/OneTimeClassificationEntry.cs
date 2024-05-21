namespace ArticleDescriptor.Infrastructure.Entities;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Enums;
using Interfaces;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Простой текст пользователя
/// </summary>
[PrimaryKey(nameof(Id))]
[Table("onetime_classification_entries")]
public class OneTimeClassificationEntry : IClassifiable
{
    [Column("id")]
    [Required]
    public long Id { get; set; }
    
    [Column("user_id")]
    public virtual ApplicationUser? User { get; set; }
    
    [Column("text")]
    [Required]
    public string Text { get; set; }
    
    [Column("normalized_text")]
    [Required]
    public string NormalizedText { get; set; }
    
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