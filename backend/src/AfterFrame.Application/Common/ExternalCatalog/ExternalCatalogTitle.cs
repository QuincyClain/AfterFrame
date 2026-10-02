using AfterFrame.Domain.Titles;

namespace AfterFrame.Application.Common.ExternalCatalog;

public sealed record ExternalCatalogTitle(
    string Source,
    string ExternalId,
    string Name,
    TitleType Type,
    TitleTag Tags,
    int ReleaseYear,
    string Description,
    string? PosterPath,
    decimal? ExternalRating,
    int ExternalVoteCount,
    IReadOnlyList<ExternalCatalogGenre> Genres);