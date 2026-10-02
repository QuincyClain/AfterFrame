using AfterFrame.Domain.Titles;

namespace AfterFrame.Application.Catalog.Models;

public enum CatalogSort
{
    Rating = 1,
    Votes = 2,
    ReleaseYear = 3,
    Name = 4
}

public sealed record CatalogQuery
{
    public string? Search { get; init; }

    public TitleType? Type { get; init; }

    public TitleTag? Tag { get; init; }

    public string? Genre { get; init; }

    public int? ReleaseYear { get; init; }

    public CatalogSort Sort { get; init; } = CatalogSort.Rating;

    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 20;
}