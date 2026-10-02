using AfterFrame.Application.Catalog.Models;
using AfterFrame.Application.Common.Identity;
using AfterFrame.Application.Common.Models;
using AfterFrame.Domain.Titles;

namespace AfterFrame.Application.Catalog.GetCatalog;

public sealed class GetCatalogService(ICatalogReader catalogReader, ICurrentUser currentUser)
{
    public Task<PagedResult<CatalogTitleSummary>> ExecuteAsync(CatalogQuery query, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        if (query.Page < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(query.Page), "Page must be greater than zero.");
        }

        if (query.PageSize is < 1 or > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(query.PageSize), "Page size must be between 1 and 100.");
        }

        if (query.ReleaseYear is <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(query.ReleaseYear), "Release year must be positive.");
        }

        if (!Enum.IsDefined(typeof(CatalogSort), query.Sort))
        {
            throw new ArgumentOutOfRangeException(nameof(query.Sort), "Catalog sort option is invalid.");
        }

        var allowedTags = TitleTag.Animation | TitleTag.Anime;

        if (query.Tag is { } tag && (tag == TitleTag.None || (tag & ~allowedTags) != 0))
        {
            throw new ArgumentOutOfRangeException(nameof(query.Tag), "Catalog tag is invalid.");
        }

        var normalizedQuery = query with
        {
            Search = Normalize(query.Search),
            Genre = Normalize(query.Genre)
        };

        return catalogReader.GetPageAsync(currentUser.UserId, normalizedQuery, ct);
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}