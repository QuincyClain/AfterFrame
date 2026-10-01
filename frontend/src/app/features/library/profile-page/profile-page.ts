import { Component, computed, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CatalogData } from '../../catalog/catalog-data';
import { Title } from '../../catalog/title';
import { StarRating } from '../../../shared/star-rating/star-rating';
import { LibraryData } from '../library-data';
import { RatedLibraryEntry, WatchStatus } from '../library-entry';

interface RankedTitle {
  readonly title: Title;
  readonly entry: RatedLibraryEntry;
}

const WATCH_STATUS_LABELS: Readonly<Record<WatchStatus, string>> = {
  planned: 'Plan to watch',
  watching: 'Watching',
  completed: 'Completed',
  'on-hold': 'On hold',
  dropped: 'Dropped',
};

@Component({
  imports: [RouterLink, StarRating],
  selector: 'app-profile-page',
  styleUrl: './profile-page.css',
  templateUrl: './profile-page.html',
})
export class ProfilePage {
  private readonly catalogData = inject(CatalogData);
  private readonly libraryData = inject(LibraryData);

  protected readonly statusLabels = WATCH_STATUS_LABELS;

  protected readonly rankedTitles = computed<readonly RankedTitle[]>(() =>
    this.libraryData
      .ratedEntries()
      .flatMap((entry): RankedTitle[] => {
        const title = this.catalogData.getTitleById(entry.titleId);

        return title ? [{ title, entry }] : [];
      })
      .sort((left, right) => {
        const ratingDifference = right.entry.rating - left.entry.rating;

        return (
          ratingDifference ||
          left.title.title.localeCompare(right.title.title)
        );
      }),
  );
}
