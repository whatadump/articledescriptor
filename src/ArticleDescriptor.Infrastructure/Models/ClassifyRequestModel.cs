namespace ArticleDescriptor.Infrastructure.Models;

using System.Text.Json.Serialization;
using Enums;

public class ClassifyRequestModel
{
    [JsonPropertyName("label")]
    public string Label { get; set; }
    
    [JsonPropertyName("result")]
    public ClassificationResult Result { get; set; }
}