export type ReviewVibe =
  | 'Loved'
  | 'Happy'
  | 'Amused'
  | 'Excited'
  | 'Surprised'
  | 'Thoughtful'
  | 'Emotional'
  | 'Scared'
  | 'Angry'
  | 'Disappointed';

export interface TitleReview {
  readonly userId: string;
  readonly userDisplayName: string;
  readonly rating: number;
  readonly vibe: ReviewVibe | null;
  readonly review: string;
  readonly containsSpoilers: boolean;
  readonly reviewedAtUtc: string;
  readonly isCurrentUser: boolean;
}

export interface TitleReviewsSummary {
  readonly averageRating: number | null;
  readonly ratingCount: number;
  readonly reviewCount: number;
  readonly items: readonly TitleReview[];
}

export interface SaveTitleReviewRequest {
  readonly rating: number;
  readonly vibe: ReviewVibe;
  readonly review: string | null;
  readonly containsSpoilers: boolean;
}
