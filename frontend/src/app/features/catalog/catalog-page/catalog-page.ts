import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Component, computed, DestroyRef, inject, signal } from '@angular/core';
import { finalize } from 'rxjs';
import { CatalogApi } from '../data-access/catalog-api';
import { CatalogQuery, CatalogSort, CatalogTitleSummary } from '../data-access/catalog-api.models';
import { TitleCard } from '../title-card/title-card';

type CatalogFilter = 'all' | 'movies' | 'series' | 'animation' | 'anime';

interface SortOption {
  readonly value: CatalogSort;
  readonly label: string;
}

@Component({
  selector: 'app-catalog-page',
  imports: [TitleCard],
  templateUrl: './catalog-page.html',
  styleUrl: './catalog-page.css',
})
export class CatalogPage {
  private readonly catalogApi = inject(CatalogApi);
  private readonly destroyRef = inject(DestroyRef);

  protected readonly titles = signal<readonly CatalogTitleSummary[]>([]);
  protected readonly totalCount = signal(0);
  protected readonly totalPages = signal(0);
  protected readonly isLoading = signal(false);
  protected readonly errorMessage = signal<string | null>(null);
  protected readonly searchTerm = signal('');
  protected readonly appliedSearchTerm = signal('');
  protected readonly selectedFilter = signal<CatalogFilter>('all');
  protected readonly sort = signal<CatalogSort>('Rating');
  protected readonly isSortOpen = signal(false);
  protected readonly page = signal(1);

  protected readonly sortOptions: readonly SortOption[] = [
    { value: 'Rating', label: 'Rating' },
    { value: 'Votes', label: 'Most voted' },
    { value: 'ReleaseYear', label: 'Newest' },
    { value: 'Name', label: 'Name' },
  ];

  protected readonly sortLabel = computed(
    () => this.sortOptions.find((option) => option.value === this.sort())?.label ?? 'Rating',
  );

  constructor() {
    this.loadCatalog();
  }

  protected retry(): void {
    this.loadCatalog();
  }

  protected applySearch(): void {
    this.appliedSearchTerm.set(this.searchTerm().trim());
    this.page.set(1);
    this.loadCatalog();
  }

  protected updateSearchTerm(value: string): void {
    this.searchTerm.set(value);

    if (value === '' && this.appliedSearchTerm() !== '') {
      this.appliedSearchTerm.set('');
      this.page.set(1);
      this.loadCatalog();
    }
  }

  protected setFilter(filter: CatalogFilter): void {
    this.selectedFilter.set(filter);
    this.page.set(1);
    this.loadCatalog();
  }

  protected toggleSort(): void {
    this.isSortOpen.update((isOpen) => !isOpen);
  }

  protected selectSort(sort: CatalogSort): void {
    this.sort.set(sort);
    this.isSortOpen.set(false);
    this.page.set(1);
    this.loadCatalog();
  }

  protected previousPage(): void {
    if (this.page() > 1) {
      this.page.update((page) => page - 1);
      this.loadCatalog();
    }
  }

  protected nextPage(): void {
    if (this.page() < this.totalPages()) {
      this.page.update((page) => page + 1);
      this.loadCatalog();
    }
  }

  private loadCatalog(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.catalogApi
      .getCatalog(this.buildQuery())
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        finalize(() => this.isLoading.set(false)),
      )
      .subscribe({
        next: (result) => {
          this.titles.set(result.items);
          this.totalCount.set(result.totalCount);
          this.totalPages.set(result.totalPages);
        },
        error: () => {
          this.errorMessage.set('The catalog could not be loaded. Please try again.');
        },
      });
  }

  private buildQuery(): CatalogQuery {
    const query: CatalogQuery = {
      search: this.appliedSearchTerm() || undefined,
      sort: this.sort(),
      page: this.page(),
      pageSize: 24,
    };

    return {
      ...query,
      ...(this.selectedFilter() === 'movies' && { type: 'Movie' as const }),
      ...(this.selectedFilter() === 'series' && { type: 'Series' as const }),
      ...(this.selectedFilter() === 'animation' && { tag: 'Animation' as const }),
      ...(this.selectedFilter() === 'anime' && { tag: 'Anime' as const }),
    };
  }
}
