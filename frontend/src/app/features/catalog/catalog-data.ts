import { Injectable } from '@angular/core';
import { CATALOG_MOCK } from './catalog-mock';
import { Title } from './title';

@Injectable({
  providedIn: 'root',
})
export class CatalogData {
  getTitles(): readonly Title[] {
    return CATALOG_MOCK;
  }

  getTitleById(id: number): Title | undefined {
    return CATALOG_MOCK.find((title) => title.id === id);
  }
}
