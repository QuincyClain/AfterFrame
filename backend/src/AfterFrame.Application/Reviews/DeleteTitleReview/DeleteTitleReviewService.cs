using AfterFrame.Application.Catalog;
using AfterFrame.Application.Common.Exceptions;
using AfterFrame.Application.Common.Identity;
using AfterFrame.Application.Common.Persistence;
using AfterFrame.Application.Library;

namespace AfterFrame.Application.Reviews.DeleteTitleReview;

public sealed class DeleteTitleReviewService(ICatalogReader catalogReader, ICurrentUser currentUser, ILibraryEntryRepository libraryEntries,
    IUnitOfWork unitOfWork, TimeProvider timeProvider)
{
    public async Task ExecuteAsync(Guid titleId, CancellationToken ct = default)
    {
        if (titleId == Guid.Empty)
        {
            throw new ArgumentException("Title identifier is required.", nameof(titleId));
        }

        var userId = currentUser.UserId;

        var title = await catalogReader.GetByIdAsync(titleId, userId, ct);

        if (title is null)
        {
            throw new EntityNotFoundException("Title", titleId);
        }

        var entry = await libraryEntries.GetAsync(userId, titleId, ct);

        if (entry is null)
        {
            return;
        }

        entry.RemoveReview(timeProvider.GetUtcNow());

        await unitOfWork.SaveChangesAsync(ct);
    }
}