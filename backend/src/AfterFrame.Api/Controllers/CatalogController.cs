using AfterFrame.Application.Catalog.GetCatalog;
using AfterFrame.Application.Catalog.GetTitleDetails;
using AfterFrame.Application.Catalog.Models;
using AfterFrame.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace AfterFrame.Api.Controllers;

[ApiController]
[Route("api/catalog")]
public sealed class CatalogController(GetCatalogService getCatalog, GetTitleDetailsService getTitleDetails) : ControllerBase
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
}