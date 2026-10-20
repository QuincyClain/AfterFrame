import { DOCUMENT } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  DestroyRef,
  HostListener,
  effect,
  inject,
  input,
  output,
} from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { StarRatingInput } from '../../../shared/star-rating-input/star-rating-input';
import {
  ReviewVibe,
  SaveTitleReviewRequest,
  TitleReview,
} from '../data-access/reviews-api.models';
import { toSignal } from '@angular/core/rxjs-interop';

interface VibeOption {
  readonly value: ReviewVibe;
  readonly emoji: string;
  readonly label: string;
}

const VIBE_OPTIONS: readonly VibeOption[] = [
  { value: 'Loved', emoji: '😍', label: 'Loved it' },
  { value: 'Happy', emoji: '😊', label: 'Happy' },
  { value: 'Amused', emoji: '😂', label: 'Amused' },
  { value: 'Excited', emoji: '🤩', label: 'Excited' },
  { value: 'Surprised', emoji: '😮', label: 'Surprised' },
  { value: 'Thoughtful', emoji: '🤔', label: 'Thoughtful' },
  { value: 'Emotional', emoji: '🥹', label: 'Emotional' },
  { value: 'Scared', emoji: '😨', label: 'Scared' },
  { value: 'Angry', emoji: '😠', label: 'Angry' },
  { value: 'Disappointed', emoji: '😞', label: 'Disappointed' },
];

@Component({
  selector: 'app-title-review-dialog',
  imports: [ReactiveFormsModule, StarRatingInput],
  templateUrl: './title-review-dialog.html',
  styleUrl: './title-review-dialog.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TitleReviewDialog {
  readonly titleName = input.required<string>();
  readonly existingReview = input<TitleReview | null>(null);
  readonly saving = input(false);
  readonly deleting = input(false);
  readonly errorMessage = input<string | null>(null);

  readonly saveRequested = output<SaveTitleReviewRequest>();
  readonly deleteRequested = output<void>();
  readonly closed = output<void>();

  private readonly document = inject(DOCUMENT);
  private readonly destroyRef = inject(DestroyRef);

  protected readonly vibes = VIBE_OPTIONS;

  protected readonly form = new FormGroup({
    rating: new FormControl<number | null>(
      null,
      Validators.required,
    ),
    vibe: new FormControl<ReviewVibe | null>(
      null,
      Validators.required,
    ),
    review: new FormControl('', {
      nonNullable: true,
      validators: [Validators.maxLength(2000)],
    }),
    containsSpoilers: new FormControl(false, {
      nonNullable: true,
    }),
  });

  protected readonly selectedRating = toSignal(
    this.form.controls.rating.valueChanges,
    {
      initialValue: this.form.controls.rating.value,
    },
  );

  protected readonly selectedVibe = toSignal(
    this.form.controls.vibe.valueChanges,
    {
      initialValue: this.form.controls.vibe.value,
    },
  );

  protected readonly ratingPercentage = computed(
    () => (this.selectedRating() ?? 0) * 10,
  );

  protected readonly ratingMessage = computed(() => {
    const rating = this.selectedRating();

    if (rating === null) {
      return 'Choose your score';
    }

    if (rating >= 9) {
      return 'An unforgettable favorite';
    }

    if (rating >= 8) {
      return 'Highly recommended';
    }

    if (rating >= 7) {
      return 'A strong watch';
    }

    if (rating >= 5) {
      return 'Worth a look';
    }

    return 'Not quite for me';
  });

  protected readonly selectedVibeDetails = computed(() => {
    const selected = this.selectedVibe();

    return this.vibes.find((vibe) => vibe.value === selected) ?? null;
  });

  constructor() {
    effect(() => {
      const review = this.existingReview();

      this.form.reset({
        rating: review?.rating ?? null,
        vibe: review?.vibe ?? null,
        review: review?.review ?? '',
        containsSpoilers: review?.containsSpoilers ?? false,
      });
    });

    const previousOverflow = this.document.body.style.overflow;
    this.document.body.style.overflow = 'hidden';

    this.destroyRef.onDestroy(() => {
      this.document.body.style.overflow = previousOverflow;
    });
  }

  protected setRating(rating: number): void {
    this.form.controls.rating.setValue(rating);
    this.form.controls.rating.markAsTouched();
  }

  protected selectVibe(vibe: ReviewVibe): void {
    this.form.controls.vibe.setValue(vibe);
    this.form.controls.vibe.markAsTouched();
  }

  protected submit(): void {
    if (this.form.invalid || this.saving() || this.deleting()) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();

    if (value.rating === null || value.vibe === null) {
      return;
    }

    const normalizedReview = value.review.trim();

    this.saveRequested.emit({
      rating: value.rating,
      vibe: value.vibe,
      review: normalizedReview || null,
      containsSpoilers:
        normalizedReview.length > 0 && value.containsSpoilers,
    });
  }

  protected requestDelete(): void {
    if (this.saving() || this.deleting()) {
      return;
    }

    const confirmed =
      this.document.defaultView?.confirm(
        'Remove your rating and review for this title?',
      ) ?? false;

    if (confirmed) {
      this.deleteRequested.emit();
    }
  }

  protected close(): void {
    if (!this.saving() && !this.deleting()) {
      this.closed.emit();
    }
  }

  @HostListener('document:keydown.escape')
  protected closeOnEscape(): void {
    this.close();
  }
}
