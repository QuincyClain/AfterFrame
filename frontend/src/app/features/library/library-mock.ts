import { LibraryEntry } from './library-entry';

export const LIBRARY_MOCK: readonly LibraryEntry[] = [
  {
    titleId: 1,
    status: 'completed',
    rating: 9.5,
    review: 'An ambitious and emotional science-fiction story with an unforgettable score.',
    seriesProgress: null,
  },
  {
    titleId: 2,
    status: 'completed',
    rating: 8.5,
    review: 'A clever concept supported by strong pacing, visuals, and layered storytelling.',
    seriesProgress: null,
  },
  {
    titleId: 3,
    status: 'watching',
    rating: 9,
    review: 'A carefully constructed mystery that rewards attention and patience.',
    seriesProgress: {
      lastWatchedSeason: 2,
      lastWatchedEpisode: 6,
    },
  },
];
