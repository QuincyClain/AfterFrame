import { DecimalPipe } from '@angular/common';
import { Component, computed, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CatalogTitleSummary } from '../data-access/catalog-api.models';

@Component({
  selector: 'app-title-card',
  imports: [DecimalPipe, RouterLink],
  templateUrl: './title-card.html',
  styleUrl: './title-card.css',
})
export class TitleCard {
  readonly title = input.required<CatalogTitleSummary>();

  protected readonly posterUrl = computed(() => {
    const posterPath = this.title().posterPath;

    return posterPath ? `https://image.tmdb.org/t/p/w500${posterPath}` : null;
  });
}
