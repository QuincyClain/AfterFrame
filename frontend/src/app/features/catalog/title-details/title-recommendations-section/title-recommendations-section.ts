import {
  Component,
  effect,
  ElementRef,
  input,
  signal,
  viewChild,
} from '@angular/core';
import { CatalogTitleSummary } from '../../data-access/catalog-api.models';
import { TitleCard } from '../../title-card/title-card';

@Component({
  selector: 'app-title-recommendations-section',
  imports: [TitleCard],
  templateUrl: './title-recommendations-section.html',
  styleUrl: './title-recommendations-section.css',
})
export class TitleRecommendationsSection {
  readonly recommendations = input<readonly CatalogTitleSummary[]>([]);
  readonly isLoading = input(false);
  readonly loadFailed = input(false);

  private readonly track =
    viewChild<ElementRef<HTMLDivElement>>('recommendationsTrack');

  protected readonly skeletonItems = [0, 1, 2, 3] as const;
  protected readonly hasOverflow = signal(false);

  constructor() {
    effect((onCleanup) => {
      this.recommendations();

      const element = this.track()?.nativeElement;

      if (!element) {
        this.hasOverflow.set(false);
        return;
      }

      const updateOverflow = () => {
        this.hasOverflow.set(element.scrollWidth > element.clientWidth + 1);
      };

      updateOverflow();

      if (typeof ResizeObserver === 'undefined') {
        return;
      }

      const observer = new ResizeObserver(updateOverflow);
      observer.observe(element);

      onCleanup(() => observer.disconnect());
    });
  }

  protected scroll(direction: -1 | 1): void {
    const element = this.track()?.nativeElement;

    if (!element) {
      return;
    }

    const distance = Math.max(element.clientWidth * 0.8, 280);

    element.scrollBy({
      left: distance * direction,
      behavior: 'smooth',
    });
  }
}
