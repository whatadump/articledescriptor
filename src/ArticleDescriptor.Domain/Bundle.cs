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
            services.AddHostedService<MigratorHostedService>();
            services.AddHostedService<ClassificationHostedService>();
            services.AddHostedService<OneTimeClassificationHostedService>();
            
            services.AddTransient<IFeedService, FeedService>();
            services.AddTransient<IOneTimeService, OneTimeService>();
            
            services.AddSingleton<IHtmlSanitizer, HtmlSanitizer>(_ =>
            {
                var sanitizer = new HtmlSanitizer(new HtmlSanitizerOptions()
                {
                    AllowedAttributes = ImmutableHashSet<string>.Empty,
                    AllowedTags = ImmutableHashSet<string>.Empty,
                    AllowedSchemes = ImmutableHashSet<string>.Empty,
                    AllowedAtRules = ImmutableHashSet<CssRuleType>.Empty,
                    AllowedCssClasses = ImmutableHashSet<string>.Empty,
                    AllowedCssProperties = ImmutableHashSet<string>.Empty,
                })
                {
                    KeepChildNodes = true,
                };

                return sanitizer;
            });


            
            return services;
        } 
    }
}