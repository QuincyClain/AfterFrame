import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  CatalogCastMember,
  CatalogQuery,
  CatalogTitleDetails,
  CatalogTitleSummary,
  PagedResult,
  CatalogTitleMedia,
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

  getTitleCast(titleId: string): Observable<readonly CatalogCastMember[]> {
    return this.http.get<readonly CatalogCastMember[]>(`${this.baseUrl}/${titleId}/cast`);
  }

  getTitleMedia(titleId: string): Observable<CatalogTitleMedia> {
    return this.http.get<CatalogTitleMedia>(`${this.baseUrl}/${titleId}/media`);
  }

  getTitleRecommendations(titleId: string): Observable<readonly CatalogTitleSummary[]> {
    return this.http.get<readonly CatalogTitleSummary[]>(
      `${this.baseUrl}/${titleId}/recommendations`,
    );
  }
}
