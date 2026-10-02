import { DecimalPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, computed, input, output } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CatalogTitleDetails } from '../../data-access/catalog-api.models';

@Component({
  selector: 'app-title-hero',
  imports: [DecimalPipe, RouterLink],
  templateUrl: './title-hero.html',
  styleUrl: './title-hero.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TitleHero {
  readonly title = input.required<CatalogTitleDetails>();

  readonly addToLibrary = output<void>();
  readonly addToFavourites = output<void>();
  readonly rateAndReview = output<void>();
  readonly playTrailer = output<void>();

  protected readonly posterUrl = computed(() => {
    const posterPath = this.title().posterPath;

    return posterPath ? `https://image.tmdb.org/t/p/w780${posterPath}` : null;
  });

  protected readonly backdropBackground = computed(() => {
    const backdropPath = this.title().backdropPath;

    return backdropPath ? `url("https://image.tmdb.org/t/p/w1280${backdropPath}")` : null;
  });

  protected readonly scoreBackground = computed(() => {
    const rating = this.title().externalRating ?? 0;
    const percentage = Math.round(rating * 10);

    return `conic-gradient(var(--rating) ${percentage}%, rgb(255 255 255 / 18%) 0)`;
  });
}
