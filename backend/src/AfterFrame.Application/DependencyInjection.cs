using AfterFrame.Application.Library.AddTitleToLibrary;
using Microsoft.Extensions.DependencyInjection;
using AfterFrame.Application.Catalog.ImportExternalCatalog;
using AfterFrame.Application.Catalog.GetCatalog;
using AfterFrame.Application.Catalog.GetTitleDetails;

namespace AfterFrame.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddSingleton<TimeProvider>(TimeProvider.System);
        services.AddScoped<AddTitleToLibraryService>();
        services.AddScoped<ImportExternalCatalogService>();
        services.AddScoped<GetCatalogService>();
        services.AddScoped<GetTitleDetailsService>();

        return services;
    }
}