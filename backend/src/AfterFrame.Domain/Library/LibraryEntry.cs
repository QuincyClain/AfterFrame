using AfterFrame.Domain.Titles;
using AfterFrame.Domain.Users;

namespace AfterFrame.Domain.Library;

public sealed class LibraryEntry
{
    private LibraryEntry()
    {
    }

    public Guid UserId { get; private set; }

    public User User { get; private set; } = null!;

    public Guid TitleId { get; private set; }

    public Title Title { get; private set; } = null!;

    public WatchStatus Status { get; private set; }

    public decimal? Rating { get; private set; }

    public string? Review { get; private set; }
    
    public ReviewVibe? Vibe { get; private set; }

    public bool ContainsSpoilers { get; private set; }

    public DateTimeOffset? ReviewedAtUtc { get; private set; }

    public int? LastWatchedSeason { get; private set; }

    public int? LastWatchedEpisode { get; private set; }

    public DateTimeOffset CreatedAtUtc { get; private set; }

    public DateTimeOffset UpdatedAtUtc { get; private set; }

    public static LibraryEntry CreatePlanned(Guid userId, Guid titleId, DateTimeOffset createdAtUtc)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User identifier is required.", nameof(userId));
        }

        if (titleId == Guid.Empty)
        {
            throw new ArgumentException("Title identifier is required.", nameof(titleId));
        }

        return new LibraryEntry
        {
            UserId = userId,
            TitleId = titleId,
            Status = WatchStatus.Planned,
            CreatedAtUtc = createdAtUtc,
            UpdatedAtUtc = createdAtUtc
        };
    }
    
    public void SetReview(decimal rating, ReviewVibe vibe, string? review, bool containsSpoilers, DateTimeOffset reviewedAtUtc)
    {
        ValidateRating(rating);

        if (!Enum.IsDefined(vibe))
        {
            throw new ArgumentOutOfRangeException(nameof(vibe), "Review vibe is not supported.");
        }

        var normalizedReview = string.IsNullOrWhiteSpace(review) ? null : review.Trim();

        if (normalizedReview?.Length > 2000)
        {
            throw new ArgumentException("Review cannot be longer than 2000 characters.", nameof(review));
        }

        Rating = rating;
        Vibe = vibe;
        Review = normalizedReview;
        ContainsSpoilers = normalizedReview is not null && containsSpoilers;
        ReviewedAtUtc = reviewedAtUtc;
        UpdatedAtUtc = reviewedAtUtc;
    }

    public void RemoveReview(DateTimeOffset updatedAtUtc)
    {
        Rating = null;
        Vibe = null;
        Review = null;
        ContainsSpoilers = false;
        ReviewedAtUtc = null;
        UpdatedAtUtc = updatedAtUtc;
    }

    private static void ValidateRating(decimal rating)
    {
        var doubledRating = rating * 2;

        if (rating is < 0.5m or > 10m || doubledRating != decimal.Truncate(doubledRating))
        {
            throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 0.5 and 10 in increments of 0.5.");
        }
    }
    
}