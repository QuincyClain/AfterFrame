export type WatchStatus = 'planned' | 'watching' | 'completed' | 'on-hold' | 'dropped';

export type StarRating =
  | 0.5
  | 1
  | 1.5
  | 2
  | 2.5
  | 3
  | 3.5
  | 4
  | 4.5
  | 5
  | 5.5
  | 6
  | 6.5
  | 7
  | 7.5
  | 8
  | 8.5
  | 9
  | 9.5
  | 10;

export interface SeriesProgress {
  readonly lastWatchedSeason: number;
  readonly lastWatchedEpisode: number;
}

export interface LibraryEntry {
  readonly titleId: number;
  readonly status: WatchStatus;
  readonly rating: StarRating | null;
  readonly review: string | null;
  readonly seriesProgress: SeriesProgress | null;
}

export type RatedLibraryEntry = LibraryEntry & {
  readonly rating: StarRating;
};

export function isStarRating(value: number): value is StarRating {
  return value >= 0.5 && value <= 10 && Number.isInteger(value * 2);
}
