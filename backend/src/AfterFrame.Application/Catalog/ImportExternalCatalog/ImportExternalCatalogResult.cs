namespace AfterFrame.Application.Catalog.ImportExternalCatalog;

public sealed record ImportExternalCatalogResult(
    int FetchedCount,
    int CreatedCount,
    int UpdatedCount,
    int CreatedGenresCount);