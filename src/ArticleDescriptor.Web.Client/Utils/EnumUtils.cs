namespace ArticleDescriptor.Web.Client.Utils;

using Infrastructure.Enums;
using MudBlazor;

public static class EnumUtils
{
    /// <summary>
    /// Преобразует статус классификации в соответствующий цвет
    /// </summary>
    public static Color ToColor(this ClassificationStatus? status)
    {
        return status switch
        {
            ClassificationStatus.ClassificationCompleted => Color.Success,
            ClassificationStatus.ClassificationError => Color.Error,
            _ => Color.Info,
        };
    }
}