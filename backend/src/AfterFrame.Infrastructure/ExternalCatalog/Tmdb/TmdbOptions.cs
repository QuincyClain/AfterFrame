namespace AfterFrame.Infrastructure.ExternalCatalog.Tmdb;

internal sealed class TmdbOptions
{
    internal const string SectionName = "Tmdb";

    public string BaseUrl { get; init; } = string.Empty;

    public string AccessToken { get; init; } = string.Empty;

    public string Language { get; init; } = "en-US";
}