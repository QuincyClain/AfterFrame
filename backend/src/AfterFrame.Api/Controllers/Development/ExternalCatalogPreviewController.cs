using AfterFrame.Application.Common.ExternalCatalog;
using Microsoft.AspNetCore.Mvc;

namespace AfterFrame.Api.Controllers.Development;

[ApiController]
[Route("api/development/external-catalog")]
public sealed class ExternalCatalogPreviewController(IExternalCatalogClient externalCatalog, IHostEnvironment environment) : ControllerBase
{
    [HttpGet("movies")]
    public async Task<ActionResult<IReadOnlyList<ExternalCatalogTitle>>> GetMovies([FromQuery] int page = 1, CancellationToken ct = default)
    {
        if (!environment.IsDevelopment())
        {
            return NotFound();
        }

        var titles = await externalCatalog.GetPopularMoviesAsync(page, ct);

        return Ok(titles);
    }

    [HttpGet("series")]
    public async Task<ActionResult<IReadOnlyList<ExternalCatalogTitle>>> GetSeries([FromQuery] int page = 1, CancellationToken ct = default)
    {
        if (!environment.IsDevelopment())
        {
            return NotFound();
        }

        var titles = await externalCatalog.GetPopularSeriesAsync(page, ct);

        return Ok(titles);
    }

    [HttpGet("anime")]
    public async Task<ActionResult<IReadOnlyList<ExternalCatalogTitle>>> GetAnime([FromQuery] int page = 1, CancellationToken ct = default)
    {
        if (!environment.IsDevelopment())
        {
            return NotFound();
        }

        var titles = await externalCatalog.GetPopularAnimeAsync(page, ct);

        return Ok(titles);
    }
}