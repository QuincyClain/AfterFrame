import { HttpErrorResponse } from '@angular/common/http';
import { Component, computed, effect, inject, input, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CatalogApi } from '../data-access/catalog-api';
import { TitleHero } from '../title-details/title-hero/title-hero';
import { TitleSectionNav } from '../title-details/title-section-nav/title-section-nav';
import { TitleFacts } from '../title-details/title-facts/title-facts';
import { TitleActivityCard } from '../../library/title-activity-card/title-activity-card';
import { TitleCastSection } from '../title-details/title-cast-section/title-cast-section';
import {
  CatalogCastMember,
  CatalogMediaImage,
  CatalogTitleDetails,
  CatalogTitleMedia,
} from '../data-access/catalog-api.models';
import { TitleTrailerDialog } from '../title-details/title-trailer-dialog/title-trailer-dialog';
import { TitleMediaSection } from '../title-details/title-media-section/title-media-section';
import { TitleImageDialog } from '../title-details/title-image-dialog/title-image-dialog';

@Component({
  selector: 'app-title-details-page',
  imports: [
    RouterLink,
    TitleHero,
    TitleSectionNav,
    TitleFacts,
    TitleActivityCard,
    TitleCastSection,
    TitleTrailerDialog,
    TitleMediaSection,
    TitleImageDialog,
  ],
  templateUrl: './title-details-page.html',
  styleUrl: './title-details-page.css',
})
export class TitleDetailsPage {
  readonly id = input.required<string>();

  private readonly catalogApi = inject(CatalogApi);
  private readonly reloadVersion = signal(0);

  protected readonly title = signal<CatalogTitleDetails | null>(null);
  protected readonly cast = signal<readonly CatalogCastMember[]>([]);
  protected readonly media = signal<CatalogTitleMedia | null>(null);
  protected readonly isMediaLoading = signal(true);
  protected readonly mediaLoadFailed = signal(false);
  protected readonly isTrailerOpen = signal(false);
  protected readonly trailer = computed(() => this.media()?.trailer ?? null);
  protected readonly selectedMediaImage = signal<CatalogMediaImage | null>(null);
  protected readonly mediaImages = computed(() => [
    ...(this.media()?.backdrops ?? []),
    ...(this.media()?.posters ?? []),
  ]);
  protected readonly isCastLoading = signal(true);
  protected readonly castLoadFailed = signal(false);
  protected readonly isLoading = signal(true);
  protected readonly isNotFound = signal(false);
  protected readonly errorMessage = signal<string | null>(null);

  constructor() {
    effect((onCleanup) => {
      this.reloadVersion();

      const titleId = this.id();

      this.title.set(null);
      this.cast.set([]);

      this.isLoading.set(true);
      this.isCastLoading.set(true);

      this.isNotFound.set(false);
      this.errorMessage.set(null);
      this.castLoadFailed.set(false);
      this.media.set(null);
      this.isMediaLoading.set(true);
      this.mediaLoadFailed.set(false);
      this.isTrailerOpen.set(false);
      this.selectedMediaImage.set(null);

      const titleSubscription = this.catalogApi.getTitleById(titleId).subscribe({
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

      const castSubscription = this.catalogApi.getTitleCast(titleId).subscribe({
        next: (cast) => {
          this.cast.set(cast);
          this.isCastLoading.set(false);
        },
        error: () => {
          this.isCastLoading.set(false);
          this.castLoadFailed.set(true);
        },
      });

      const mediaSubscription = this.catalogApi.getTitleMedia(titleId).subscribe({
        next: (media) => {
          this.media.set(media);
          this.isMediaLoading.set(false);
        },
        error: () => {
          this.isMediaLoading.set(false);
          this.mediaLoadFailed.set(true);
        },
      });

      onCleanup(() => {
        titleSubscription.unsubscribe();
        castSubscription.unsubscribe();
        mediaSubscription.unsubscribe();
      });
    });
  }

  protected retry(): void {
    this.reloadVersion.update((version) => version + 1);
  }

  protected openTrailer(): void {
    if (this.trailer()) {
      this.isTrailerOpen.set(true);
    }
  }

  protected closeTrailer(): void {
    this.isTrailerOpen.set(false);
  }

  protected openMediaImage(image: CatalogMediaImage): void {
    this.isTrailerOpen.set(false);
    this.selectedMediaImage.set(image);
  }

  protected closeMediaImage(): void {
    this.selectedMediaImage.set(null);
  }
}
