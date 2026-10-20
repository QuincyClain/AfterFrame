import { HttpErrorResponse } from '@angular/common/http';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { of, throwError } from 'rxjs';
import { vi } from 'vitest';
import { CatalogApi } from '../data-access/catalog-api';
import {
  CatalogTitleDetails,
  CatalogTitleMedia,
  CatalogTitleSummary,
} from '../data-access/catalog-api.models';
import { TitleDetailsPage } from './title-details-page';

describe('TitleDetailsPage', () => {
  let fixture: ComponentFixture<TitleDetailsPage>;

  const titleId = '0199f3f4-7c00-7000-8000-000000000101';

  const title: CatalogTitleDetails = {
    id: titleId,
    name: 'Spirited Away',
    type: 'Movie',
    tags: 'Animation, Anime',
    releaseYear: 2001,
    description:
      'A young girl enters a mysterious world ruled by spirits and must find a way to save her parents.',
    posterPath: '/39wmItIWsg5sZMyRUHLkWBcuVCM.jpg',
    backdropPath: '/x2RS3uTcsJJ9IfjNPcgDmukoEcQ.jpg',
    externalRating: 8.5,
    externalVoteCount: 17349,
    origin: 'External',
    publicationStatus: 'Published',
    externalSource: 'tmdb',
    genres: ['Adventure', 'Animation', 'Family'],
  };

  const media: CatalogTitleMedia = {
    trailer: {
      id: 'trailer-1',
      name: 'Official Trailer',
      site: 'YouTube',
      key: 'example-video-key',
      type: 'Trailer',
      official: true,
    },
    backdrops: [],
    posters: [],
  };

  const recommendations: readonly CatalogTitleSummary[] = [
    {
      id: '0199f3f4-7c00-7000-8000-000000000102',
      name: 'Princess Mononoke',
      type: 'Movie',
      tags: 'Animation, Anime',
      releaseYear: 1997,
      posterPath: '/cMYCDADoLKLbB83g4WnJegaZimC.jpg',
      externalRating: 8.3,
      externalVoteCount: 9335,
      publicationStatus: 'Published',
      genres: ['Adventure', 'Animation', 'Fantasy'],
    },
  ];

  const getTitleRecommendations = vi.fn(() => of(recommendations));

  const getTitleMedia = vi.fn(() => of(media));

  const getTitleCast = vi.fn(() =>
    of([
      {
        id: '85',
        name: 'Hayao Miyazaki',
        character: 'Additional Voices',
        profilePath: null,
      },
    ]),
  );

  const getTitleById = vi.fn(() => of(title));

  beforeEach(async () => {
    getTitleById.mockReset();
    getTitleById.mockReturnValue(of(title));
    getTitleMedia.mockReset();
    getTitleMedia.mockReturnValue(of(media));
    getTitleRecommendations.mockReset();
    getTitleRecommendations.mockReturnValue(of(recommendations));

    await TestBed.configureTestingModule({
      imports: [TitleDetailsPage],
      providers: [
        provideRouter([]),
        {
          provide: CatalogApi,
          useValue: {
            getTitleById,
            getTitleCast,
            getTitleMedia,
            getTitleRecommendations,
          },
        },
      ],
    }).compileComponents();

    getTitleCast.mockReset();
    getTitleCast.mockReturnValue(
      of([
        {
          id: '85',
          name: 'Hayao Miyazaki',
          character: 'Additional Voices',
          profilePath: null,
        },
      ]),
    );

    fixture = TestBed.createComponent(TitleDetailsPage);
    fixture.componentRef.setInput('id', titleId);
    fixture.detectChanges();

    await fixture.whenStable();
    fixture.detectChanges();
  });

  it('should request and render the selected title', () => {
    const element = fixture.nativeElement as HTMLElement;

    expect(getTitleById).toHaveBeenCalledWith(titleId);
    expect(element.querySelector('h1')?.textContent).toContain('Spirited Away');
    expect(element.textContent).toContain('Movie');
    expect(element.textContent).toContain('2001');
    expect(element.textContent).toContain('8.5');
    expect(element.textContent).toContain('Adventure');
    expect(element.querySelector('.back-link')?.getAttribute('href')).toBe('/catalog');
    expect(element.querySelector<HTMLElement>('.title-hero')?.style.backgroundImage).toContain(
      title.backdropPath,
    );
    expect(getTitleCast).toHaveBeenCalledWith(titleId);
    expect(element.textContent).toContain('Hayao Miyazaki');
    expect(getTitleMedia).toHaveBeenCalledWith(titleId);

    const trailerButton = Array.from(element.querySelectorAll<HTMLButtonElement>('button')).find(
      (button) => button.textContent?.includes('Play trailer'),
    );

    expect(trailerButton).toBeTruthy();

    trailerButton?.click();
    fixture.detectChanges();

    expect(element.querySelector('.trailer-dialog')).toBeTruthy();
    expect(element.querySelector('iframe')?.getAttribute('title')).toBe('Official Trailer');
    expect(element.textContent).toContain('Trailer & Media');
    expect(element.textContent).toContain('Official Trailer');
    expect(getTitleRecommendations).toHaveBeenCalledWith(titleId);
    expect(getTitleRecommendations).toHaveBeenCalledWith(titleId);
    expect(element.textContent).toContain('You might also like');
    expect(element.textContent).toContain('Princess Mononoke');
  });

  it('should render a not-found state for an unavailable title', async () => {
    const missingTitleId = '0199f3f4-7c00-7000-8000-000000000999';

    getTitleById.mockReturnValueOnce(throwError(() => new HttpErrorResponse({ status: 404 })));

    fixture.componentRef.setInput('id', missingTitleId);
    fixture.detectChanges();

    await fixture.whenStable();
    fixture.detectChanges();

    const element = fixture.nativeElement as HTMLElement;

    expect(getTitleById).toHaveBeenLastCalledWith(missingTitleId);
    expect(element.querySelector('h1')?.textContent).toContain('Title not found');
  });
});
