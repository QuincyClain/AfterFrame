import { Component, computed, inject, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { LibraryData } from '../../library/library-data';
import { CatalogData } from '../catalog-data';

@Component({
  imports: [RouterLink],
  selector: 'app-title-details-page',
  styleUrl: './title-details-page.css',
  templateUrl: './title-details-page.html',
})
export class TitleDetailsPage {
  readonly id = input.required<string>();

  private readonly catalogData = inject(CatalogData);
  private readonly libraryData = inject(LibraryData);

  protected readonly title = computed(() =>
    this.catalogData.getTitleById(Number(this.id())),
  );

  protected readonly libraryEntry = computed(() =>
    this.libraryData.getEntry(Number(this.id())),
  );

  protected addToLibrary(): void {
    this.libraryData.addTitle(Number(this.id()));
  }
}
