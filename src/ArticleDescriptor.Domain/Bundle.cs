namespace ArticleDescriptor.Domain
{
    using System.Collections.Immutable;
    using AngleSharp.Css.Dom;
    using Ganss.Xss;
    using HostedServices;
    using Infrastructure.Interfaces;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Services;

    public static class Bundle
    {
        public static IServiceCollection UseDomainServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IFeedService, FeedService>();
            services.AddSingleton<IClassificationService, ClassificationService>(_ => new ClassificationService());
            services.AddSingleton<IHtmlSanitizer, HtmlSanitizer>(_ => new HtmlSanitizer(new HtmlSanitizerOptions()
            {
                AllowedAttributes = ImmutableHashSet<string>.Empty,
                AllowedTags = ImmutableHashSet<string>.Empty,
                AllowedSchemes = ImmutableHashSet<string>.Empty,
                AllowedAtRules = ImmutableHashSet<CssRuleType>.Empty,
                AllowedCssClasses = ImmutableHashSet<string>.Empty,
                AllowedCssProperties = ImmutableHashSet<string>.Empty,
            }));

            services.AddHostedService<MigratorHostedService>();
            services.AddHostedService<ClassificationHostedService>();
            
            return services;
        } 
    }
}