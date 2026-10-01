export type TitleType = 'movie' | 'series';
export type TitleTag = 'animation' | 'anime';

export interface Title {
  readonly id: number;
  readonly title: string;
  readonly type: TitleType;
  readonly tags: readonly TitleTag[];
  readonly releaseYear: number;
  readonly description: string;
}
