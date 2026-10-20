using AfterFrame.Application.Catalog.Models;
using AfterFrame.Application.Common.Exceptions;
using AfterFrame.Application.Common.Identity;

namespace AfterFrame.Application.Catalog.GetTitleRecommendations;

public sealed class GetTitleRecommendationsService(ICatalogReader catalogReader, ICurrentUser currentUser)
{
    private const int RecommendationLimit = 8;

    public async Task<IReadOnlyList<CatalogTitleSummary>> ExecuteAsync(Guid titleId, CancellationToken ct = default)
    {
        if (titleId == Guid.Empty)
        {
            throw new ArgumentException("Title identifier is required.", nameof(titleId));
        }

        var title = await catalogReader.GetByIdAsync(titleId, currentUser.UserId, ct);

        if (title is null)
        {
            throw new EntityNotFoundException("Title", titleId);
        }

        return await catalogReader.GetRecommendationsAsync(titleId, currentUser.UserId, RecommendationLimit, ct);
    }
}