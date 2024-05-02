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
    
    public ApplicationUser User { get; set; }
    
    public ICollection<ClassificationEntry> Entries { get; }
    
    [Column("user_id")]
    [Required]
    protected internal string UserId { get; set; }
}