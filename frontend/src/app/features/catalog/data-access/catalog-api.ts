import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  CatalogQuery,
  CatalogTitleDetails,
  CatalogTitleSummary,
  PagedResult,
} from './catalog-api.models';

@Injectable({
  providedIn: 'root',
})
export class CatalogApi {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/catalog';

  getCatalog(query: CatalogQuery = {}): Observable<PagedResult<CatalogTitleSummary>> {
    let params = new HttpParams();

    for (const [key, value] of Object.entries(query)) {
      if (value !== undefined && value !== null && value !== '') {
        params = params.set(key, String(value));
      }
    }

    return this.http.get<PagedResult<CatalogTitleSummary>>(this.baseUrl, { params });
  }

  getTitleById(titleId: string): Observable<CatalogTitleDetails> {
    return this.http.get<CatalogTitleDetails>(`${this.baseUrl}/${titleId}`);
  }
}
