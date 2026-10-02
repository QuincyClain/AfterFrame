import { HttpErrorResponse } from '@angular/common/http';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { of, throwError } from 'rxjs';
import { vi } from 'vitest';
import { CatalogApi } from '../data-access/catalog-api';
import { CatalogTitleDetails } from '../data-access/catalog-api.models';
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
    externalRating: 8.5,
    externalVoteCount: 17349,
    origin: 'External',
    publicationStatus: 'Published',
    externalSource: 'tmdb',
    genres: ['Adventure', 'Animation', 'Family'],
  };

  const getTitleById = vi.fn(() => of(title));

  beforeEach(async () => {
    getTitleById.mockReset();
    getTitleById.mockReturnValue(of(title));

    await TestBed.configureTestingModule({
      imports: [TitleDetailsPage],
      providers: [
        provideRouter([]),
        {
          provide: CatalogApi,
          useValue: {
            getTitleById,
          },
        },
      ],
    }).compileComponents();

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
