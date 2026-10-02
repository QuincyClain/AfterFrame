using AfterFrame.Domain.Titles;

namespace AfterFrame.Application.Titles;

public interface ITitleRepository
{
    Task<bool> ExistsAsync(Guid titleId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Title>> GetByExternalIdsAsync(string source, IReadOnlyCollection<string> externalIds, CancellationToken ct = default);

    void AddRange(IEnumerable<Title> titles);
}