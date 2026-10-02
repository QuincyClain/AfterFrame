using AfterFrame.Application.Catalog;
using AfterFrame.Application.Catalog.Models;
using AfterFrame.Application.Common.Models;
using AfterFrame.Domain.Titles;
using Microsoft.EntityFrameworkCore;

namespace AfterFrame.Infrastructure.Persistence.Readers;

internal sealed class EfCatalogReader(AfterFrameDbContext dbContext) : ICatalogReader
{
    public async Task<PagedResult<CatalogTitleSummary>> GetPageAsync(Guid currentUserId, CatalogQuery query, CancellationToken ct = default)
    {
        var titles = GetVisibleTitles(currentUserId);

        if (query.Search is not null)
        {
            var searchPattern = $"%{query.Search}%";

            titles = titles.Where(title => EF.Functions.ILike(title.Name, searchPattern));
        }

        if (query.Type is not null)
        {
            titles = titles.Where(title => title.Type == query.Type);
        }

        if (query.Tag is { } tag)
        {
            titles = titles.Where(title => (title.Tags & tag) == tag);
        }

        if (query.Genre is not null)
        {
            titles = titles.Where(title =>
                title.Genres.Any(genre => EF.Functions.ILike(genre.Name, query.Genre)));
        }

        if (query.ReleaseYear is not null)
        {
            titles = titles.Where(title => title.ReleaseYear == query.ReleaseYear);
        }

        var totalCount = await titles.CountAsync(ct);

        var orderedTitles = query.Sort switch
        {
            CatalogSort.Votes => titles
                .OrderByDescending(title => title.ExternalVoteCount)
                .ThenByDescending(title => title.ExternalRating ?? -1m)
                .ThenBy(title => title.Name),

            CatalogSort.ReleaseYear => titles.OrderByDescending(title => title.ReleaseYear).ThenBy(title => title.Name),

            CatalogSort.Name => titles.OrderBy(title => title.Name),

            _ => titles
                .OrderByDescending(title => title.ExternalRating ?? -1m)
                .ThenByDescending(title => title.ExternalVoteCount)
                .ThenBy(title => title.Name)
        };

        var items = await orderedTitles
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(title => new CatalogTitleSummary(
                title.Id,
                title.Name,
                title.Type,
                title.Tags,
                title.ReleaseYear,
                title.PosterPath,
                title.ExternalRating,
                title.ExternalVoteCount,
                title.PublicationStatus,
                title.Genres.OrderBy(genre => genre.Name).Select(genre => genre.Name).ToArray()))
            .ToArrayAsync(ct);

        return new PagedResult<CatalogTitleSummary>(items, query.Page, query.PageSize, totalCount);
    }

    public Task<CatalogTitleDetails?> GetByIdAsync(Guid titleId, Guid currentUserId, CancellationToken ct = default)
    {
        return GetVisibleTitles(currentUserId)
            .Where(title => title.Id == titleId)
            .Select(title => new CatalogTitleDetails(
                title.Id,
                title.Name,
                title.Type,
                title.Tags,
                title.ReleaseYear,
                title.Description,
                title.PosterPath,
                title.BackdropPath,
                title.ExternalRating,
                title.ExternalVoteCount,
                title.Origin,
                title.PublicationStatus,
                title.ExternalSource,
                title.Genres.OrderBy(genre => genre.Name).Select(genre => genre.Name).ToArray()))
            .SingleOrDefaultAsync(ct);
    }

    private IQueryable<Title> GetVisibleTitles(Guid currentUserId)
    {
        return dbContext.Titles
            .AsNoTracking()
            .Where(title => title.PublicationStatus == TitlePublicationStatus.Published || title.CreatedByUserId == currentUserId);
    }
}