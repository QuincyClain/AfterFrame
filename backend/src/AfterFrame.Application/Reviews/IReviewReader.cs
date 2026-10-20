using AfterFrame.Application.Reviews.Models;

namespace AfterFrame.Application.Reviews;

public interface IReviewReader
{
    Task<TitleReviewsSummary> GetForTitleAsync(Guid titleId, Guid currentUserId, CancellationToken ct = default);
}