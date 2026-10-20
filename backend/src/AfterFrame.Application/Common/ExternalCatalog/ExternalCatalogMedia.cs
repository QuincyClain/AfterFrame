namespace AfterFrame.Application.Common.ExternalCatalog;

public sealed record ExternalCatalogMedia(
    IReadOnlyList<ExternalCatalogVideo> Videos,
    IReadOnlyList<ExternalCatalogImage> Backdrops,
    IReadOnlyList<ExternalCatalogImage> Posters);

public sealed record ExternalCatalogVideo(
    string ExternalId,
    string Name,
    string Site,
    string Key,
    string Type,
    bool Official,
    DateTimeOffset? PublishedAt);

public sealed record ExternalCatalogImage(
    string Path,
    int Width,
    int Height);