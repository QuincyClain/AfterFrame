using System.Net.Http.Headers;
using AfterFrame.Application.Common.ExternalCatalog;
using AfterFrame.Application.Common.Identity;
using AfterFrame.Application.Common.Persistence;
using AfterFrame.Application.Library;
using AfterFrame.Application.Titles;
using AfterFrame.Application.Users;
using AfterFrame.Infrastructure.ExternalCatalog.Tmdb;
using AfterFrame.Infrastructure.Identity;
using AfterFrame.Infrastructure.Persistence;
using AfterFrame.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using AfterFrame.Application.Catalog;
using AfterFrame.Infrastructure.Persistence.Readers;

namespace AfterFrame.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database") ?? throw new InvalidOperationException("Connection string 'Database' was not found.");

        services.AddDbContext<AfterFrameDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<AfterFrameDbContext>());

        services.AddSingleton<ICurrentUser>(_ => new DevelopmentCurrentUser());

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITitleRepository, TitleRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<ILibraryEntryRepository, LibraryEntryRepository>();
        services.AddScoped<ICatalogReader, EfCatalogReader>();

        services.AddOptions<TmdbOptions>().Bind(configuration.GetSection(TmdbOptions.SectionName))
            .Validate(options => Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out _), "Tmdb:BaseUrl must be a valid absolute URL.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.AccessToken), "Tmdb:AccessToken is required.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.Language), "Tmdb:Language is required.")
            .ValidateOnStart();

        services.AddHttpClient<IExternalCatalogClient, TmdbCatalogClient>((provider, client) =>
            {
                var options = provider.GetRequiredService<IOptions<TmdbOptions>>().Value;

                client.BaseAddress = new Uri(options.BaseUrl);
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", options.AccessToken);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

        return services;
    }
}