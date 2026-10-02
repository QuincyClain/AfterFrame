using AfterFrame.Application.Titles;
using AfterFrame.Domain.Titles;
using Microsoft.EntityFrameworkCore;

namespace AfterFrame.Infrastructure.Persistence.Repositories;

internal sealed class GenreRepository(AfterFrameDbContext dbContext) : IGenreRepository
{
    public async Task<IReadOnlyList<Genre>> GetByNamesAsync(IReadOnlyCollection<string> names, CancellationToken cancellationToken = default)
    {
        if (names.Count == 0)
        {
            return [];
        }

        var requestedNames = names.ToArray();

        return await dbContext.Genres
            .Where(genre => requestedNames.Contains(genre.Name))
            .ToListAsync(cancellationToken);
    }

    public void AddRange(IEnumerable<Genre> genres)
    {
        dbContext.Genres.AddRange(genres);
    }
}