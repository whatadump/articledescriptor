namespace ArticleDescriptor.Infrastructure.Models;

using System.Text.Json.Serialization;
using Enums;

public class ClassifyResponseModel
{
    [JsonPropertyName("label")]
    public string? Label { get; set; }
    
    [JsonPropertyName("result")]
    public ClassificationResult? Result { get; set; }
    
    [JsonPropertyName("error")]
    public string? Error { get; set; }
}