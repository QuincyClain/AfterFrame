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

    public int? LastWatchedSeason { get; private set; }

    public int? LastWatchedEpisode { get; private set; }

    public DateTimeOffset CreatedAtUtc { get; private set; }

    public DateTimeOffset UpdatedAtUtc { get; private set; }

    public static LibraryEntry CreatePlanned(
        Guid userId,
        Guid titleId,
        DateTimeOffset createdAtUtc)
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
}