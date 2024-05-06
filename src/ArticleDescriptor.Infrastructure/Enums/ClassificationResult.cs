namespace ArticleDescriptor.Infrastructure.Enums;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonNumberEnumConverter<ClassificationResult>))]
public enum ClassificationResult
{
    Other = 0,
    InformationExplanation = 1,
    News = 2,
    Instruction = 3,
    OpinionArgumentation = 4,
    Forum = 5,
    ProseLyrical = 6,
    Legal = 7,
    Promotion = 8
}