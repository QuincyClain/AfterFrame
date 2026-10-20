import { ChangeDetectionStrategy, Component, computed, input, output } from '@angular/core';
import { CatalogMediaImage, CatalogTitleMedia } from '../../data-access/catalog-api.models';

@Component({
  selector: 'app-title-media-section',
  imports: [],
  templateUrl: './title-media-section.html',
  styleUrl: './title-media-section.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TitleMediaSection {
  readonly media = input<CatalogTitleMedia | null>(null);
  readonly titleName = input.required<string>();
  readonly isLoading = input(false);
  readonly loadFailed = input(false);
  readonly playTrailer = output<void>();
  readonly openImage = output<CatalogMediaImage>();

  protected readonly visibleBackdrops = computed(() => this.media()?.backdrops.slice(0, 2) ?? []);

  protected readonly visiblePosters = computed(() => this.media()?.posters.slice(0, 2) ?? []);

  protected readonly hasMedia = computed(() => {
    const media = this.media();

    return Boolean(media?.trailer || media?.backdrops.length || media?.posters.length);
  });

  protected readonly hasImages = computed(
    () => this.visibleBackdrops().length > 0 || this.visiblePosters().length > 0,
  );

  protected readonly trailerImageUrl = computed(() => {
    const image = this.media()?.backdrops[0] ?? this.media()?.posters[0];

    return image ? this.imageUrl(image) : null;
  });

  protected imageUrl(image: CatalogMediaImage): string {
    return `https://image.tmdb.org/t/p/w780${image.path}`;
  }
}
