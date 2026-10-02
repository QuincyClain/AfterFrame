import { DecimalPipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, computed, effect, inject, input, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CatalogApi } from '../data-access/catalog-api';
import { CatalogTitleDetails } from '../data-access/catalog-api.models';

@Component({
  selector: 'app-title-details-page',
  imports: [DecimalPipe, RouterLink],
  templateUrl: './title-details-page.html',
  styleUrl: './title-details-page.css',
})
export class TitleDetailsPage {
  readonly id = input.required<string>();

  private readonly catalogApi = inject(CatalogApi);
  private readonly reloadVersion = signal(0);

  protected readonly title = signal<CatalogTitleDetails | null>(null);
  protected readonly isLoading = signal(true);
  protected readonly isNotFound = signal(false);
  protected readonly errorMessage = signal<string | null>(null);

  protected readonly posterUrl = computed(() => {
    const posterPath = this.title()?.posterPath;

    return posterPath ? `https://image.tmdb.org/t/p/w500${posterPath}` : null;
  });

  protected readonly backdropBackground = computed(() => {
    const backdropPath = this.title()?.backdropPath;

    return backdropPath ? `url("https://image.tmdb.org/t/p/w1280${backdropPath}")` : null;
  });

  protected readonly scoreBackground = computed(() => {
    const rating = this.title()?.externalRating ?? 0;
    const percentage = Math.round(rating * 10);

    return `conic-gradient(var(--rating) ${percentage}%, rgb(255 255 255 / 18%) 0)`;
  });

  constructor() {
    effect((onCleanup) => {
      this.reloadVersion();

      const titleId = this.id();

      this.title.set(null);
      this.isLoading.set(true);
      this.isNotFound.set(false);
      this.errorMessage.set(null);

      const subscription = this.catalogApi.getTitleById(titleId).subscribe({
        next: (title) => {
          this.title.set(title);
          this.isLoading.set(false);
        },
        error: (error: HttpErrorResponse) => {
          this.isLoading.set(false);

          if (error.status === 404) {
            this.isNotFound.set(true);
            return;
          }

          this.errorMessage.set('We could not load this title. Please try again.');
        },
      });

      onCleanup(() => subscription.unsubscribe());
    });
  }

  protected retry(): void {
    this.reloadVersion.update((version) => version + 1);
  }
}
