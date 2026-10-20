import { ChangeDetectionStrategy, Component, computed, input, output } from '@angular/core';
import { CatalogCastMember } from '../../data-access/catalog-api.models';

@Component({
  selector: 'app-title-cast-section',
  imports: [],
  templateUrl: './title-cast-section.html',
  styleUrl: './title-cast-section.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TitleCastSection {
  readonly cast = input<readonly CatalogCastMember[]>([]);
  readonly viewFullCast = output<void>();
  readonly isLoading = input(false);
  readonly loadFailed = input(false);

  protected readonly visibleCast = computed(() => this.cast().slice(0, 6));

  protected profileUrl(profilePath: string | null): string | null {
    return profilePath ? `https://image.tmdb.org/t/p/w342${profilePath}` : null;
  }
}
