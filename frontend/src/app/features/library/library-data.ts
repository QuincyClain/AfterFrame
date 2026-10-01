import { computed, Injectable, signal } from '@angular/core';
import { LibraryEntry, RatedLibraryEntry } from './library-entry';
import { LIBRARY_MOCK } from './library-mock';

@Injectable({
  providedIn: 'root',
})
export class LibraryData {
  private readonly entriesState = signal<readonly LibraryEntry[]>(LIBRARY_MOCK);

  readonly entries = this.entriesState.asReadonly();

  readonly ratedEntries = computed(() =>
    this.entries().filter((entry): entry is RatedLibraryEntry => entry.rating !== null),
  );

  getEntry(titleId: number): LibraryEntry | undefined {
    return this.entries().find((entry) => entry.titleId === titleId);
  }

  addTitle(titleId: number): void {
    if (this.getEntry(titleId)) {
      return;
    }

    const entry: LibraryEntry = {
      titleId,
      status: 'planned',
      rating: null,
      review: null,
      seriesProgress: null,
    };

    this.entriesState.update((entries) => [...entries, entry]);
  }

  updateEntry(updatedEntry: LibraryEntry): void {
    this.entriesState.update((entries) =>
      entries.map((entry) => (entry.titleId === updatedEntry.titleId ? updatedEntry : entry)),
    );
  }
}
