import { Component, computed, input, output, signal } from '@angular/core';

@Component({
  selector: 'app-star-rating-input',
  imports: [],
  templateUrl: './star-rating-input.html',
  styleUrl: './star-rating-input.css',
})
export class StarRatingInput {
  readonly rating = input<number | null>(null);
  readonly max = input(10);
  readonly disabled = input(false);

  readonly ratingChange = output<number>();

  protected readonly previewRating = signal<number | null>(null);

  protected readonly stars = computed(() =>
    Array.from({ length: this.max() }, (_, index) => index + 1),
  );

  protected readonly displayedRating = computed(() => this.previewRating() ?? this.rating());

  protected readonly label = computed(() => {
    const rating = this.rating();

    return rating === null ? 'Not rated' : `${rating} out of ${this.max()} stars`;
  });

  protected getFillPercentage(star: number): number {
    const rating = this.displayedRating() ?? 0;
    const fill = rating - (star - 1);

    return Math.min(Math.max(fill, 0), 1) * 100;
  }

  protected previewFromPointer(event: MouseEvent): void {
    if (!this.disabled()) {
      this.previewRating.set(this.getRatingFromPointer(event));
    }
  }

  protected selectFromPointer(event: MouseEvent): void {
    if (!this.disabled()) {
      this.ratingChange.emit(this.getRatingFromPointer(event));
    }
  }

  protected clearPreview(): void {
    this.previewRating.set(null);
  }

  protected handleKeydown(event: KeyboardEvent): void {
    if (this.disabled()) {
      return;
    }

    const currentRating = this.rating();
    let nextRating: number | null = null;

    switch (event.key) {
      case 'ArrowRight':
      case 'ArrowUp':
        nextRating = Math.min(this.max(), (currentRating ?? 0) + 0.5);
        break;

      case 'ArrowLeft':
      case 'ArrowDown':
        nextRating = currentRating === null ? 0.5 : Math.max(0.5, currentRating - 0.5);
        break;

      case 'Home':
        nextRating = 0.5;
        break;

      case 'End':
        nextRating = this.max();
        break;
    }

    if (nextRating !== null) {
      event.preventDefault();
      this.ratingChange.emit(nextRating);
    }
  }

  private getRatingFromPointer(event: MouseEvent): number {
    const element = event.currentTarget as HTMLElement;
    const bounds = element.getBoundingClientRect();
    const pointerPosition = event.clientX - bounds.left;
    const ratio = Math.min(Math.max(pointerPosition / bounds.width, 0), 1);
    const rating = Math.ceil(ratio * this.max() * 2) / 2;

    return Math.min(Math.max(rating, 0.5), this.max());
  }
}
