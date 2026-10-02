using AfterFrame.Application.Titles;
using AfterFrame.Domain.Titles;
using Microsoft.EntityFrameworkCore;

namespace AfterFrame.Infrastructure.Persistence.Repositories;

internal sealed class TitleRepository(AfterFrameDbContext dbContext) : ITitleRepository
{
    public Task<bool> ExistsAsync(Guid titleId, CancellationToken cancellationToken = default)
    {
        return dbContext.Titles.AnyAsync(title => title.Id == titleId, cancellationToken);
    }

    public async Task<IReadOnlyList<Title>> GetByExternalIdsAsync(string source, IReadOnlyCollection<string> externalIds, CancellationToken cancellationToken = default)
    {
        if (externalIds.Count == 0)
        {
            return [];
        }

        var ids = externalIds.ToArray();

        return await dbContext.Titles
            .Include(title => title.Genres)
            .Where(title => title.ExternalSource == source && title.ExternalId != null && ids.Contains(title.ExternalId))
            .ToListAsync(cancellationToken);
    }

    public void AddRange(IEnumerable<Title> titles)
    {
        dbContext.Titles.AddRange(titles);
    }
}