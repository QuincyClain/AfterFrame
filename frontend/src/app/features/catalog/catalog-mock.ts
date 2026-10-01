import { Title } from './title';

export const CATALOG_MOCK: readonly Title[] = [
  {
    id: 1,
    title: 'Интерстеллар',
    type: 'movie',
    tags: [],
    releaseYear: 2014,
    description:
      'Исследователи отправляются в космос в поисках нового дома для человечества.',
  },
  {
    id: 2,
    title: 'Начало',
    type: 'movie',
    tags: [],
    releaseYear: 2010,
    description:
      'Команда специалистов пытается внедрить идею в сознание человека через его сны.',
  },
  {
    id: 3,
    title: 'Тьма',
    type: 'series',
    tags: [],
    releaseYear: 2017,
    description:
      'Исчезновение ребёнка раскрывает тайны нескольких семей небольшого города.',
  },
];
