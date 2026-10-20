using AfterFrame.Domain.Library;

namespace AfterFrame.Application.Reviews.Models;

public sealed record TitleReviewsSummary(
    decimal? AverageRating,
    int RatingCount,
    int ReviewCount,
    IReadOnlyList<TitleReviewItem> Items);

public sealed record TitleReviewItem(
    Guid UserId,
    string UserDisplayName,
    decimal Rating,
    ReviewVibe? Vibe,
    string Review,
    bool ContainsSpoilers,
    DateTimeOffset ReviewedAtUtc,
    bool IsCurrentUser);