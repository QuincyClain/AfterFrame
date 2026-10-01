import { Component, computed, effect, input, output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { toSignal } from '@angular/core/rxjs-interop';
import { TitleType } from '../../catalog/title';
import { StarRatingInput } from '../../../shared/star-rating-input/star-rating-input';
import { isStarRating, LibraryEntry, StarRating, WatchStatus } from '../library-entry';

interface WatchStatusOption {
  readonly value: WatchStatus;
  readonly label: string;
}

const WATCH_STATUS_OPTIONS: readonly WatchStatusOption[] = [
  { value: 'planned', label: 'Plan to watch' },
  { value: 'watching', label: 'Watching' },
  { value: 'completed', label: 'Completed' },
  { value: 'on-hold', label: 'On hold' },
  { value: 'dropped', label: 'Dropped' },
];

@Component({
  selector: 'app-library-entry-form',
  imports: [ReactiveFormsModule, StarRatingInput],
  templateUrl: './library-entry-form.html',
  styleUrl: './library-entry-form.css',
})
export class LibraryEntryForm {
  readonly entry = input.required<LibraryEntry>();
  readonly titleType = input.required<TitleType>();

  readonly entrySaved = output<LibraryEntry>();
  readonly cancelled = output<void>();

  protected readonly statuses = WATCH_STATUS_OPTIONS;

  protected readonly form = new FormGroup({
    status: new FormControl<WatchStatus>('planned', { nonNullable: true }),
    rating: new FormControl<StarRating | null>(null),
  });

  private readonly selectedStatus = toSignal(this.form.controls.status.valueChanges, {
    initialValue: this.form.controls.status.value,
  });

  protected readonly canRate = computed(() => this.isRatingAllowed(this.selectedStatus()));

  constructor() {
    effect(() => {
      const entry = this.entry();

      this.form.reset({ status: entry.status, rating: entry.rating });
    });
  }

  protected setRating(rating: number): void {
    if (isStarRating(rating)) {
      this.form.controls.rating.setValue(rating);
    }
  }

  protected saveChanges(): void {
    const entry = this.entry();
    const status = this.form.controls.status.value;
    const ratingAllowed = this.isRatingAllowed(status);

    let rating = this.form.controls.rating.value;
    let review = entry.review;
    let seriesProgress = entry.seriesProgress;

    const hasDataToRemove = rating !== null || review !== null || seriesProgress !== null;

    if (!ratingAllowed && hasDataToRemove) {
      const confirmed = window.confirm(
        'The selected status does not allow a rating, review, or series progress. Continue and remove this personal data?',
      );

      if (!confirmed) {
        return;
      }

      rating = null;
      review = null;
      seriesProgress = null;
    }

    this.entrySaved.emit({
      ...entry,
      status,
      rating,
      review,
      seriesProgress,
    });
  }

  protected cancel(): void {
    this.cancelled.emit();
  }

  private isRatingAllowed(status: WatchStatus): boolean {
    if (this.titleType() === 'movie') {
      return status === 'completed' || status === 'dropped';
    }

    return status !== 'planned';
  }
}
