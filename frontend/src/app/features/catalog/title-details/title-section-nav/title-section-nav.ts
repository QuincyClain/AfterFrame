import { ChangeDetectionStrategy, Component, computed, input } from '@angular/core';
import { RouterLink } from '@angular/router';

export type TitleSectionId = 'overview' | 'episodes' | 'media' | 'reviews' | 'details';

interface TitleSectionLink {
  readonly id: TitleSectionId;
  readonly label: string;
}

@Component({
  selector: 'app-title-section-nav',
  imports: [RouterLink],
  templateUrl: './title-section-nav.html',
  styleUrl: './title-section-nav.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TitleSectionNav {
  readonly showEpisodes = input(false);
  readonly activeSection = input<TitleSectionId>('overview');

  protected readonly sections = computed<readonly TitleSectionLink[]>(() => {
    const sections: TitleSectionLink[] = [
      { id: 'overview', label: 'Overview' },
      { id: 'media', label: 'Media' },
      { id: 'reviews', label: 'Reviews' },
      { id: 'details', label: 'Details' },
    ];

    if (this.showEpisodes()) {
      sections.splice(1, 0, { id: 'episodes', label: 'Episodes' });
    }

    return sections;
  });
}
