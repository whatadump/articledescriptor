namespace ArticleDescriptor.Infrastructure.Entities;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

// Пользователь приложения
public class ApplicationUser : IdentityUser // <-- Вот тут лежат все остальные столбцы
{
    [Column("real_name")]
    [Required]
    [DefaultValue("")]
    public string RealName { get; set; }
}