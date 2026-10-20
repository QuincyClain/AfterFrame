import { DOCUMENT } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  HostListener,
  computed,
  effect,
  inject,
  input,
  output,
  signal,
} from '@angular/core';
import { CatalogMediaImage } from '../../data-access/catalog-api.models';

@Component({
  selector: 'app-title-image-dialog',
  imports: [],
  templateUrl: './title-image-dialog.html',
  styleUrl: './title-image-dialog.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TitleImageDialog {
  readonly images = input.required<readonly CatalogMediaImage[]>();
  readonly initialPath = input.required<string>();
  readonly titleName = input.required<string>();
  readonly closed = output<void>();

  private readonly document = inject(DOCUMENT);
  private readonly destroyRef = inject(DestroyRef);

  protected readonly currentIndex = signal(0);

  protected readonly currentImage = computed(() => this.images()[this.currentIndex()] ?? null);

  protected readonly imageUrl = computed(() => {
    const image = this.currentImage();

    return image ? `https://image.tmdb.org/t/p/original${image.path}` : null;
  });

  protected readonly canNavigate = computed(() => this.images().length > 1);

  constructor() {
    effect(() => {
      const images = this.images();
      const initialPath = this.initialPath();
      const index = images.findIndex((image) => image.path === initialPath);

      this.currentIndex.set(index >= 0 ? index : 0);
    });

    const previousOverflow = this.document.body.style.overflow;
    this.document.body.style.overflow = 'hidden';

    this.destroyRef.onDestroy(() => {
      this.document.body.style.overflow = previousOverflow;
    });
  }

  protected previous(): void {
    const count = this.images().length;

    if (count > 1) {
      this.currentIndex.update((index) => (index - 1 + count) % count);
    }
  }

  protected next(): void {
    const count = this.images().length;

    if (count > 1) {
      this.currentIndex.update((index) => (index + 1) % count);
    }
  }

  @HostListener('document:keydown', ['$event'])
  protected handleKeydown(event: KeyboardEvent): void {
    switch (event.key) {
      case 'Escape':
        this.closed.emit();
        break;
      case 'ArrowLeft':
        this.previous();
        break;
      case 'ArrowRight':
        this.next();
        break;
    }
  }
}
