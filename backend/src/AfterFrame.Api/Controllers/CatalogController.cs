using AfterFrame.Application.Catalog.GetCatalog;
using AfterFrame.Application.Catalog.GetTitleDetails;
using AfterFrame.Application.Catalog.GetTitleCast;
using AfterFrame.Application.Catalog.GetTitleMedia;
using AfterFrame.Application.Catalog.GetTitleRecommendations;
using AfterFrame.Application.Reviews.GetTitleReviews;
using AfterFrame.Application.Reviews.Models;
using AfterFrame.Application.Catalog.Models;
using AfterFrame.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace AfterFrame.Api.Controllers;

[ApiController]
[Route("api/catalog")]
public sealed class CatalogController(GetCatalogService getCatalog, GetTitleDetailsService getTitleDetails,
    GetTitleCastService getTitleCast, GetTitleMediaService getTitleMedia, 
    GetTitleRecommendationsService getTitleRecommendations, GetTitleReviewsService getTitleReviews) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<CatalogTitleSummary>>> Get([FromQuery] CatalogQuery query, CancellationToken ct)
    {
        var result = await getCatalog.ExecuteAsync(query, ct);

        return Ok(result);
    }

    [HttpGet("{titleId:guid}")]
    public async Task<ActionResult<CatalogTitleDetails>> GetById(Guid titleId, CancellationToken ct)
    {
        var result = await getTitleDetails.ExecuteAsync(titleId, ct);

        return Ok(result);
    }

    [HttpGet("{titleId:guid}/cast")]
    public async Task<ActionResult<IReadOnlyList<CatalogCastMember>>> GetCast(Guid titleId, CancellationToken ct)
    {
        var result = await getTitleCast.ExecuteAsync(titleId, ct);

        return Ok(result);
    }

    [HttpGet("{titleId:guid}/media")]
    public async Task<ActionResult<CatalogTitleMedia>> GetMedia(Guid titleId, CancellationToken ct)
    {
        var result = await getTitleMedia.ExecuteAsync(titleId, ct);

        return Ok(result);
    }
    
    [HttpGet("{titleId:guid}/recommendations")]
    public async Task<ActionResult<IReadOnlyList<CatalogTitleSummary>>> GetRecommendations(Guid titleId, CancellationToken ct)
    {
        var result = await getTitleRecommendations.ExecuteAsync(titleId, ct);

        return Ok(result);
    }
    
    [HttpGet("{titleId:guid}/reviews")]
    public async Task<ActionResult<TitleReviewsSummary>> GetReviews(Guid titleId, CancellationToken ct)
    {
        var result = await getTitleReviews.ExecuteAsync(titleId, ct);

        return Ok(result);
    }
}