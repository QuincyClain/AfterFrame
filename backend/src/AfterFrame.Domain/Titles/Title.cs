using AfterFrame.Domain.Users;

namespace AfterFrame.Domain.Titles;

public sealed class Title
{
    private readonly List<Genre> _genres = [];

    private Title()
    {
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public TitleType Type { get; private set; }

    public TitleTag Tags { get; private set; }

    public int ReleaseYear { get; private set; }

    public string Description { get; private set; } = string.Empty;

    public string? PosterPath { get; private set; }

    public decimal? ExternalRating { get; private set; }

    public int ExternalVoteCount { get; private set; }

    public TitleOrigin Origin { get; private set; }

    public TitlePublicationStatus PublicationStatus { get; private set; }

    public string? ExternalSource { get; private set; }

    public string? ExternalId { get; private set; }

    public Guid? CreatedByUserId { get; private set; }

    public User? CreatedByUser { get; private set; }

    public DateTimeOffset CreatedAtUtc { get; private set; }

    public DateTimeOffset UpdatedAtUtc { get; private set; }

    public IReadOnlyCollection<Genre> Genres => _genres;

    public static Title CreateExternal(
        string name,
        TitleType type,
        TitleTag tags,
        int releaseYear,
        string description,
        string? posterPath,
        decimal? externalRating,
        int externalVoteCount,
        string externalSource,
        string externalId,
        DateTimeOffset createdAtUtc)
    {
        Validate(name, releaseYear, description);
        ValidateExternalMetadata(externalRating, externalVoteCount, externalSource, externalId);

        return new Title
        {
            Id = Guid.CreateVersion7(),
            Name = name.Trim(),
            Type = type,
            Tags = tags,
            ReleaseYear = releaseYear,
            Description = description.Trim(),
            PosterPath = NormalizePosterPath(posterPath),
            ExternalRating = externalRating,
            ExternalVoteCount = externalVoteCount,
            Origin = TitleOrigin.External,
            PublicationStatus = TitlePublicationStatus.Published,
            ExternalSource = externalSource.Trim(),
            ExternalId = externalId.Trim(),
            CreatedAtUtc = createdAtUtc,
            UpdatedAtUtc = createdAtUtc
        };
    }

    public static Title CreatePrivate(
        Guid userId,
        string name,
        TitleType type,
        TitleTag tags,
        int releaseYear,
        string description,
        DateTimeOffset createdAtUtc)
    {
        Validate(name, releaseYear, description);

        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User identifier is required.", nameof(userId));
        }

        return new Title
        {
            Id = Guid.CreateVersion7(),
            Name = name.Trim(),
            Type = type,
            Tags = tags,
            ReleaseYear = releaseYear,
            Description = description.Trim(),
            Origin = TitleOrigin.UserCreated,
            PublicationStatus = TitlePublicationStatus.Private,
            CreatedByUserId = userId,
            CreatedAtUtc = createdAtUtc,
            UpdatedAtUtc = createdAtUtc
        };
    }

    public void UpdateExternalMetadata(
        string name,
        TitleType type,
        TitleTag tags,
        int releaseYear,
        string description,
        string? posterPath,
        decimal? externalRating,
        int externalVoteCount,
        DateTimeOffset updatedAtUtc)
    {
        if (Origin != TitleOrigin.External)
        {
            throw new InvalidOperationException("Only external titles can be updated from an external catalog.");
        }

        Validate(name, releaseYear, description);
        ValidateExternalRating(externalRating, externalVoteCount);

        Name = name.Trim();
        Type = type;
        Tags = tags;
        ReleaseYear = releaseYear;
        Description = description.Trim();
        PosterPath = NormalizePosterPath(posterPath);
        ExternalRating = externalRating;
        ExternalVoteCount = externalVoteCount;
        UpdatedAtUtc = updatedAtUtc;
    }

    public void ReplaceGenres(IEnumerable<Genre> genres)
    {
        ArgumentNullException.ThrowIfNull(genres);

        _genres.Clear();
        _genres.AddRange(genres.DistinctBy(genre => genre.Id));
    }

    private static void Validate(string name, int releaseYear, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Title name is required.", nameof(name));
        }

        if (releaseYear <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(releaseYear), "Release year must be positive.");
        }

        ArgumentNullException.ThrowIfNull(description);
    }

    private static void ValidateExternalMetadata(decimal? externalRating, int externalVoteCount, string externalSource, string externalId)
    {
        if (string.IsNullOrWhiteSpace(externalSource))
        {
            throw new ArgumentException("External source is required.", nameof(externalSource));
        }

        if (string.IsNullOrWhiteSpace(externalId))
        {
            throw new ArgumentException("External identifier is required.", nameof(externalId));
        }

        ValidateExternalRating(externalRating, externalVoteCount);
    }

    private static void ValidateExternalRating(decimal? externalRating, int externalVoteCount)
    {
        if (externalVoteCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(externalVoteCount), "External vote count cannot be negative.");
        }

        if (externalRating is < 0 or > 10)
        {
            throw new ArgumentOutOfRangeException(nameof(externalRating), "External rating must be between 0 and 10.");
        }

        var hasRatingWithoutVotes = externalRating is not null && externalVoteCount == 0;

        var hasVotesWithoutRating = externalRating is null && externalVoteCount > 0;

        if (hasRatingWithoutVotes || hasVotesWithoutRating)
        {
            throw new ArgumentException("External rating and vote count must be provided together.");
        }
    }

    private static string? NormalizePosterPath(string? posterPath)
    {
        return string.IsNullOrWhiteSpace(posterPath) ? null : posterPath.Trim();
    }
}