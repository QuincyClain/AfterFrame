using System.Globalization;
using System.Net.Http.Json;
using AfterFrame.Application.Common.ExternalCatalog;
using AfterFrame.Domain.Titles;
using Microsoft.Extensions.Options;

namespace AfterFrame.Infrastructure.ExternalCatalog.Tmdb;

internal sealed class TmdbCatalogClient : IExternalCatalogClient
{
    private const string Source = "tmdb";
    private const int AnimationGenreId = 16;

    private readonly HttpClient _httpClient;
    private readonly TmdbOptions _options;

    public TmdbCatalogClient(HttpClient httpClient, IOptions<TmdbOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<IReadOnlyList<ExternalCatalogTitle>> GetPopularMoviesAsync(int page, CancellationToken ct = default)
    {
        ValidatePage(page);

        var genres = await GetGenresAsync("genre/movie/list", ct);
        var response = await GetAsync<TmdbPageResponse<TmdbMovie>>($"movie/popular?language={Language}&page={page}", ct);

        return response.Results
            .Select(movie => MapMovie(movie, genres, forceAnime: false))
            .OfType<ExternalCatalogTitle>()
            .ToArray();
    }

    public async Task<IReadOnlyList<ExternalCatalogTitle>> GetPopularSeriesAsync(int page, CancellationToken ct = default)
    {
        ValidatePage(page);

        var genres = await GetGenresAsync("genre/tv/list", ct);
        var response = await GetAsync<TmdbPageResponse<TmdbSeries>>($"tv/popular?language={Language}&page={page}", ct);

        return response.Results
            .Select(series => MapSeries(series, genres, forceAnime: false))
            .OfType<ExternalCatalogTitle>()
            .ToArray();
    }

    public async Task<IReadOnlyList<ExternalCatalogTitle>> GetPopularAnimeAsync(int page, CancellationToken ct = default)
    {
        ValidatePage(page);

        var movieGenres = await GetGenresAsync("genre/movie/list", ct);
        var seriesGenres = await GetGenresAsync("genre/tv/list", ct);

        var movies = await GetAsync<TmdbPageResponse<TmdbMovie>>(
            $"discover/movie?include_adult=false&include_video=false&language={Language}&page={page}&sort_by=popularity.desc&with_genres={AnimationGenreId}&with_original_language=ja",
            ct);

        var series = await GetAsync<TmdbPageResponse<TmdbSeries>>(
            $"discover/tv?include_adult=false&include_null_first_air_dates=false&language={Language}&page={page}&sort_by=popularity.desc&with_genres={AnimationGenreId}&with_origin_country=JP",
            ct);

        var animeMovies = movies.Results
            .Select(movie => MapMovie(movie, movieGenres, forceAnime: true))
            .OfType<ExternalCatalogTitle>()
            .Take(10);

        var animeSeries = series.Results
            .Select(item => MapSeries(item, seriesGenres, forceAnime: true))
            .OfType<ExternalCatalogTitle>()
            .Take(10);

        return animeMovies.Concat(animeSeries).ToArray();
    }

    private async Task<IReadOnlyDictionary<int, ExternalCatalogGenre>> GetGenresAsync(
        string path,
        CancellationToken ct)
    {
        var response = await GetAsync<TmdbGenreResponse>(
            $"{path}?language={Language}",
            ct);

        return response.Genres.ToDictionary(genre => genre.Id, genre => new ExternalCatalogGenre(
                genre.Id.ToString(CultureInfo.InvariantCulture),
                genre.Name));
    }

    private async Task<T> GetAsync<T>(string path, CancellationToken ct)
    {
        var response = await _httpClient.GetFromJsonAsync<T>(path, ct);

        return response ?? throw new InvalidOperationException($"TMDB returned an empty response for '{path}'.");
    }

    private static ExternalCatalogTitle? MapMovie(TmdbMovie movie, IReadOnlyDictionary<int, ExternalCatalogGenre> genres, bool forceAnime)
    {
        var releaseYear = ParseYear(movie.ReleaseDate);

        if (movie.Adult || releaseYear is null || string.IsNullOrWhiteSpace(movie.Title))
        {
            return null;
        }

        var isAnime = forceAnime || movie.OriginalLanguage == "ja" && movie.GenreIds.Contains(AnimationGenreId);

        return new ExternalCatalogTitle(
            Source,
            $"movie:{movie.Id}",
            movie.Title.Trim(),
            TitleType.Movie,
            GetTags(movie.GenreIds, isAnime),
            releaseYear.Value,
            movie.Overview.Trim(),
            movie.PosterPath,
            movie.BackdropPath,
            GetRating(movie.VoteAverage, movie.VoteCount),
            movie.VoteCount,
            MapGenres(movie.GenreIds, genres));
    }

    private static ExternalCatalogTitle? MapSeries(TmdbSeries series, IReadOnlyDictionary<int, ExternalCatalogGenre> genres, bool forceAnime)
    {
        var releaseYear = ParseYear(series.FirstAirDate);

        if (series.Adult || releaseYear is null || string.IsNullOrWhiteSpace(series.Name))
        {
            return null;
        }

        var isAnime = forceAnime || series.OriginCountry.Contains("JP") && series.GenreIds.Contains(AnimationGenreId);

        return new ExternalCatalogTitle(
            Source,
            $"tv:{series.Id}",
            series.Name.Trim(),
            TitleType.Series,
            GetTags(series.GenreIds, isAnime),
            releaseYear.Value,
            series.Overview.Trim(),
            series.PosterPath,
            series.BackdropPath,
            GetRating(series.VoteAverage, series.VoteCount),
            series.VoteCount,
            MapGenres(series.GenreIds, genres));
    }

    private static IReadOnlyList<ExternalCatalogGenre> MapGenres(IEnumerable<int> genreIds, IReadOnlyDictionary<int, ExternalCatalogGenre> genres)
    {
        return genreIds.Where(genres.ContainsKey).Select(id => genres[id]).ToArray();
    }

    private static TitleTag GetTags(IReadOnlyCollection<int> genreIds, bool isAnime)
    {
        var tags = TitleTag.None;

        if (genreIds.Contains(AnimationGenreId))
        {
            tags |= TitleTag.Animation;
        }

        if (isAnime)
        {
            tags |= TitleTag.Animation | TitleTag.Anime;
        }

        return tags;
    }

    private static decimal? GetRating(decimal rating, int voteCount)
    {
        return voteCount > 0 ? rating : null;
    }

    private static int? ParseYear(string? date)
    {
        return DateOnly.TryParse(date, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed)
            ? parsed.Year
            : null;
    }

    private static void ValidatePage(int page)
    {
        if (page < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(page), "Page must be greater than zero.");
        }
    }

    private string Language => Uri.EscapeDataString(_options.Language);
}