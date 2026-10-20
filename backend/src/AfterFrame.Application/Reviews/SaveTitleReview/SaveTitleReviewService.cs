using AfterFrame.Application.Catalog;
using AfterFrame.Application.Common.Exceptions;
using AfterFrame.Application.Common.Identity;
using AfterFrame.Application.Common.Persistence;
using AfterFrame.Application.Library;
using AfterFrame.Application.Users;
using AfterFrame.Domain.Library;

namespace AfterFrame.Application.Reviews.SaveTitleReview;

public sealed class SaveTitleReviewService(ICurrentUser currentUser, IUserRepository users, ICatalogReader catalogReader,
    ILibraryEntryRepository libraryEntries, IUnitOfWork unitOfWork, TimeProvider timeProvider)
{
    public async Task ExecuteAsync(Guid titleId, SaveTitleReviewCommand command, CancellationToken ct = default)
    {
        if (titleId == Guid.Empty)
        {
            throw new ArgumentException("Title identifier is required.", nameof(titleId));
        }

        var userId = currentUser.UserId;

        if (!await users.ExistsAsync(userId, ct))
        {
            throw new EntityNotFoundException("User", userId);
        }

        var title = await catalogReader.GetByIdAsync(titleId, userId, ct);

        if (title is null)
        {
            throw new EntityNotFoundException("Title", titleId);
        }

        var entry = await libraryEntries.GetAsync(userId, titleId, ct);

        var now = timeProvider.GetUtcNow();

        if (entry is null)
        {
            entry = LibraryEntry.CreatePlanned(userId, titleId, now);

            libraryEntries.Add(entry);
        }

        entry.SetReview(command.Rating, command.Vibe, command.Review, command.ContainsSpoilers, now);

        await unitOfWork.SaveChangesAsync(ct);
    }
}