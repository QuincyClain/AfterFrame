using AfterFrame.Application.Catalog.Models;
using AfterFrame.Application.Common.Exceptions;
using AfterFrame.Application.Common.ExternalCatalog;
using AfterFrame.Application.Common.Identity;

namespace AfterFrame.Application.Catalog.GetTitleMedia;

public sealed class GetTitleMediaService(ICatalogReader catalogReader, IExternalCatalogClient externalCatalogClient,
    ICurrentUser currentUser)
{
    public async Task<CatalogTitleMedia> ExecuteAsync(Guid titleId, CancellationToken ct = default)
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

        if (!string.Equals(externalReference.ExternalSource, "tmdb", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(
                externalReference.ExternalId))
        {
            return new CatalogTitleMedia(null, [], []);
        }

        var media = await externalCatalogClient.GetMediaAsync(externalReference.ExternalId, ct);

        var trailer = SelectTrailer(media.Videos);

        return new CatalogTitleMedia(trailer, media.Backdrops.Select(MapImage).ToArray(), media.Posters.Select(MapImage).ToArray());
    }

    private static CatalogVideo? SelectTrailer(IReadOnlyList<ExternalCatalogVideo> videos)
    {
        var trailer = videos.FirstOrDefault(video => video.Type.Equals("Trailer", StringComparison.OrdinalIgnoreCase))
                      ?? videos.FirstOrDefault();

        return trailer is null ? null
            : new CatalogVideo(
                trailer.ExternalId,
                trailer.Name,
                trailer.Site,
                trailer.Key,
                trailer.Type,
                trailer.Official);
    }

    private static CatalogMediaImage MapImage(ExternalCatalogImage image)
    {
        return new CatalogMediaImage(image.Path, image.Width, image.Height);
    }
}