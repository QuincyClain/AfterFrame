namespace AfterFrame.Domain.Titles;

public enum TitleType
{
    Movie = 1,
    Series = 2
}

[Flags]
public enum TitleTag
{
    None = 0,
    Animation = 1,
    Anime = 2
}

public enum TitleOrigin
{
    External = 1,
    UserCreated = 2
}

public enum TitlePublicationStatus
{
    Private = 1,
    PendingReview = 2,
    Published = 3,
    Rejected = 4
}