import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  SaveTitleReviewRequest,
  TitleReviewsSummary,
} from './reviews-api.models';

@Injectable({
  providedIn: 'root',
})
export class ReviewsApi {
  private readonly http = inject(HttpClient);

  getTitleReviews(titleId: string): Observable<TitleReviewsSummary> {
    return this.http.get<TitleReviewsSummary>(
      `/api/catalog/${titleId}/reviews`,
    );
  }

  saveTitleReview(
    titleId: string,
    request: SaveTitleReviewRequest,
  ): Observable<void> {
    return this.http.put<void>(
      `/api/library/${titleId}/review`,
      request,
    );
  }

  deleteTitleReview(titleId: string): Observable<void> {
    return this.http.delete<void>(
      `/api/library/${titleId}/review`,
    );
  }
}
