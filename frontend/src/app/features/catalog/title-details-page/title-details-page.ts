import { HttpErrorResponse } from '@angular/common/http';
import { Component, effect, inject, input, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CatalogApi } from '../data-access/catalog-api';
import { CatalogTitleDetails } from '../data-access/catalog-api.models';
import { TitleHero } from '../title-details/title-hero/title-hero';
import { TitleSectionNav } from '../title-details/title-section-nav/title-section-nav';
import { TitleFacts } from '../title-details/title-facts/title-facts';
import { TitleActivityCard } from '../../library/title-activity-card/title-activity-card';

@Component({
  selector: 'app-title-details-page',
  imports: [RouterLink, TitleHero, TitleSectionNav, TitleFacts, TitleActivityCard],
  templateUrl: './title-details-page.html',
  styleUrl: './title-details-page.css',
})
export class TitleDetailsPage {
  readonly id = input.required<string>();

  private readonly catalogApi = inject(CatalogApi);
  private readonly reloadVersion = signal(0);

  protected readonly title = signal<CatalogTitleDetails | null>(null);
  protected readonly isLoading = signal(true);
  protected readonly isNotFound = signal(false);
  protected readonly errorMessage = signal<string | null>(null);

  constructor() {
    effect((onCleanup) => {
      this.reloadVersion();

      const titleId = this.id();

      this.title.set(null);
      this.isLoading.set(true);
      this.isNotFound.set(false);
      this.errorMessage.set(null);

      const subscription = this.catalogApi.getTitleById(titleId).subscribe({
        next: (title) => {
          this.title.set(title);
          this.isLoading.set(false);
        },
        error: (error: HttpErrorResponse) => {
          this.isLoading.set(false);

          if (error.status === 404) {
            this.isNotFound.set(true);
            return;
          }

          this.errorMessage.set('We could not load this title. Please try again.');
        },
      });

      onCleanup(() => subscription.unsubscribe());
    });
  }

  protected retry(): void {
    this.reloadVersion.update((version) => version + 1);
  }
}
