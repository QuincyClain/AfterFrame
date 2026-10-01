using AfterFrame.Application.Library.AddTitleToLibrary;
using Microsoft.Extensions.DependencyInjection;

namespace AfterFrame.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddSingleton<TimeProvider>(TimeProvider.System);
        services.AddScoped<AddTitleToLibraryService>();

        return services;
    }
}