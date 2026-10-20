using AfterFrame.Application.Catalog.Models;
using AfterFrame.Application.Common.Exceptions;
using AfterFrame.Application.Common.ExternalCatalog;
using AfterFrame.Application.Common.Identity;

namespace AfterFrame.Application.Catalog.GetTitleCast;

public sealed class GetTitleCastService(ICatalogReader catalogReader, IExternalCatalogClient externalCatalogClient, ICurrentUser currentUser)
{
    public async Task<IReadOnlyList<CatalogCastMember>> ExecuteAsync(Guid titleId, CancellationToken ct = default)
    {
        if (titleId == Guid.Empty)
        {
            throw new ArgumentException("Title identifier is required.", nameof(titleId));
        }

        var externalReference = await catalogReader.GetExternalReferenceAsync(titleId, currentUser.UserId, ct);

        if (externalReference is null)
        {
            throw new EntityNotFoundException("Title", titleId);
        }

        if (!string.Equals(externalReference.ExternalSource, "tmdb", StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrWhiteSpace(externalReference.ExternalId))
        {
            return [];
        }

        var cast = await externalCatalogClient.GetCastAsync(externalReference.ExternalId, ct);

        return cast
            .OrderBy(member => member.Order)
            .Select(member => new CatalogCastMember(
                member.ExternalId,
                member.Name,
                member.Character,
                member.ProfilePath))
            .ToArray();
    }
}