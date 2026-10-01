import { Component, computed, input } from '@angular/core';

@Component({
  selector: 'app-star-rating',
  imports: [],
  templateUrl: './star-rating.html',
  styleUrl: './star-rating.css',
})
export class StarRating {
  readonly rating = input.required<number>();
  readonly max = input(10);

  protected readonly stars = computed(() =>
    Array.from({ length: this.max() }, (_, index) => index + 1),
  );

  protected readonly label = computed(
    () => `${this.rating()} out of ${this.max()} stars`,
  );

  protected getFillPercentage(star: number): number {
    const fill = this.rating() - (star - 1);

    return Math.min(Math.max(fill, 0), 1) * 100;
  }
}
