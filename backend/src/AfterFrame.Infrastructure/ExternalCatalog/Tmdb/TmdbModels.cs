using System.Text.Json.Serialization;

namespace AfterFrame.Infrastructure.ExternalCatalog.Tmdb;

internal sealed class TmdbPageResponse<T>
{
    [JsonPropertyName("results")]
    public List<T> Results { get; init; } = [];
}

internal sealed class TmdbGenreResponse
{
    [JsonPropertyName("genres")]
    public List<TmdbGenre> Genres { get; init; } = [];
}

internal sealed class TmdbGenre
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;
}

internal sealed class TmdbMovie
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("adult")]
    public bool Adult { get; init; }

    [JsonPropertyName("title")]
    public string Title { get; init; } = string.Empty;

    [JsonPropertyName("overview")]
    public string Overview { get; init; } = string.Empty;

    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; init; }

    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; init; }

    [JsonPropertyName("backdrop_path")]
    public string? BackdropPath { get; init; }

    [JsonPropertyName("vote_average")]
    public decimal VoteAverage { get; init; }

    [JsonPropertyName("vote_count")]
    public int VoteCount { get; init; }

    [JsonPropertyName("genre_ids")]
    public List<int> GenreIds { get; init; } = [];

    [JsonPropertyName("original_language")]
    public string OriginalLanguage { get; init; } = string.Empty;
}

internal sealed class TmdbSeries
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("adult")]
    public bool Adult { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("overview")]
    public string Overview { get; init; } = string.Empty;

    [JsonPropertyName("first_air_date")]
    public string? FirstAirDate { get; init; }

    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; init; }

    [JsonPropertyName("backdrop_path")]
    public string? BackdropPath { get; init; }

    [JsonPropertyName("vote_average")]
    public decimal VoteAverage { get; init; }

    [JsonPropertyName("vote_count")]
    public int VoteCount { get; init; }

    [JsonPropertyName("genre_ids")]
    public List<int> GenreIds { get; init; } = [];

    [JsonPropertyName("origin_country")]
    public List<string> OriginCountry { get; init; } = [];
}

internal sealed class TmdbCreditsResponse<TCastMember>
{
    [JsonPropertyName("cast")]
    public List<TCastMember> Cast { get; init; } = [];
}

internal sealed class TmdbMovieCastMember
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("character")]
    public string? Character { get; init; }

    [JsonPropertyName("profile_path")]
    public string? ProfilePath { get; init; }

    [JsonPropertyName("order")]
    public int Order { get; init; }
}

internal sealed class TmdbSeriesCastMember
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("profile_path")]
    public string? ProfilePath { get; init; }

    [JsonPropertyName("order")]
    public int Order { get; init; }

    [JsonPropertyName("roles")]
    public List<TmdbSeriesCastRole> Roles { get; init; } = [];
}

internal sealed class TmdbSeriesCastRole
{
    [JsonPropertyName("character")]
    public string Character { get; init; } = string.Empty;
}