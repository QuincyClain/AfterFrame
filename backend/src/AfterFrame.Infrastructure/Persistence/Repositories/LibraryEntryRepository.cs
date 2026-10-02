using AfterFrame.Application.Library;
using AfterFrame.Domain.Library;
using Microsoft.EntityFrameworkCore;

namespace AfterFrame.Infrastructure.Persistence.Repositories;

internal sealed class LibraryEntryRepository(AfterFrameDbContext dbContext) : ILibraryEntryRepository
{
    public Task<bool> ExistsAsync(Guid userId, Guid titleId, CancellationToken cancellationToken = default)
    {
        return dbContext.LibraryEntries.AnyAsync(entry => 
                entry.UserId == userId && entry.TitleId == titleId, cancellationToken);
    }

    public void Add(LibraryEntry entry)
    {
        dbContext.LibraryEntries.Add(entry);
    }
}