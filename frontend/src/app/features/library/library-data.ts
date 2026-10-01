import { Injectable } from '@angular/core';
import { LibraryEntry, RatedLibraryEntry } from './library-entry';
import { LIBRARY_MOCK } from './library-mock';

@Injectable({
  providedIn: 'root',
})
export class LibraryData {
  getEntries(): readonly LibraryEntry[] {
    return LIBRARY_MOCK;
  }

  getRatedEntries(): readonly RatedLibraryEntry[] {
    return this.getEntries().filter((entry): entry is RatedLibraryEntry => entry.rating !== null,);
  }
}
