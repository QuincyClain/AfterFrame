namespace AfterFrame.Application.Common.ExternalCatalog;

public interface IExternalCatalogClient
{
    Task<IReadOnlyList<ExternalCatalogTitle>> GetPopularMoviesAsync(int page, CancellationToken ct = default);

    Task<IReadOnlyList<ExternalCatalogTitle>> GetPopularSeriesAsync(int page, CancellationToken ct = default);

    Task<IReadOnlyList<ExternalCatalogTitle>> GetPopularAnimeAsync(int page, CancellationToken ct = default);
}