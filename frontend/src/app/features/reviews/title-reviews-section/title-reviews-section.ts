import { DatePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  input,
  effect,
  signal,
} from '@angular/core';
import {
  ReviewVibe,
  TitleReview,
  TitleReviewsSummary,
} from '../data-access/reviews-api.models';

interface VibeDetails {
  readonly emoji: string;
  readonly label: string;
}

const VIBE_DETAILS: Record<ReviewVibe, VibeDetails> = {
  Loved: { emoji: '😍', label: 'Loved it' },
  Happy: { emoji: '😊', label: 'Happy' },
  Amused: { emoji: '😂', label: 'Amused' },
  Excited: { emoji: '🤩', label: 'Excited' },
  Surprised: { emoji: '😮', label: 'Surprised' },
  Thoughtful: { emoji: '🤔', label: 'Thoughtful' },
  Emotional: { emoji: '🥹', label: 'Emotional' },
  Scared: { emoji: '😨', label: 'Scared' },
  Angry: { emoji: '😠', label: 'Angry' },
  Disappointed: { emoji: '😞', label: 'Disappointed' },
};

@Component({
  selector: 'app-title-reviews-section',
  imports: [DatePipe],
  templateUrl: './title-reviews-section.html',
  styleUrl: './title-reviews-section.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TitleReviewsSection {
  readonly reviews = input<TitleReviewsSummary | null>(null);
  readonly isLoading = input(false);
  readonly loadFailed = input(false);

  protected readonly reviewItems = computed(() => this.reviews()?.items ?? [],
  );

  private readonly revealedSpoilers =
    signal<ReadonlySet<string>>(new Set());

  protected getInitials(displayName: string): string {
    const initials = displayName
      .trim()
      .split(/\s+/)
      .slice(0, 2)
      .map((part) => part.charAt(0).toUpperCase())
      .join('');

    return initials || 'AF';
  }

  protected getVibeEmoji(vibe: ReviewVibe | null): string {
    return vibe ? VIBE_DETAILS[vibe].emoji : '✨';
  }

  protected getVibeLabel(vibe: ReviewVibe | null): string {
    return vibe ? VIBE_DETAILS[vibe].label : 'No vibe selected';
  }

  protected isSpoilerRevealed(review: TitleReview): boolean {
    return this.revealedSpoilers().has(review.userId);
  }

  protected revealSpoiler(userId: string): void {
    this.revealedSpoilers.update((current) => {
      const updated = new Set(current);
      updated.add(userId);
      return updated;
    });
  }

  private static readonly reviewBatchSize = 3;

  protected readonly visibleReviewCount = signal(TitleReviewsSection.reviewBatchSize,
  );

  protected readonly visibleReviewItems = computed(() =>
    this.reviewItems().slice(0, this.visibleReviewCount()),
  );

  protected readonly remainingReviewCount = computed(() =>
    Math.max(
      0,
      this.reviewItems().length - this.visibleReviewCount(),
    ),
  );

  protected readonly nextReviewBatchSize = computed(() =>
    Math.min(
      TitleReviewsSection.reviewBatchSize,
      this.remainingReviewCount(),
    ),
  );

  protected readonly hasMoreReviews = computed(() => this.remainingReviewCount() > 0,
  );

  protected readonly hasExpandedReviews = computed(
    () =>
      this.reviewItems().length >
      TitleReviewsSection.reviewBatchSize &&
      this.visibleReviewCount() >
      TitleReviewsSection.reviewBatchSize,
  );

  constructor() {
    effect(() => {
      this.reviews();

      this.visibleReviewCount.set(
        TitleReviewsSection.reviewBatchSize,
      );
    });
  }

  protected showMoreReviews(): void {
    this.visibleReviewCount.update(
      (count) => count + TitleReviewsSection.reviewBatchSize,
    );
  }

  protected showFewerReviews(): void {
    this.visibleReviewCount.set(
      TitleReviewsSection.reviewBatchSize,
    );
  }
}
