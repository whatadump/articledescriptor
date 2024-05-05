namespace ArticleDescriptor.Infrastructure.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

[Table("classification_sources")]
[PrimaryKey(nameof(Id))]
public class ClassificationSource
{
    [Column("Id")]
    [Required]
    public long Id { get; set; }
    
    [Column("name")]
    [Required]
    public string Name { get; set; }
    
    [Column("rss_source")]
    [Required]
    public string RssSource { get; set; }
    
    [Column("user_id")]
    [Required]
    public virtual ApplicationUser User { get; set; }
    
    public virtual ICollection<ClassificationEntry> Entries { get; }
}