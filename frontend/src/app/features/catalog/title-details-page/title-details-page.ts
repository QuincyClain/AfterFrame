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
  CatalogTitleSummary,
} from '../data-access/catalog-api.models';
import { TitleTrailerDialog } from '../title-details/title-trailer-dialog/title-trailer-dialog';
import { TitleMediaSection } from '../title-details/title-media-section/title-media-section';
import { TitleImageDialog } from '../title-details/title-image-dialog/title-image-dialog';
import { TitleRecommendationsSection } from '../title-details/title-recommendations-section/title-recommendations-section';
import { TitleReviewsSection } from '../../reviews/title-reviews-section/title-reviews-section';
import { ReviewsApi } from '../../reviews/data-access/reviews-api';
import { TitleReviewsSummary } from '../../reviews/data-access/reviews-api.models';
import { switchMap } from 'rxjs';
import { TitleReviewDialog } from '../../reviews/title-review-dialog/title-review-dialog';
import { SaveTitleReviewRequest } from '../../reviews/data-access/reviews-api.models';

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
    TitleRecommendationsSection,
    TitleReviewsSection,
    TitleReviewDialog,
  ],
  templateUrl: './title-details-page.html',
  styleUrl: './title-details-page.css',
})
export class TitleDetailsPage {
  readonly id = input.required<string>();

  private readonly catalogApi = inject(CatalogApi);
  private readonly reviewsApi = inject(ReviewsApi);
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
  protected readonly recommendations = signal<readonly CatalogTitleSummary[]>([]);
  protected readonly isRecommendationsLoading = signal(true);
  protected readonly recommendationsLoadFailed = signal(false);
  protected readonly reviews = signal<TitleReviewsSummary | null>(null);
  protected readonly isReviewsLoading = signal(true);
  protected readonly reviewsLoadFailed = signal(false);
  protected readonly isCastLoading = signal(true);
  protected readonly castLoadFailed = signal(false);
  protected readonly isLoading = signal(true);
  protected readonly isNotFound = signal(false);
  protected readonly errorMessage = signal<string | null>(null);
  protected readonly isReviewDialogOpen = signal(false);
  protected readonly isReviewSaving = signal(false);
  protected readonly isReviewDeleting = signal(false);
  protected readonly reviewActionError = signal<string | null>(null);

  protected readonly currentUserReview = computed(
    () => this.reviews()?.items.find((review) => review.isCurrentUser) ?? null,
  );

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
      this.recommendations.set([]);
      this.isRecommendationsLoading.set(true);
      this.recommendationsLoadFailed.set(false);
      this.reviews.set(null);
      this.isReviewsLoading.set(true);
      this.reviewsLoadFailed.set(false);

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

      const recommendationsSubscription = this.catalogApi
        .getTitleRecommendations(titleId)
        .subscribe({
          next: (recommendations) => {
            this.recommendations.set(recommendations);
            this.isRecommendationsLoading.set(false);
          },
          error: () => {
            this.isRecommendationsLoading.set(false);
            this.recommendationsLoadFailed.set(true);
          },
        });

      const reviewsSubscription = this.reviewsApi
        .getTitleReviews(titleId)
        .subscribe({
          next: (reviews) => {
            this.reviews.set(reviews);
            this.isReviewsLoading.set(false);
          },
          error: () => {
            this.isReviewsLoading.set(false);
            this.reviewsLoadFailed.set(true);
          },
        });

      onCleanup(() => {
        titleSubscription.unsubscribe();
        castSubscription.unsubscribe();
        mediaSubscription.unsubscribe();
        recommendationsSubscription.unsubscribe();
        reviewsSubscription.unsubscribe();
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

  protected openReviewDialog(): void {
    this.reviewActionError.set(null);
    this.isReviewDialogOpen.set(true);
  }

  protected closeReviewDialog(): void {
    if (!this.isReviewSaving() && !this.isReviewDeleting()) {
      this.isReviewDialogOpen.set(false);
      this.reviewActionError.set(null);
    }
  }

  protected saveReview(
    request: SaveTitleReviewRequest,
  ): void {
    this.isReviewSaving.set(true);
    this.reviewActionError.set(null);

    this.reviewsApi
      .saveTitleReview(this.id(), request)
      .pipe(
        switchMap(() =>
          this.reviewsApi.getTitleReviews(this.id()),
        ),
      )
      .subscribe({
        next: (reviews) => {
          this.reviews.set(reviews);
          this.isReviewSaving.set(false);
          this.isReviewDialogOpen.set(false);
        },
        error: () => {
          this.isReviewSaving.set(false);
          this.reviewActionError.set('We could not save your review. Please try again.',
          );
        },
      });
  }

  protected deleteReview(): void {
    this.isReviewDeleting.set(true);
    this.reviewActionError.set(null);

    this.reviewsApi
      .deleteTitleReview(this.id())
      .pipe(
        switchMap(() =>
          this.reviewsApi.getTitleReviews(this.id()),
        ),
      )
      .subscribe({
        next: (reviews) => {
          this.reviews.set(reviews);
          this.isReviewDeleting.set(false);
          this.isReviewDialogOpen.set(false);
        },
        error: () => {
          this.isReviewDeleting.set(false);
          this.reviewActionError.set('We could not remove your review. Please try again.',
          );
        },
      });
  }
}
