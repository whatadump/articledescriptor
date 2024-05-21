namespace ArticleDescriptor.Infrastructure.Models;

using System.Text.Json.Serialization;

public class ClassifyRequestModel
{
    [JsonPropertyName("text")]
    public string Text { get; init; }
}