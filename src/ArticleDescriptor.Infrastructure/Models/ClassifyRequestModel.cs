namespace ArticleDescriptor.Infrastructure.Models;

using System.Text.Json.Serialization;
using Enums;

public class ClassifyRequestModel
{
    [JsonPropertyName("text")]
    public string Text { get; init; }
}