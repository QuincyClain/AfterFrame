using AfterFrame.Application.Reviews;
using AfterFrame.Application.Reviews.Models;
using Microsoft.EntityFrameworkCore;

namespace AfterFrame.Infrastructure.Persistence.Readers;

internal sealed class EfReviewReader(AfterFrameDbContext dbContext) : IReviewReader
{
    public async Task<TitleReviewsSummary> GetForTitleAsync(Guid titleId, Guid currentUserId, CancellationToken ct = default)
    {
        var entries = dbContext.LibraryEntries
            .AsNoTracking()
            .Where(entry => entry.TitleId == titleId);

        var ratingStats = await entries
            .Where(entry => entry.Rating.HasValue)
            .GroupBy(_ => 1)
            .Select(group => new
            {
                AverageRating = group.Average(entry => entry.Rating!.Value),
                RatingCount = group.Count()
            })
            .SingleOrDefaultAsync(ct);

        var reviews = await entries
            .Where(entry => entry.Rating.HasValue && entry.Review != null && entry.Review != "")
            .OrderByDescending(entry => entry.UserId == currentUserId)
            .ThenByDescending(entry => entry.ReviewedAtUtc ?? entry.UpdatedAtUtc)
            .Select(entry => new TitleReviewItem(
                entry.UserId,
                entry.User.DisplayName,
                entry.Rating!.Value,
                entry.Vibe,
                entry.Review!,
                entry.ContainsSpoilers,
                entry.ReviewedAtUtc ?? entry.UpdatedAtUtc,
                entry.UserId == currentUserId))
            .ToArrayAsync(ct);

        return new TitleReviewsSummary(ratingStats?.AverageRating, ratingStats?.RatingCount ?? 0, reviews.Length, reviews);
    }
}