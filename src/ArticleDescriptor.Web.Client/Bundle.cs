namespace ArticleDescriptor.Web.Client;

using Services;

public static class Bundle
{
    public static IServiceCollection UseInteractiveApplication(this IServiceCollection services, IConfigurationRoot config)
    {
        services.AddScoped<UserManager>(); // Тут мы подключаем нужные зависимости для Client приложения
        
        return services;
    }
}