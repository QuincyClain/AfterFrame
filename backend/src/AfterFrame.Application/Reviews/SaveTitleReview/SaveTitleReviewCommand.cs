using AfterFrame.Domain.Library;

namespace AfterFrame.Application.Reviews.SaveTitleReview;

public sealed record SaveTitleReviewCommand(
    decimal Rating,
    ReviewVibe Vibe,
    string? Review,
    bool ContainsSpoilers);