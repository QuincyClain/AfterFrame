import { RouterLink } from '@angular/router';
import { LibraryData } from '../../library/library-data';
import { CatalogData } from '../catalog-data';
import { Component, computed, inject, input, signal } from '@angular/core';
import { LibraryEntryForm } from '../../library/library-entry-form/library-entry-form';
import { LibraryEntry } from '../../library/library-entry';

@Component({
  imports: [LibraryEntryForm, RouterLink],
  selector: 'app-title-details-page',
  styleUrl: './title-details-page.css',
  templateUrl: './title-details-page.html',
})
export class TitleDetailsPage {
  readonly id = input.required<string>();

  private readonly catalogData = inject(CatalogData);
  private readonly libraryData = inject(LibraryData);
  protected readonly isEditing = signal(false);

  protected startEditing(): void {
    this.isEditing.set(true);
  }

  protected cancelEditing(): void {
    this.isEditing.set(false);
  }

  protected saveEntry(entry: LibraryEntry): void {
    this.libraryData.updateEntry(entry);
    this.isEditing.set(false);
  }

  protected readonly title = computed(() => this.catalogData.getTitleById(Number(this.id())));

  protected readonly libraryEntry = computed(() => this.libraryData.getEntry(Number(this.id())));

  protected addToLibrary(): void {
    this.libraryData.addTitle(Number(this.id()));
  }
}
