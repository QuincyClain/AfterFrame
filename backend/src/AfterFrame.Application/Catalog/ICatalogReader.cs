using AfterFrame.Application.Catalog.Models;
using AfterFrame.Application.Common.Models;

namespace AfterFrame.Application.Catalog;

public interface ICatalogReader
{
    Task<PagedResult<CatalogTitleSummary>> GetPageAsync(Guid currentUserId, CatalogQuery query, CancellationToken ct = default);

    Task<CatalogTitleDetails?> GetByIdAsync(Guid titleId, Guid currentUserId, CancellationToken ct = default);
}