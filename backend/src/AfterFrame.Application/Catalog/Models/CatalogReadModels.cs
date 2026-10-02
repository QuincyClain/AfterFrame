using AfterFrame.Domain.Titles;

namespace AfterFrame.Application.Catalog.Models;

public sealed record CatalogTitleSummary(
    Guid Id,
    string Name,
    TitleType Type,
    TitleTag Tags,
    int ReleaseYear,
    string? PosterPath,
    decimal? ExternalRating,
    int ExternalVoteCount,
    TitlePublicationStatus PublicationStatus,
    IReadOnlyList<string> Genres);

public sealed record CatalogTitleDetails(
    Guid Id,
    string Name,
    TitleType Type,
    TitleTag Tags,
    int ReleaseYear,
    string Description,
    string? PosterPath,
    string? BackdropPath,
    decimal? ExternalRating,
    int ExternalVoteCount,
    TitleOrigin Origin,
    TitlePublicationStatus PublicationStatus,
    string? ExternalSource,
    IReadOnlyList<string> Genres);