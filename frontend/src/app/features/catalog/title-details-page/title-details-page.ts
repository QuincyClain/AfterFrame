import { Component, computed, inject, input } from '@angular/core';
import { CatalogData } from '../catalog-data';
import { RouterLink } from '@angular/router';

@Component({
  imports: [RouterLink],
  selector: 'app-title-details-page',
  styleUrl: './title-details-page.css',
  templateUrl: './title-details-page.html',
})
export class TitleDetailsPage {
  readonly id = input.required<string>();

  private readonly catalogData = inject(CatalogData);

  protected readonly title = computed(() =>
    this.catalogData.getTitleById(Number(this.id())),
  );
}
