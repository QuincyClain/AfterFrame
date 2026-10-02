using AfterFrame.Domain.Titles;

namespace AfterFrame.Application.Titles;

public interface IGenreRepository
{
    Task<IReadOnlyList<Genre>> GetByNamesAsync(IReadOnlyCollection<string> names, CancellationToken ct = default);

    void AddRange(IEnumerable<Genre> genres);
}