using AfterFrame.Domain.Users;

namespace AfterFrame.Domain.Titles;

public sealed class Title
{
    private Title()
    {
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public TitleType Type { get; private set; }

    public TitleTag Tags { get; private set; }

    public int ReleaseYear { get; private set; }

    public string Description { get; private set; } = string.Empty;

    public TitleOrigin Origin { get; private set; }

    public TitlePublicationStatus PublicationStatus { get; private set; }

    public string? ExternalSource { get; private set; }

    public string? ExternalId { get; private set; }

    public Guid? CreatedByUserId { get; private set; }

    public User? CreatedByUser { get; private set; }

    public DateTimeOffset CreatedAtUtc { get; private set; }

    public DateTimeOffset UpdatedAtUtc { get; private set; }

    public static Title CreateExternal(
        string name,
        TitleType type,
        TitleTag tags,
        int releaseYear,
        string description,
        string externalSource,
        string externalId,
        DateTimeOffset createdAtUtc)
    {
        Validate(name, releaseYear);

        if (string.IsNullOrWhiteSpace(externalSource))
        {
            throw new ArgumentException("External source is required.", nameof(externalSource));
        }

        if (string.IsNullOrWhiteSpace(externalId))
        {
            throw new ArgumentException("External identifier is required.", nameof(externalId));
        }

        return new Title
        {
            Id = Guid.CreateVersion7(),
            Name = name.Trim(),
            Type = type,
            Tags = tags,
            ReleaseYear = releaseYear,
            Description = description.Trim(),
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
        Validate(name, releaseYear);

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

    private static void Validate(string name, int releaseYear)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Title name is required.", nameof(name));
        }

        if (releaseYear <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(releaseYear), "Release year must be positive.");
        }
    }
}