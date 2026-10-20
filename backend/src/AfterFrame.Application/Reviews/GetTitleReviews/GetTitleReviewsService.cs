using AfterFrame.Application.Catalog;
using AfterFrame.Application.Common.Exceptions;
using AfterFrame.Application.Common.Identity;
using AfterFrame.Application.Reviews.Models;

namespace AfterFrame.Application.Reviews.GetTitleReviews;

public sealed class GetTitleReviewsService(ICatalogReader catalogReader, IReviewReader reviewReader, ICurrentUser currentUser)
{
    public async Task<TitleReviewsSummary> ExecuteAsync(Guid titleId, CancellationToken ct = default)
    {
        if (titleId == Guid.Empty)
        {
            throw new ArgumentException("Title identifier is required.", nameof(titleId));
        }

        var title = await catalogReader.GetByIdAsync(titleId, currentUser.UserId, ct);

        if (title is null)
        {
            throw new EntityNotFoundException("Title", titleId);
        }

        return await reviewReader.GetForTitleAsync(titleId, currentUser.UserId, ct);
    }
}