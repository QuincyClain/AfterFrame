using AfterFrame.Domain.Library;

namespace AfterFrame.Application.Library;

public interface ILibraryEntryRepository
{
    Task<bool> ExistsAsync(Guid userId, Guid titleId, CancellationToken cancellationToken = default);

    void Add(LibraryEntry entry);
}