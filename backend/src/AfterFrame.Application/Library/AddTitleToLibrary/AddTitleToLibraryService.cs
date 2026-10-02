using AfterFrame.Application.Common.Exceptions;
using AfterFrame.Application.Common.Persistence;
using AfterFrame.Application.Titles;
using AfterFrame.Application.Users;
using AfterFrame.Domain.Library;
using AfterFrame.Application.Common.Identity;

namespace AfterFrame.Application.Library.AddTitleToLibrary;

public sealed class AddTitleToLibraryService(
    ICurrentUser currentUser,
    IUserRepository users,
    ITitleRepository titles,
    ILibraryEntryRepository libraryEntries,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider)
{
    public async Task<LibraryEntry> ExecuteAsync(Guid titleId, CancellationToken cancellationToken = default)
    {
        var userId = currentUser.UserId;

        if (!await users.ExistsAsync(userId, cancellationToken))
        {
            throw new EntityNotFoundException("User", userId);
        }

        if (!await titles.ExistsAsync(titleId, cancellationToken))
        {
            throw new EntityNotFoundException("Title", titleId);
        }

        if (await libraryEntries.ExistsAsync(userId, titleId, cancellationToken))
        {
            throw new ConflictException("The title is already present in the user's library.");
        }

        var entry = LibraryEntry.CreatePlanned(userId, titleId, timeProvider.GetUtcNow());

        libraryEntries.Add(entry);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return entry;
    }
}