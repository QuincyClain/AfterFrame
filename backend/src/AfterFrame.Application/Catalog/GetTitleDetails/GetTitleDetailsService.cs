using AfterFrame.Application.Catalog.Models;
using AfterFrame.Application.Common.Exceptions;
using AfterFrame.Application.Common.Identity;

namespace AfterFrame.Application.Catalog.GetTitleDetails;

public sealed class GetTitleDetailsService(ICatalogReader catalogReader, ICurrentUser currentUser)
{
    public async Task<CatalogTitleDetails> ExecuteAsync(Guid titleId, CancellationToken ct = default)
    {
        if (titleId == Guid.Empty)
        {
            throw new ArgumentException("Title identifier is required.", nameof(titleId));
        }

        var title = await catalogReader.GetByIdAsync(titleId, currentUser.UserId, ct);

        return title ?? throw new EntityNotFoundException("Title", titleId);
    }
}