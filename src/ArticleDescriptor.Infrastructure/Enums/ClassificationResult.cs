namespace ArticleDescriptor.Infrastructure.Enums;

using System.Text.Json.Serialization;
using Attributes;

[JsonConverter(typeof(JsonNumberEnumConverter<ClassificationResult>))]
public enum ClassificationResult
{
    [MemberName("Другое")]
    Other = 0,
    
    [MemberName("Информация/Объяснение")]
    InformationExplanation = 1,
    
    [MemberName("Новость")]
    News = 2,
    
    [MemberName("Инструкция")]
    Instruction = 3,
    
    [MemberName("Мнение/Аргументация")]
    OpinionArgumentation = 4,
    
    [MemberName("Форум")]
    Forum = 5,
    
    [MemberName("Проза")]
    ProseLyrical = 6,
    
    [MemberName("Юридический текст")]
    Legal = 7,
    
    [MemberName("Реклама")]
    Promotion = 8
}