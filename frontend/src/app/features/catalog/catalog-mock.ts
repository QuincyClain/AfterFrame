import { Title } from './title';

export const CATALOG_MOCK: readonly Title[] = [
  {
    id: 1,
    title: 'Interstellar',
    type: 'movie',
    tags: [],
    releaseYear: 2014,
    description:
      'A team of explorers travels through space in search of a new home for humanity.',
  },
  {
    id: 2,
    title: 'Inception',
    type: 'movie',
    tags: [],
    releaseYear: 2010,
    description:
      'A skilled team attempts to plant an idea in a person’s mind through dreams.',
  },
  {
    id: 3,
    title: 'Dark',
    type: 'series',
    tags: [],
    releaseYear: 2017,
    description:
      'A child’s disappearance uncovers the secrets of several families in a small town.',
  },
];
