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
}
