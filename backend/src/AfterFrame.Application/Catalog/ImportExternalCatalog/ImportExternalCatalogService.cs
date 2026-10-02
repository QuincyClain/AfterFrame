using AfterFrame.Application.Common.ExternalCatalog;
using AfterFrame.Application.Common.Persistence;
using AfterFrame.Application.Titles;
using AfterFrame.Domain.Titles;

namespace AfterFrame.Application.Catalog.ImportExternalCatalog;

public sealed class ImportExternalCatalogService(
    IExternalCatalogClient externalCatalog,
    ITitleRepository titleRepository,
    IGenreRepository genreRepository,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider)
{
    public async Task<ImportExternalCatalogResult> ExecuteAsync(int page, CancellationToken ct = default)
    {
        if (page < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(page), "Page must be greater than zero.");
        }

        var moviesTask = externalCatalog.GetPopularMoviesAsync(page, ct);

        var seriesTask = externalCatalog.GetPopularSeriesAsync(page, ct);

        var animeTask = externalCatalog.GetPopularAnimeAsync(page, ct);

        await Task.WhenAll(moviesTask, seriesTask, animeTask);

        var externalTitles = (await moviesTask)
            .Concat(await seriesTask)
            .Concat(await animeTask)
            .DistinctBy(title => (title.Source, title.ExternalId))
            .ToArray();

        var existingTitles = new List<Title>();

        foreach (var sourceGroup in externalTitles.GroupBy(title => title.Source))
        {
            var externalIds = sourceGroup
                .Select(title => title.ExternalId)
                .ToArray();

            var titles = await titleRepository
                .GetByExternalIdsAsync(sourceGroup.Key, externalIds, ct);

            existingTitles.AddRange(titles);
        }

        var existingTitlesByExternalIdentity = existingTitles
            .ToDictionary(title => (Source: title.ExternalSource!, ExternalId: title.ExternalId!));

        var genreNames = externalTitles
            .SelectMany(title => title.Genres)
            .Select(genre => genre.Name.Trim())
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var existingGenres = await genreRepository.GetByNamesAsync(genreNames, ct);

        var genresByName = existingGenres.ToDictionary(genre => genre.Name, StringComparer.OrdinalIgnoreCase);

        var createdGenres = new List<Genre>();

        foreach (var genreName in genreNames)
        {
            if (genresByName.ContainsKey(genreName))
            {
                continue;
            }

            var genre = Genre.Create(genreName);

            genresByName.Add(genre.Name, genre);
            createdGenres.Add(genre);
        }

        genreRepository.AddRange(createdGenres);

        var createdTitles = new List<Title>();
        var updatedCount = 0;
        var now = timeProvider.GetUtcNow();

        foreach (var externalTitle in externalTitles)
        {
            var genres = externalTitle.Genres
                .Select(genre => genresByName[genre.Name])
                .ToArray();

            var externalIdentity = (externalTitle.Source, externalTitle.ExternalId);

            if (existingTitlesByExternalIdentity.TryGetValue(externalIdentity, out var existingTitle))
            {
                existingTitle.UpdateExternalMetadata(
                    externalTitle.Name,
                    externalTitle.Type,
                    externalTitle.Tags,
                    externalTitle.ReleaseYear,
                    externalTitle.Description,
                    externalTitle.PosterPath,
                    externalTitle.BackdropPath,
                    externalTitle.ExternalRating,
                    externalTitle.ExternalVoteCount,
                    now);

                existingTitle.ReplaceGenres(genres);
                updatedCount++;

                continue;
            }

            var title = Title.CreateExternal(
                externalTitle.Name,
                externalTitle.Type,
                externalTitle.Tags,
                externalTitle.ReleaseYear,
                externalTitle.Description,
                externalTitle.PosterPath,
                externalTitle.BackdropPath,
                externalTitle.ExternalRating,
                externalTitle.ExternalVoteCount,
                externalTitle.Source,
                externalTitle.ExternalId,
                now);

            title.ReplaceGenres(genres);
            createdTitles.Add(title);
        }

        titleRepository.AddRange(createdTitles);

        await unitOfWork.SaveChangesAsync(ct);

        return new ImportExternalCatalogResult(externalTitles.Length, createdTitles.Count, updatedCount, createdGenres.Count);
    }
}