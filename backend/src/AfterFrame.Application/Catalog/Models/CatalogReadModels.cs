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

public sealed record CatalogTitleExternalReference(
    string? ExternalSource,
    string? ExternalId);

public sealed record CatalogCastMember(
    string Id,
    string Name,
    string? Character,
    string? ProfilePath);

public sealed record CatalogTitleMedia(
    CatalogVideo? Trailer,
    IReadOnlyList<CatalogMediaImage> Backdrops,
    IReadOnlyList<CatalogMediaImage> Posters);

public sealed record CatalogVideo(
    string Id,
    string Name,
    string Site,
    string Key,
    string Type,
    bool Official);

public sealed record CatalogMediaImage(
    string Path,
    int Width,
    int Height);