export type CatalogTitleType = 'Movie' | 'Series';

export type CatalogTitleTag = 'None' | 'Animation' | 'Anime' | 'Animation, Anime';

export type CatalogSort = 'Rating' | 'Votes' | 'ReleaseYear' | 'Name';

export type TitlePublicationStatus = 'Private' | 'PendingReview' | 'Published' | 'Rejected';

export interface CatalogQuery {
  readonly search?: string;
  readonly type?: CatalogTitleType;
  readonly tag?: 'Animation' | 'Anime';
  readonly genre?: string;
  readonly releaseYear?: number;
  readonly sort?: CatalogSort;
  readonly page?: number;
  readonly pageSize?: number;
}

export interface CatalogTitleSummary {
  readonly id: string;
  readonly name: string;
  readonly type: CatalogTitleType;
  readonly tags: CatalogTitleTag;
  readonly releaseYear: number;
  readonly posterPath: string | null;
  readonly externalRating: number | null;
  readonly externalVoteCount: number;
  readonly publicationStatus: TitlePublicationStatus;
  readonly genres: readonly string[];
}

export interface CatalogTitleDetails extends CatalogTitleSummary {
  readonly description: string;
  readonly backdropPath: string | null;
  readonly origin: 'External' | 'UserCreated';
  readonly externalSource: string | null;
}

export interface PagedResult<T> {
  readonly items: readonly T[];
  readonly page: number;
  readonly pageSize: number;
  readonly totalCount: number;
  readonly totalPages: number;
}

export interface CatalogCastMember {
  readonly id: string;
  readonly name: string;
  readonly character: string | null;
  readonly profilePath: string | null;
}

export interface CatalogVideo {
  readonly id: string;
  readonly name: string;
  readonly site: string;
  readonly key: string;
  readonly type: string;
  readonly official: boolean;
}

export interface CatalogMediaImage {
  readonly path: string;
  readonly width: number;
  readonly height: number;
}

export interface CatalogTitleMedia {
  readonly trailer: CatalogVideo | null;
  readonly backdrops: readonly CatalogMediaImage[];
  readonly posters: readonly CatalogMediaImage[];
}
