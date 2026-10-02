using AfterFrame.Application.Common.Persistence;
using AfterFrame.Application.Library;
using AfterFrame.Application.Titles;
using AfterFrame.Application.Users;
using AfterFrame.Infrastructure.Persistence;
using AfterFrame.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AfterFrame.Application.Common.Identity;
using AfterFrame.Infrastructure.Identity;

namespace AfterFrame.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database")
                               ?? throw new InvalidOperationException("Connection string 'Database' was not found.");

        services.AddDbContext<AfterFrameDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<AfterFrameDbContext>());
        
        services.AddSingleton<ICurrentUser>(_ => new DevelopmentCurrentUser());

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITitleRepository, TitleRepository>();
        services.AddScoped<ILibraryEntryRepository, LibraryEntryRepository>();

        return services;
    }
}